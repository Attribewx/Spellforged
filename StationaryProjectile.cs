using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryProjectile : Projectile
{
    public int destructionTime;

    public void Start()
    {
        Destroy(gameObject, destructionTime);
    }
}
