using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreaperBoss : Boss
{

    [SerializeField] private float movSpeed;
    [SerializeField] private GameObject groundProj;
    [SerializeField] private GameObject floatingProj;
    [SerializeField] private Transform groundSpawn;
    [SerializeField] private Transform floatingSpawn;
    [SerializeField] private float idleWalkTime = 1.25f;
    [SerializeField, Header("Launch Options"), Space(5)] private Vector2 groundSpeed;
    [SerializeField] private Vector2 floatingSpeed;
    [SerializeField, Header("RNG Options"), Space(5)] private int scrapeRNG = 25;
    [SerializeField] private int beamRNG = 50;
    [SerializeField] private int teleportRNG = 75;
    [SerializeField, Header("Move Lag Options"), Space(5)] private int scrapeLag = 1;
    [SerializeField] private int beamLag = 1;
    [SerializeField] private int teleportLag = 4;
    private float movementTimer;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        float rngMoves = Random.Range(0, 100);


        if(canAttack)
        {
            SetLeftRight(-2);
            if (movementTimer < Time.time)
                anim.SetBool("Movement", false);
            if (rngMoves < scrapeRNG && movementTimer < Time.time)
            {
                rig.velocity = Vector3.zero;
                anim.Play("TreaperGroundScrape");
                SetAttackTime(scrapeLag);
            }
            else if (rngMoves < beamRNG && movementTimer < Time.time)
            {
                rig.velocity = Vector3.zero;
                anim.Play("TreaperSlice");
                SetAttackTime(beamLag);
            }
            else if (rngMoves < teleportRNG && movementTimer < Time.time)
            {
                rig.velocity = Vector3.zero;
                anim.Play("TreaperTP");
                SetAttackTime(teleportLag);
            }
            else
            {
                if(movementTimer < Time.time)
                {
                    movementTimer = Time.time + idleWalkTime;
                    anim.Play("TreaperMove");
                    anim.SetBool("Movement", true);
                }
                    Move();
            }
        }
    }

    public void GroundAttack()
    {
        if (transform.localScale.x > 0)
            LaunchProjectile(groundProj, groundSpawn.position, Quaternion.identity, -groundSpeed);
        else
            LaunchProjectile(groundProj, groundSpawn.position, Quaternion.Euler(new Vector3(0, 180, 0)), groundSpeed);
    }

    public void FloatAttack()
    {
        if (transform.localScale.x > 0)
            LaunchProjectile(floatingProj, floatingSpawn.position, Quaternion.identity, -floatingSpeed);
        else
            LaunchProjectile(floatingProj, floatingSpawn.position, Quaternion.Euler(new Vector3(0, 180, 0)), floatingSpeed);
    }

    public void Move()
    {
        if(playerLoc.position.x < transform.position.x)
            rig.AddForce(Vector2.left * movSpeed * Time.deltaTime);
        else
            rig.AddForce(Vector2.right * movSpeed * Time.deltaTime);

    }

    public void TP()
    {
        Vector3 playerExactPos = GetPlayerPos().position;
        RaycastHit2D raycastHit = Physics2D.Raycast(playerExactPos, Vector2.down, 100, 1 << LayerMask.NameToLayer("Ground"));
        gameObject.transform.position = raycastHit.point + new Vector2(0, 1);
    }
}
