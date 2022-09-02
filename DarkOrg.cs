using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrg : Projectile
{
    [Header("Homing Soulmass Setup"), Space(20) ] public CircleCollider2D circleOH;
    private Transform closestEnemi;
    public Rigidbody2D movieo;
    private bool foundTarg;
    private float shortDist;
    private Collider2D[] Enemies;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!foundTarg)
        { 
            Enemies = Physics2D.OverlapCircleAll(transform.position, 4, 1 << LayerMask.NameToLayer("EnemyHBox"));
            for (int i = 0; i < Enemies.Length; i++)
            {
                RaycastHit2D ronald = Physics2D.Raycast(transform.position, Enemies[i].transform.position - transform.position, 4, (1 << LayerMask.NameToLayer("EnemyHBox") | (1 << LayerMask.NameToLayer("Ground"))));
                if (ronald)
                {
                    if(ronald.collider.name == Enemies[i].transform.name)
                    {
                        Debug.Log(ronald.collider.name);
                        if (ronald.distance < shortDist)
                        {
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
                movieo.velocity = (closestEnemi.position - transform.position).normalized * 8;
            else
                movieo.velocity = Vector2.zero;
        }
        else
        {
            if (!(playerPos.position.x - transform.position.x < .2f && playerPos.position.x - transform.position.x > -.2f) || !(playerPos.position.y + 1.5f - transform.position.y < .2f && playerPos.position.y + 1.5f - transform.position.y > -.2f))
                movieo.velocity = ((playerPos.position + new Vector3(0 , 1.5f))- transform.position).normalized * 5;
            else
                movieo.velocity = Vector2.zero;
        }
    }

    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            collision.GetComponent<Health>().TakeDamage(damage, 0, 0, Element);
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(Enemies.Length > 0)
        Gizmos.DrawRay(transform.position, Enemies[0].transform.position - transform.position);
    }
}
