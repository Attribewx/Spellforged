using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoss : Boss
{

    [SerializeField, Header("Inspector Setup"), Space(5)] private GameObject windProj;
    [SerializeField] private GameObject hadoukenProj;
    [SerializeField] private float speed;
    [SerializeField] private float hoverHeight;
    [SerializeField, Header("Attack Setup"), Space(5)] private float xWindProjOffset;
    [SerializeField] private float yWindProjOffset;
    [SerializeField] private float WindProjSpeed;
    [SerializeField] private Transform hadoukenSpawn;
    [SerializeField] private float hadoukenSpeed;
    [SerializeField] private float hadoukenBossOffset;
    [SerializeField, Header("Attack Lag"), Space(5)] private float windProjLag;
    [SerializeField] private float hadoukenLag;
    [SerializeField, Header("Attack RNG"), Space(5)] private int DiagonalRNG;
    [SerializeField] private int hadoukenRNG;
    private bool canMove2;
    private bool rightSide;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        SetLeftRightFlying(2);

        if(canAttack)
        {
            int RNG = Random.Range(0, 100);
            if(RNG <= DiagonalRNG)
            {
                DiagonalWinds();
            }
            else if(RNG <= hadoukenRNG)
            {
                canMove = false;
                canMove2 = true;
                anim.Play("WizWindBossHadouken");
                if (transform.position.x > GetPlayerPos().position.x)
                    rightSide = true;
                else
                    rightSide = false;
            }
        }
    }

    public void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 hoverDir = new Vector3(GetPlayerPos().transform.position.x - transform.position.x, GetPlayerPos().transform.position.y + hoverHeight - transform.position.y, 0);
            Move(hoverDir);
        }
        if(canMove2)
        {
            if (rightSide)
            {
                Vector3 hoverDir = new Vector3(GetPlayerPos().transform.position.x - transform.position.x + hadoukenBossOffset, GetPlayerPos().transform.position.y - transform.position.y, 0);
                MoveFaster(hoverDir);
            }
            else
            {
                Vector3 hoverDir = new Vector3(GetPlayerPos().transform.position.x - transform.position.x - hadoukenBossOffset, GetPlayerPos().transform.position.y - transform.position.y, 0);
                MoveFaster(hoverDir);
            }
        }
    }

    public void Move(Vector3 hoverDir)
    {
        rig.AddForce(hoverDir.normalized * speed);
    }

    public void MoveFaster(Vector3 hoverDir)
    {
        rig.AddForce(hoverDir.normalized * 3 * speed);
    }

    public void DiagonalWinds()
    {
        Vector3 offset = PlayerOffsetPos(xWindProjOffset, yWindProjOffset);
        LaunchProjectile(windProj, offset, Quaternion.identity, PlayerDir(offset) * WindProjSpeed);
        offset = PlayerOffsetPos(-xWindProjOffset, yWindProjOffset); ;
        LaunchProjectile(windProj, offset, Quaternion.identity, PlayerDir(offset) * WindProjSpeed);
        SetAttackTime(windProjLag);
    }

    public void Hadouken()
    {
        if(transform.localScale.x < 0)
            LaunchProjectile(hadoukenProj, hadoukenSpawn.position, Quaternion.identity, -hadoukenSpeed * Vector2.right);
        else
            LaunchProjectile(hadoukenProj, hadoukenSpawn.position, Quaternion.identity, hadoukenSpeed * Vector2.right);
        SetAttackTime(hadoukenLag);
        canMove = true;
        canMove2 = false;
    }
}
