using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRoam : MonoBehaviour
{

    public Vector3 leftPoint1;
    public Vector3 rioghtPoint2;
    public Rigidbody2D gigan;
    
    public bool leftForDead = true;
    public float speed;
    private Vector3 activePoint;
    private Vector3 scalsa;
    private Vector3 invScalsa;
    // Start is called before the first frame update
    void Start()
    {
        activePoint = leftPoint1;
        gigan = GetComponent<Rigidbody2D>();
        scalsa = transform.localScale;
        invScalsa = new Vector3(-scalsa.x, scalsa.y, scalsa.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(activePoint == leftPoint1)
        {
            if (activePoint.x < transform.position.x)
            {
                transform.localScale = scalsa;
            }
            else
                activePoint = rioghtPoint2;
        }else
        {
            if (activePoint.x > transform.position.x)
            {
                transform.localScale = invScalsa;
            }
            else
                activePoint = leftPoint1;
        }



    }

    void FixedUpdate()
    {
        if(transform.localScale == scalsa)
        {
            gigan.velocity = new Vector2(-1 * speed * Time.deltaTime, gigan.velocity.y);
        }else
        {
            gigan.velocity = new Vector2(1 * speed * Time.deltaTime, gigan.velocity.y);
        }
    }
}
