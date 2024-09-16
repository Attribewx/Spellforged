using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int damagio = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                collision.GetComponent<Health>().TakeDamage(damagio, -100, 400, ElementType.Physical);
            else
                collision.GetComponent<Health>().TakeDamage(damagio, 100, 400, ElementType.Physical);
        }
    }
}
