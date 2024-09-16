using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiyaah : MonoBehaviour
{

    public int damage;
    public GameObject explosion;
    public Transform pS;
    public GameObject pST;
    public float home;
    private RaycastHit2D hittyTitty;
    public LayerMask hollowKinght;
    [SerializeField] private LayerMask keyMask;

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

        Collider2D[] burntSiennaBois = Physics2D.OverlapCircleAll(transform.position, home, keyMask | hollowKinght);
        for (int i = 0; i < burntSiennaBois.Length; i++)
        {
            ITakeDamage interaction1 = burntSiennaBois[i].GetComponent<ITakeDamage>();
            interaction1?.TakeDamage(damage, 0, 0, ElementType.Fire);
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(pST, transform.position, Quaternion.identity);
        Destroy(gameObject);

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
                Instantiate(pST, transform.position, Quaternion.identity);
            }
        }
    }
}
