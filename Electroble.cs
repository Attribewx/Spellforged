using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Electroble : MonoBehaviour
{

    public int damage;
    public GameObject explosion;
    public Transform pS;
    public GameObject pST;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        ITakeDamage interaction1 = collision.GetComponent<ITakeDamage>();
        interaction1?.TakeDamage(damage, 0, 0, ElementType.Water);

        if (collision.tag == "Enemy" || collision.tag == "Boss")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Instantiate(pST, transform.position, Quaternion.identity);
        }

        if (collision.tag == "Floor")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Instantiate(pST, transform.position, Quaternion.identity);
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
                Instantiate(pST, transform.position, Quaternion.identity);
            }
        }

        if (gameObject.tag == "Gray Magic")
        {
            if (collision.tag == "Enemy Magic")
            {
                Instantiate(pST, transform.position, Quaternion.identity);
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
        }
    }
}
