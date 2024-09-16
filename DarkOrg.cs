using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrg : Projectile
{
    [Header("Homing Soulmass Setup"), Space(20) ] public CircleCollider2D circleOH;
    private Transform closestEnemi;
    public Rigidbody2D movieo;
    private bool foundTarg;
    [SerializeField]private float shortDist;
    [SerializeField] private float searchRange;
    [SerializeField] private float playerFollowSpeed;
    [SerializeField] private static float orbLimit = 3;

    [SerializeField, Header("Spacings"), Space(20)] private float yOffset1 = .75f;
    [SerializeField] private float xOffset1 = .75f;

    private Collider2D[] Enemies;
    private static DarkOrg[] orbs = new DarkOrg[5];



    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i] == null)
            {
                Debug.Log(orbs[i]);
                orbs[i] = this;
                break;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!foundTarg)
        { 
            Enemies = Physics2D.OverlapCircleAll(transform.position, searchRange, 1 << LayerMask.NameToLayer("EnemyHBox"));
            for (int i = 0; i < Enemies.Length; i++)
            {
                RaycastHit2D ronald = Physics2D.Raycast(transform.position, Enemies[i].transform.position - transform.position, searchRange, (1 << LayerMask.NameToLayer("EnemyHBox") | (1 << LayerMask.NameToLayer("Ground"))));
                if (ronald)
                {
                    if(ronald.collider.name == Enemies[i].transform.name)
                    {
                        //Debug.Log(ronald.collider.name);
                        if (ronald.distance < shortDist)
                        {
                            //Debug.Log("LETS GO");
                            shortDist = ronald.distance;
                            closestEnemi = ronald.collider.transform;
                            foundTarg = true;
                        }
                    }
                }
            }
        }

        if(closestEnemi)
        {
            if (!(closestEnemi.position.x - transform.position.x < .1f && closestEnemi.position.x - transform.position.x > -.1f) || !(closestEnemi.position.y - transform.position.y < .1f && closestEnemi.position.y - transform.position.y > -.1f))
                movieo.velocity = (closestEnemi.position - transform.position).normalized * speed.x;
            else
            {
                closestEnemi = null;
                foundTarg = false;
            }
        }
        else
        {
            int orbInt = -1;
            for (int i = 0; i < orbs.Length; i++)
            {
                if (orbs[i] == this)
                {
                    orbInt = i;
                }
            }
            switch (orbInt)
            {
                case 0:
                    if (!(playerPos.position.x - transform.position.x < .2f && playerPos.position.x - transform.position.x > -.2f) || !(playerPos.position.y + 1.5f - transform.position.y < .2f && playerPos.position.y + 1.5f - transform.position.y > -.2f))
                        movieo.velocity = ((playerPos.position + new Vector3(0, 1.5f)) - transform.position).normalized * playerFollowSpeed;
                    else
                        movieo.velocity = Vector2.zero;
                    break;
                case 1:
                    if (!(playerPos.position.y + .5f - transform.position.y < .2f && playerPos.position.y + .5f - transform.position.y > -.2f) || !(playerPos.position.x + -1.5f - transform.position.x < .2f && playerPos.position.x + -1.5f - transform.position.x > -.2f))
                        movieo.velocity = ((playerPos.position + new Vector3(-1.5f, .5f)) - transform.position).normalized * playerFollowSpeed;
                    else
                        movieo.velocity = Vector2.zero;
                    break;
                case 2:
                    if (!(playerPos.position.y + .5f - transform.position.y < .2f && playerPos.position.y + .5f - transform.position.y > -.2f) || !(playerPos.position.x + 1.5f - transform.position.x < .2f && playerPos.position.x + 1.5f - transform.position.x > -.2f))
                        movieo.velocity = ((playerPos.position + new Vector3(1.5f, .5f)) - transform.position).normalized * playerFollowSpeed;
                    else
                        movieo.velocity = Vector2.zero;
                    break;
                case 3:
                    if (!(playerPos.position.y + yOffset1 - transform.position.y < .2f && playerPos.position.y + yOffset1 - transform.position.y > -.2f) || !(playerPos.position.x - xOffset1 - transform.position.x < .2f && playerPos.position.x - xOffset1 - transform.position.x > -.2f))
                        movieo.velocity = ((playerPos.position + new Vector3(-xOffset1, yOffset1)) - transform.position).normalized * playerFollowSpeed;
                    else
                        movieo.velocity = Vector2.zero;
                    break;
                case 4:
                    if (!(playerPos.position.y + yOffset1 - transform.position.y < .2f && playerPos.position.y + yOffset1 - transform.position.y > -.2f) || !(playerPos.position.x + xOffset1 - transform.position.x < .2f && playerPos.position.x + xOffset1 - transform.position.x > -.2f))
                        movieo.velocity = ((playerPos.position + new Vector3(xOffset1, yOffset1)) - transform.position).normalized * playerFollowSpeed;
                    else
                        movieo.velocity = Vector2.zero;
                    break;
            }
            
        }
    }

    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Instantiate(pSExplosion, transform.position, Quaternion.identity);
        }

        if (collision.tag == "Floor")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Instantiate(pSExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (gameObject.tag == "Gray Magic")
        {
            if (collision.tag != "Enemy Magic")
            {
                if (pS != null)
                {
                    pS.GetComponent<ParticleSystem>().Stop();
                    pS.parent = null;
                    pS.localScale = new Vector3(1, 1, 1);
                    Destroy(pS.gameObject, 3);
                }
                Destroy(gameObject);
                Instantiate(pSExplosion, transform.position, Quaternion.identity);
            }
        }
    }

    public static DarkOrg[] GetOrbs()
    {
        return orbs;
    }

    public static int NullOrbCheck()
    {
        if(Inventory.instance.FindEquippedRune(Runes.Rune_of_Triangle))
        {
            for (int i = 0; i < orbs.Length; i++)
            {
                if (orbs[i] == null)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < orbLimit ; i++)
            {
                if (orbs[i] == null)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static bool IsOrbSpace()
    {
        if (Inventory.instance.FindEquippedRune(Runes.Rune_of_Triangle))
        {
            for (int i = 0; i < orbs.Length; i++)
            {
                if (orbs[i] == null)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < orbLimit; i++)
            {
                if (orbs[i] == null)
                    return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //if(Enemies.Length > 0)
        //Gizmos.DrawRay(transform.position, Enemies[0].transform.position - transform.position);
    }
}
