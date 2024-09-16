using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwayne : BaseEnemyMovement
{

    public float idleDist = 3f;
    public float jForce;
    public float stompCD;
    public float throwCD;
    private float cooldown;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        if (isGrounded == true && nearGround == true && regigigas.velocity.y < .05f)
        {
            Jump();
            Debug.Log("inHer");
        }
        if(isAttacking == true && attackingTimer < Time.time)
        {
            isAttacking = false;
        }
    }

    void FixedUpdate()
    {

        if (!isRangeIdle && !isLOSIdle)
        {
            if (inRange && inLOS)
            {
                InLOS();
            }
        }else
        {
            IsLOSIdle();
        }

    }


    public override void IsLOSIdle()
    {
        base.IsLOSIdle();
        if(!isAttacking)
        {
            if (isFloorInFront)
            {
                if (isFacingRight)
                {
                    regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if(isGrounded)
            {
                regigigas.velocity = new Vector2(regigigas.velocity.x / 2,regigigas.velocity.y);
                if(isFacingRight)
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

            //if (regigigas.velocity.x < 0 && reallyNGround == false && (lastIdlePos.x - idleDist) < transform.position.x)
            //{
            //    transform.rotation = Quaternion.Euler(0, 180, 0);
            //   regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
            //}
            // else  if (lastIdlePos.x + idleDist > transform.position.x && reallyNGround == false)
            //{
            //    transform.rotation = Quaternion.Euler(0, 0, 0);
            //    regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
            // }else
            // {
            //    if(transform.rotation == Quaternion.Euler(0,0,0))
            //     {
            //         regigigas.velocity = new Vector2(-1, regigigas.velocity.y);
            //         regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
            //        transform.rotation = Quaternion.Euler(0, 180, 0);
            //     }else
            //    {
            //         regigigas.velocity = new Vector2(1, regigigas.velocity.y);
            //        regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
            //        transform.rotation = Quaternion.Euler(0, 0, 0);
            //    }
            // }
        }
    }

    public override void InRange()
    {
        base.InRange();
    }

    public override void InLOS()
    {
        base.InLOS();
        if (isAttacking == false && isGrounded)
        {
            isAttacking = true;
            animo.SetTrigger("Attack1");
            attackingTimer = Time.time + maxAttackTime;
            if(gameObject.transform.position.x < player.transform.position.x)
            {
                regigigas.velocity = Vector2.zero;
                regigigas.AddForce(Vector2.right * Time.deltaTime * speed * .75f, ForceMode2D.Impulse);
                isFacingRight = true;
            }
            else
            {
                regigigas.velocity = Vector2.zero;
                regigigas.AddForce(Vector2.left * Time.deltaTime * speed * .75f, ForceMode2D.Impulse);
                isFacingRight = false;
            }
            if(regigigas.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0,180,0);
            }
            else if (regigigas.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            lastIdlePos = transform.position;
        }

            if (isGrounded == true && nearGround == true && regigigas.velocity.y < .005f && regigigas.velocity.y > -.005f)
            {
                Jump();
            }

        if (!isGrounded && !isAttacking)
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

    public void Jump()
    {
        if(!isAttacking)
        {
            animo.SetTrigger("Jump");
            if (!steepGround)
            {
                regigigas.velocity = new Vector2(regigigas.velocity.x, jForce * .8f);
                if(isFacingRight)
                {
                    regigigas.AddForce(Vector2.right * Time.deltaTime * speed);
                }
                else
                {
                    regigigas.AddForce(Vector2.left * Time.deltaTime * speed);
                }
            }
            else
            {
                regigigas.velocity = new Vector2(regigigas.velocity.x, jForce);
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

}
