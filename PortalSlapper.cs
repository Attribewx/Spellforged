using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSlapper : MonoBehaviour
{
    public int damagio = 1;
    public bool active = false;
    public bool hit = false;
    public bool off = false;
    public LayerMask lego;
    public BoxCollider2D boko;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active && !hit)
        {
            boko.enabled = true;
        }
        else
        {
            boko.enabled = false;
        }

        if(off)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D mileyRayCyrus)
    {
        if (mileyRayCyrus)
        {
            if (mileyRayCyrus.tag == "Player")
            {
                if (mileyRayCyrus.transform.position.x < transform.position.x)
                    mileyRayCyrus.GetComponent<Health>().TakeDamage(damagio, -100, 400, ElementType.Physical);
                else
                    mileyRayCyrus.GetComponent<Health>().TakeDamage(damagio, 100, 400, ElementType.Physical);
                hit = true;
            }
        }
    }


}
