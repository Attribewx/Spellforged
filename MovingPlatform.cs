using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public abstract class MovingPlatform : MonoBehaviour
{

    public Vector3 speed;
    public Movement playerSpeeds;
    public BoxCollider2D boxer;
    public Rigidbody2D rb;
    public List<Rigidbody2D> rigs;
    public float Yoffset;
    [SerializeField]
    private float platformStrScalar = 50;

    [Header("Lights Setup"), Space(10)]
    public GameObject indicator;
    [HideInInspector] public Light2D lightColor;
    [HideInInspector] public Color startLightColor;
    public Color changeLightColor;
    [HideInInspector] public SpriteRenderer sR;
    [HideInInspector] public Color spriteStartColor;
    public Color spriteChangeColor;
    [HideInInspector] public Material startMaterial;
    public Material changeMaterial;

    public void Start()
    {
        playerSpeeds = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        boxer = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        //// LIGHT INITIALIZATION \\\\
        if(indicator)
        {
            if (indicator.GetComponent<Light2D>())
                lightColor = indicator.GetComponent<Light2D>();
            else if (indicator.GetComponentInChildren<Light2D>())
                lightColor = indicator.GetComponentInChildren<Light2D>();
            if (lightColor)
                startLightColor = lightColor.color;

            if (indicator.GetComponent<SpriteRenderer>())
                sR = indicator.GetComponent<SpriteRenderer>();
            else if (indicator.GetComponentInChildren<SpriteRenderer>())
                sR = indicator.GetComponentInChildren<SpriteRenderer>();

            if(sR)
            {
                if(sR.material)
                    startMaterial = sR.material;
                if (sR.material)
                    startMaterial = sR.material;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = speed * Time.fixedDeltaTime;
        if (rigs.Count > 0)
        {
            for (int i = 0; i < rigs.Count; i++)
            {
                Rigidbody2D rug = rigs[i];
                if(speed.y <= 0) //Downward Diagonal Movement
                {
                    rug.velocity = (Vector3)rug.velocity + (speed * Time.fixedDeltaTime);
                }else if(speed.y > 0 && speed.x != 0)   //Diagonal upward Movement
                {
                    rug.velocity = new Vector3(rug.velocity.x + (speed.x * Time.fixedDeltaTime), rug.velocity.y);
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            RaycastHit2D boxChek = Physics2D.BoxCast(boxer.bounds.center + new Vector3(0, .1f, 0), boxer.size, 0, Vector2.up, 5, 1 << LayerMask.NameToLayer("Player"));

            RaycastHit2D boxChekL = Physics2D.BoxCast(boxer.bounds.center + new Vector3(-1, -.1f, 0), boxer.size, 0, Vector2.left, 5, 1 << LayerMask.NameToLayer("Player"));
            RaycastHit2D boxChekR = Physics2D.BoxCast(boxer.bounds.center + new Vector3(1, -.1f, 0), boxer.size, 0, Vector2.right, 5, 1 << LayerMask.NameToLayer("Player"));

            Rigidbody2D rib = null;

            if (boxChek && boxChekL.collider == null && boxChekR.collider == null)
                rib = boxChek.collider.GetComponent<Rigidbody2D>();
            if(rib != null)
            {
                Add(rib);
            }
            //if (boxChek.rigidbody)
                //rigs.Add(boxChek.rigidbody);
               // collision.collider.transform.SetParent(transform);


            //Debug.Log(boxChek.collider);
            //if (collision.collider.transform.position.y > transform.position.y)
            //{

            //}
            //playerSpeeds.ChangePlatformForces(0, 0, true);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (speed.y > 280 && playerSpeeds.isJumping && collision.collider.transform.position.y > transform.position.y + Yoffset)
                playerSpeeds.jOverride = speed.y / platformStrScalar;
            else
                playerSpeeds.jOverride = 0;
            if (playerSpeeds.isDashins)
            {
                Rigidbody2D rib = collision.collider.GetComponent<Rigidbody2D>();
                if (rib != null)
                {
                    Remove(rib);
                }
            }
            else
            {
               // RaycastHit2D boxChek = Physics2D.BoxCast(boxer.bounds.center + new Vector3(0,.1f,0), boxer.size, 0, Vector2.up, 5, 1 << LayerMask.NameToLayer("Player"));
               // if (boxChek)
               // {
                //    collision.collider.transform.SetParent(transform);
                //    Debug.Log(boxChek.collider);
                //}
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        RaycastHit2D boxChek = Physics2D.BoxCast(boxer.bounds.center + new Vector3(0, .1f, 0), boxer.size, 0, Vector2.up, 5, 1 << LayerMask.NameToLayer("Player"));

        Rigidbody2D rib = collision.collider.GetComponent<Rigidbody2D>();
        if (rib != null)
        {
            Remove(rib);
        }
        if (collision.collider.tag == "Player")
        {
            if(collision.collider.transform.position.y > transform.position.y + Yoffset)
            {
                playerSpeeds.airSpeed = speed.x / platformStrScalar;
                if (speed.y <= 280)
                {
                    playerSpeeds.yairSpeed = speed.y / platformStrScalar;
                }
                if (!playerSpeeds.isJumping)
                {
                    playerSpeeds.jOverride = 0;
                    Debug.Log("222");
                }

                if(speed.y < 0)
                {
                    rib.velocity = (Vector3)rib.velocity + (.6f * -speed * Time.fixedDeltaTime);
                    Debug.Log("Slowing");
                }
            }
        }

    }

    void Add(Rigidbody2D rig)
    {
        if(!rigs.Contains(rig))
        {
            rigs.Add(rig);
        }
    }

    void Remove(Rigidbody2D rig)
    {
        if (rigs.Contains(rig))
        {
            rigs.Remove(rig);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.up * Yoffset, Color.green);
    }

    public void ChangeSpriteColors(Color newColor)
    {
        if(sR)
        {
            sR.color = newColor;
        }
    }

    public void ChangeMaterial(Material material)
    {
        if (sR)
        {
            sR.material = material;
        }
    }

    public void ChangeLightColors(Color newColor)
    {
        if (lightColor)
        {
            lightColor.color = newColor;
        }
    }
}
