using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [Header("Setup")]
    public float damage;
    public ElementType Element;
    public Vector2 speed;
    public GameObject explosion;
    public Transform pS;
    public GameObject pSExplosion;
    public GameObject crack;
    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        ITakeDamage interaction1 = collision.GetComponent<ITakeDamage>();
        interaction1?.TakeDamage(damage, 0, 0, Element);

        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {

            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);


            if (pSExplosion != null)
                Instantiate(pSExplosion, transform.position, Quaternion.identity);


            FindObjectOfType<AudioManager>().Play2("PONCH");
        }

        if (collision.tag == "Floor" || collision.tag == "EnemyShield")
        {
            if (crack != null)
                Instantiate(crack, transform.position, Quaternion.identity);
            if (pSExplosion != null)
                Instantiate(pSExplosion, transform.position, Quaternion.identity);
            if (gameObject.tag == "Gray Magic")
            {
                FindObjectOfType<AudioManager>().Play2("Crack");
            }
        }

        if (collision.tag == "Player")
        {
            if (explosion != null)
                Instantiate(explosion, transform.position, Quaternion.identity);
            if (collision.transform.position.x < transform.position.x)
            {
                ITakeDamage interaction = collision.GetComponent<ITakeDamage>();
                interaction?.TakeDamage(damage, 0, 0, Element);
            }
            else
            {
                ITakeDamage interaction = collision.GetComponent<ITakeDamage>();
                interaction?.TakeDamage(damage, 0, 0, Element);
            }
        }
    }

    public void Deflect()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Attacks"))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerAttacks");
        }else
        {
            gameObject.layer = LayerMask.NameToLayer("Attacks");
        }
    }
}
