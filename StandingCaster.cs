using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingCaster : BaseEnemyMovement
{
    [SerializeField, Header("Projectile Settings")] GameObject windProjPrefab;
    [SerializeField] float projSpeed;

    public new void Update()
    {
        base.Update();

        if(attackingTimer <= 0)
        {
            isAttacking = false;
        }

        if(inRange && !isAttacking)
        {
            attackingTimer = maxAttackTime;
            isAttacking = true;
            animo.Play("WizCloudFattyAttack");
        }
    }

    public void Shoot()
    {
        GameObject curProj = Instantiate(windProjPrefab, transform.position, Quaternion.identity);
        Rigidbody2D curRig = curProj.GetComponent<Rigidbody2D>();

        int rng = RNG(0, 5);
        Vector3 dir;
        switch (rng)
        {
            default:
                dir = player.transform.position - transform.position;
                break;

            case 1:
                dir = (player.transform.position + Vector3.right) - transform.position;
                break;

            case 2:
                dir = (player.transform.position + Vector3.left) - transform.position;
                break;

            case 3:
                dir = (player.transform.position + Vector3.up) - transform.position;
                break;

            case 4:
                dir = (player.transform.position + Vector3.down) - transform.position;
                break;
        }

        if(curRig)
            curRig.velocity = dir.normalized * projSpeed;

        Debug.Log("Shooting");
    }
}
