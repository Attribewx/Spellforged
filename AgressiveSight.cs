using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveSight : MonoBehaviour
{
    public float supeed = 100f;
    public float ranger = 8f;
    public float tooClose = .1f;
    public bool righto = false;
    public bool lefto = false;
    public Rigidbody2D captainRex;
    public Vector3 scalze;
    public Animator amagi;
    Vector3 invScalze;
    Transform playdo;

    void Start()
    {
        playdo = GameObject.FindGameObjectWithTag("Player").transform;
        captainRex = transform.GetComponent<Rigidbody2D>();
        scalze = transform.localScale;
        invScalze = new Vector3(-scalze.x, scalze.y, scalze.z);
        amagi = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float disto = playdo.position.x - transform.position.x;
        bool inRanger = false;
        if(disto < 0)
        {
            disto *= -1;
        }

        if(disto < ranger && disto > tooClose)
        {
            inRanger = true;
            amagi.SetBool("InRange", true);
        }
        else
        {
            inRanger = false;
            amagi.SetBool("InRange", false);
        }

        if (playdo.position.x < transform.position.x && inRanger && disto > tooClose )
        {
            lefto = true;
            righto = false;
            transform.localScale = scalze;
        }
        else if(playdo.position.x > transform.position.x && inRanger && disto > tooClose)
        {
            righto = true;
            lefto = false;
            transform.localScale = invScalze;
        }
        else
        {
            lefto = false;
            righto = false;
        }
    }

    void FixedUpdate()
    {
        if(lefto)
        {
            captainRex.velocity = new Vector2(1 * -supeed * Time.deltaTime, captainRex.velocity.y);
        }
        else if(righto)
        {
            captainRex.velocity = new Vector2(1 * supeed * Time.deltaTime, captainRex.velocity.y);
        }
        else
        {
            captainRex.velocity = new Vector2(0, captainRex.velocity.y);
        }
    }
}
