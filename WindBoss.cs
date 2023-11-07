using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoss : Boss
{

    [SerializeField, Header("Inspector Setup"), Space(5)] private GameObject windProj;
    [SerializeField, Header("Attack Setup"), Space(5)] private float xWindProjOffset;
    [SerializeField] private float yWindProjOffset;
    [SerializeField] private float WindProjSpeed;
    [SerializeField, Header("Attack Lag"), Space(5)] private float windProjLag;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(canAttack)
        {
            Vector3 offset = PlayerOffsetPos(xWindProjOffset, yWindProjOffset);
            LaunchProjectile(windProj, offset, Quaternion.identity, PlayerDir(offset) * WindProjSpeed);
            offset = PlayerOffsetPos(-xWindProjOffset, yWindProjOffset); ;
            LaunchProjectile(windProj, offset, Quaternion.identity, PlayerDir(offset) * WindProjSpeed);
            SetAttackTime(windProjLag);
        }
    }

    
}
