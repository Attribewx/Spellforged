using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
     public Transform playerLoc;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool canMove;
    [HideInInspector] public Rigidbody2D rig;
    [HideInInspector] public Animator anim;
    public string songName;
    public float attackTime = 1;
    public float moveTime = 1;
    [Header("Activation Settings")] public float activationDist = 8;
    public Vector3 activationOffset;
    private bool idleCheck;
    private BossHealthBar bHB;
    private Health health;
    private bool oneTime;

    public virtual void Start()
    {
        health = GetComponent<Health>();
        playerLoc = FindObjectOfType<Movement>().transform;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bHB = FindObjectOfType<BossHealthBar>();
        if (!health)
            health = GetComponentInChildren<Health>();
        bHB.FindBoss();
    }

    public virtual void Update()
    {
        if (playerLoc == null)
            playerLoc = FindObjectOfType<Movement>().transform;

        if (!idleCheck && DistanceToPlayer() < activationDist)
        {
            idleCheck = true;
            PlayTheme(songName);
            bHB = FindObjectOfType<BossHealthBar>();
            bHB.FindBoss();
            if (bHB)
            {
                bHB.InitializeBar(gameObject.name, health.maxHealth);
            }
        }


        if(idleCheck && health.helf > 0)
            CheckTimes();

        if(health.helf <= 0)
        {
            PlayTheme("");
        }
    }


    public void CheckTimes()
    {
        if (Time.time > attackTime)
            canAttack = true;
        else
            canAttack = false;
        if (Time.time > moveTime)
            canMove = true;
        else
            canMove = false;
    }

    public void SetMoveTime(float f)
    {
        moveTime = Time.time + f;
    }

    public void SetAttackTime(float f)
    {
        attackTime = Time.time + f;
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(playerLoc.position, transform.position + activationOffset);
    }

    public void SetMove(bool b)
    {
        canMove = b;
    }

    public void SetAttack(bool b)
    {
        canAttack = b;
    }

    public GameObject LaunchProjectile(GameObject proj, Vector3 spawnPos, Quaternion rot, Vector2 vel)
    {
        GameObject attack = Instantiate(proj, spawnPos, rot);
        Rigidbody2D projectile = attack.GetComponent<Rigidbody2D>();
        if(projectile)
            projectile.velocity = vel;
        return attack;
    }

    public void MeleeAttack(float damage, Vector2 attackSize, Vector2 dir, Vector2 knockback, float length, ElementType elem)
    {
        Vector2 boxSize = new Vector2(attackSize.x, attackSize.y);
        RaycastHit2D player =  Physics2D.BoxCast(transform.position, boxSize, 0, dir, length, 1 << (LayerMask.NameToLayer("Player")) );
        Health playerHelth;
        if(player)
        {
            playerHelth = player.collider.GetComponent<Health>();
            if(playerHelth)
                playerHelth.TakeDamage(damage, knockback.x, knockback.y, elem);
        }
    }

    public void SetLeftRight(float dir)
    {
        if(playerLoc.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-dir, transform.localScale.y, 2);
        }
        else
        {
            transform.localScale = new Vector3(dir, transform.localScale.y, 2);
        }
    }
    public void SetLeftRightFlying(float dir)
    {
        if (playerLoc.position.x < transform.position.x - .5f)
        {
            transform.localScale = new Vector3(-dir, transform.localScale.y, 2);
        }
        else if (playerLoc.position.x > transform.position.x + .5f)
        {
            transform.localScale = new Vector3(dir, transform.localScale.y, 2);
        }
    }


    public Transform GetPlayerPos()
    {
        return playerLoc;
    }

    public Vector3 PlayerOffsetPos(float x, float y)
    {
        return new Vector3(playerLoc.transform.position.x + x, playerLoc.transform.position.y + y, playerLoc.transform.position.z);
    }

    public Vector3 PlayerDir(Vector3 startPos)
    {
        return (playerLoc.position - startPos).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + activationOffset, activationDist);
    }

    public void PlayTheme(string songName)
    {
        if (songName.Length > 0)
        {
            AudioManager.arduino.Play(songName);
            AudioManager.arduino.BossMix(true);
        }
        else
        {
            AudioManager.arduino.BossMix(false);
        }
    }
}
