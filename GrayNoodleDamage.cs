using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GrayNoodleDamage : Projectile
{

    private Rigidbody2D voltocity;


    // Start is called before the first frame update
    void Start()
    {
        voltocity = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    new void OnTriggerEnter2D(Collider2D collision)
    {

        base.OnTriggerEnter2D(collision);

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
                if(pSExplosion != null)
                Instantiate(pSExplosion, transform.position, Quaternion.identity);
            }
        }

        if (gameObject.tag == "Gray Magic")
        {
            if (collision.tag == "Enemy Magic")
            {
                if (pSExplosion != null)
                    Instantiate(pSExplosion, transform.position, Quaternion.identity);
                if(explosion != null)
                    Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        if(gameObject.tag == "Enemy Magic")
        {
            if (collision.tag != "Gray Magic")
            {
                if (pS != null)
                {
                    pS.GetComponent<ParticleSystem>().Stop();
                    pS.parent = null;
                    pS.localScale = new Vector3(1, 1, 1);
                    Destroy(pS.gameObject, 3);
                }
                Destroy(gameObject);
                if(pSExplosion != null)
                Instantiate(pSExplosion, transform.position, Quaternion.identity);
            }
        }


    }
}
