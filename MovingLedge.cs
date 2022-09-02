using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLedge : MovingPlatform
{

    [Header("Platform Movement"), Space(10)]
    public Vector3 lefter;
    public Vector3 righer;
    private bool goingBack;
    public bool goingUp;
    public Vector3 upter;
    public Vector3 downter;
    private bool goinsUppter;

    void Update()
    {
        if (!goingUp)
        {
            if (transform.position.x > righer.x && !goingBack)
            {
                goingBack = true;
                speed.x *= -1;
            } else if (transform.position.x < lefter.x && goingBack)
            {
                goingBack = false;
                speed.x *= -1;
            }
        } else if (goingUp)
        {
            if (transform.position.y > upter.y && !goinsUppter)
            {
                goinsUppter = true;
                speed.y *= -1;
            }else if (transform.position.y < downter.y && goinsUppter)
            {
                goinsUppter = false;
                speed.y *= -1;
            }
        }
    }
}
