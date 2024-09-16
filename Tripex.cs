using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripex : BaseEnemyMovement
{
    public GameObject stoneRock;
    public GameObject rockStone;
    public Transform rockpos;
    public Vector2 ProjSpeed;
    public float rngMove;
    public float jumpHeight;
    public bool startWalking;

    new void Update()
    {
        base.Update();
        rngMove = Random.Range(0, 2);

        if (!isRangeIdle)
        {
            startWalking = false;
            if (!isAttacking)
            {
                isAttacking = true;
                attackingTimer = maxAttackTime;
                InRange();
            }
        }
        else if (isRangeIdle)
        {
            IsRangeIdle();
        }
        if (attackingTimer <= 0)
        {
            isAttacking = false;
        }
    }

    public override void IsRangeIdle()
    {
        if(!startWalking && !isAttacking)
        {
            startWalking = true;
            animo.Play("WizTripexWalkStart");
        }

        if (!isAttacking)
        {
            if (isFloorInFront)
            {
                if (isFacingRight)
                {
                    //regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    //regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if (isGrounded)
            {
                //regigigas.velocity = new Vector2(regigigas.velocity.x / 2, regigigas.velocity.y);
                if (isFacingRight)
                {
                    isFacingRight = false;
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    isFacingRight = true;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }


            if (!isGrounded)
            {
                if (isFacingRight)
                {
                    regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
                }
                else
                {
                    regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
                }
            }
        }
    }

    public override void InLOS()
    {
        base.InLOS();
        //animo.Play("WizTripex");
        Debug.Log("TAST");
    }

    public override void InRange()
    {
        base.InRange();
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        animo.Play("WizTripex");
        Debug.Log("SLICE");
    }

    public void SpawnRock()
    {
        GameObject roker = Instantiate(stoneRock, rockpos.position, Quaternion.identity);
        if (gameObject.transform.localScale.x < 0)
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

    public void Joomp()
    {
        regigigas.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }
}
