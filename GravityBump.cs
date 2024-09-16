using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBump : MonoBehaviour
{

    public float range;
    public float lift;
    public Transform pointed;
    public float damage = 3;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit2D chekRight = Physics2D.Raycast(pointed.position, Vector2.right, range, 1 << LayerMask.NameToLayer("Enemy"));
        RaycastHit2D chekLeft = Physics2D.Raycast(pointed.position, Vector2.left, range, 1 << LayerMask.NameToLayer("Enemy"));
        if(chekRight)
        {
            Launch(chekRight);
        }
        if (chekLeft)
        {
            Launch(chekLeft);
        }
    }


    private void OnDrawGizmos()
    {
        Debug.DrawRay(gameObject.transform.position, Vector2.right * range, Color.red);
    }

    void Launch(RaycastHit2D chekRight)
    {
        ITakeDamage interaction1 = chekRight.collider.GetComponent<ITakeDamage>();
        interaction1?.TakeDamage(damage, 0, 0, ElementType.Gravity);

        Debug.Log(chekRight.collider.name);

        if (chekRight.collider.tag == "Enemy")
        {
            if (chekRight.collider.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D movieo = chekRight.collider.GetComponent<Rigidbody2D>();
                movieo.velocity = new Vector3(movieo.velocity.x, lift, 0);
            }
        }
    }
}
