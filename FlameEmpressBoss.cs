using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameEmpressBoss : Boss
{
    [SerializeField] private GameObject wallPJT;
    [SerializeField] private GameObject ballPJT;
    [SerializeField, Header("Movement Options"), Space(5)] private float speed;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverWidth;
    [SerializeField, Header("Attack Options"), Space(5)] private float wallDist;
    [SerializeField] private int wallAmt;
    [SerializeField] private int ballAmt;
    [SerializeField] private int ballYOffset;
    [SerializeField] private int ballXOffset;
    [SerializeField] private Vector2 ballSpeed;
    [SerializeField] private float ballTime;
    [SerializeField, Header("RNG Options"), Space(5)] private int wallRNG = 25;
    [SerializeField] private int ballRNG = 50;
    [SerializeField, Header("Move Lag Options"), Space(5)] private int flameWallLag = 1;
    [SerializeField,] private int flameBallLag = 1;
    private bool fastballFlip;
    private float targetPosition;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        float rngMoves = Random.Range(0, 100);

        if (canAttack)
        {
            if (rngMoves < wallRNG)
            {
                anim.Play("FlameEmpressAttack");
                SetAttackTime(flameWallLag);
            }
            else if(rngMoves < ballRNG)
            {
                anim.Play("FlameEmpressAttack2");
                SetAttackTime(flameBallLag);
            }
            NewTargetPosition();
        }

        if(Physics2D.Raycast(transform.position, Vector2.right, hoverWidth, 1 << LayerMask.NameToLayer("Ground")))
        {
            targetPosition = -hoverWidth;
        }
        if (Physics2D.Raycast(transform.position, Vector2.left, hoverWidth, 1 << LayerMask.NameToLayer("Ground")))
        {
            targetPosition = hoverWidth;
        }

    }

    public void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 hoverDir = new Vector3(GetPlayerPos().transform.position.x - transform.position.x + targetPosition, GetPlayerPos().transform.position.y + hoverHeight - transform.position.y, 0);
            Move(hoverDir);
        }
    }

    public void CastFireballs()
    {
        int fireballSetRNG = Random.Range(1, 4);
        float yOffset = 0;
        float xOffset = ballXOffset;
        Vector2 speed = ballSpeed;
        if(fireballSetRNG == 1)
        {
            for (int i = 0; i < ballAmt; i++)
            {
                GameObject proj = Instantiate(ballPJT, playerLoc.position + new Vector3(xOffset, yOffset, 0), Quaternion.identity);
                Rigidbody2D rigi = proj.GetComponent<Rigidbody2D>();
                if (xOffset < 0)
                {
                    proj.transform.localScale = new Vector3(-proj.transform.localScale.x, proj.transform.localScale.y, proj.transform.localScale.z);
                }
                if (rigi)
                {
                    StartCoroutine(DelayedBlast(ballTime, rigi, speed));
                }
                yOffset += ballYOffset;
                xOffset = -xOffset;
                speed = -speed;
            }
        }
        else if(fireballSetRNG == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject proj = Instantiate(ballPJT, playerLoc.position + new Vector3(xOffset, yOffset, 0), Quaternion.identity);
                Rigidbody2D rigi = proj.GetComponent<Rigidbody2D>();
                GameObject proj2 = Instantiate(ballPJT, playerLoc.position + new Vector3(-xOffset, yOffset, 0), Quaternion.identity);
                Rigidbody2D rigi2 = proj2.GetComponent<Rigidbody2D>();
                proj2.transform.localScale = new Vector3(-proj2.transform.localScale.x, proj2.transform.localScale.y, proj2.transform.localScale.z);
                if (rigi)
                {
                    StartCoroutine(DelayedBlast(ballTime, rigi, speed));
                }
                if (rigi2)
                {
                    StartCoroutine(DelayedBlast(ballTime, rigi2, -speed));
                }
                yOffset += ballYOffset * 4;
            }
        }
        else
        {
            if (fastballFlip)
            {
                GameObject proj = Instantiate(ballPJT, playerLoc.position + new Vector3(xOffset, yOffset, 0), Quaternion.identity);
                Rigidbody2D rigi = proj.GetComponent<Rigidbody2D>();
                if (rigi)
                {
                    StartCoroutine(DelayedBlast(ballTime, rigi, speed * 2f));
                }
            }
            else
            {
                GameObject proj = Instantiate(ballPJT, playerLoc.position + new Vector3(-xOffset, yOffset, 0), Quaternion.identity);
                Rigidbody2D rigi = proj.GetComponent<Rigidbody2D>();
                proj.transform.localScale = new Vector3(-proj.transform.localScale.x, proj.transform.localScale.y, proj.transform.localScale.z);
                if (rigi)
                {
                    StartCoroutine(DelayedBlast(ballTime, rigi, -speed * 2f));
                }
            }
        }
        fastballFlip = !fastballFlip;
    }

    public void CastWalls()
    {
        for (int i = -wallAmt/2; i <= wallAmt/2; i++)
        {
            Vector3 launchDir = new Vector3((i * wallAmt) * wallDist, 250, 0);
            GameObject proj = Instantiate(wallPJT, transform.position, Quaternion.identity);
            Rigidbody2D projRB = proj.GetComponent<Rigidbody2D>();
            projRB.AddForce(launchDir);
        }
    }

    public void Move(Vector3 hoverDir)
    {
        rig.AddForce(hoverDir.normalized * speed);
    }

    public IEnumerator DelayedBlast(float time, Rigidbody2D rigi, Vector2 speed)
    {
        yield return new WaitForSeconds(time);
        if(rigi)
        rigi.velocity = speed;
    }

    public void NewTargetPosition()
    {
        float rngMovement = Random.Range(1, 4);
        switch (rngMovement)
        {
            default:
                targetPosition = 0;
                break;
            case 2:
                targetPosition = hoverWidth;
                break;
            case 3:
                targetPosition = -hoverWidth;
                break;
        }
    }
}
