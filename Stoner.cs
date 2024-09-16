using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoner : BaseEnemyMovement
{
    public GameObject stoneRock;
    public GameObject rockStone;
    public Transform rockpos;
    public Vector2 ProjSpeed;
    public float rngMove;
    public bool inRange2;

    new void Update()
    {
        base.Update();
        if (Physics2D.OverlapCircle(gameObject.transform.position, detectionRange * 1.25f, 1 << (LayerMask.NameToLayer("Player"))))
        {
            inRange2 = true;
        }
        else
        {
            inRange2 = false;
        }
        rngMove = Random.Range(0, 2);
        if(!isRangeIdle && !isLOSIdle)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackingTimer = maxAttackTime;
                if (rngMove == 1)
                    InLOS();
                else
                    InRange();
            }
        }else if(inRange2)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                attackingTimer = maxAttackTime;
                InRange();
            }
        }
        if(attackingTimer <= 0)
        {
            isAttacking = false;
        }
        if(!isAttacking && player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }else if(!isAttacking && player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

    public override void InLOS()
    {
        base.InLOS();
        animo.Play("WizThrower");
    }

    public override void InRange()
    {
        base.InRange();
        animo.Play("WizStonerSlamer");
    }

    public void SpawnRock()
    {
        GameObject roker = Instantiate(stoneRock, rockpos.position, Quaternion.identity);
        if(gameObject.transform.localScale.x < 0)
            roker.GetComponent<Rigidbody2D>().AddForce(-ProjSpeed, ForceMode2D.Impulse);
        else
            roker.GetComponent<Rigidbody2D>().AddForce(ProjSpeed, ForceMode2D.Impulse);
        roker.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void SpawnStone()
    {
        Vector3 floorPosition = Physics2D.Raycast(player.transform.position, Vector2.down * 100, 100f, (1 << LayerMask.NameToLayer("Ground"))).point;
        Instantiate(rockStone, floorPosition + Vector3.up, Quaternion.identity);
    }
}
