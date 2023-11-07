using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLedgeEmission : MovingPlatform
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        speed = rb.velocity;
    }

}
