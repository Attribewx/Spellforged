using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{

    public Attacks attacks;
    public Health health;
    public Transform playgerism;
    public Rigidbody2D player;
    public EnemyManager fondingNemo;
    public bool isResting;
    public Movement movements;


    public override void Interaction()
    {
        base.Interaction();
        if(!isResting)
        {
            GamePersist.instance.Save();
            Rest();
            isResting = true;
            Debug.Log("RESTING");
            hasInteracted = false;
        }else if(isResting)
        {
            UnRest();
            isResting = false;
            Debug.Log("UnRESTING");
            hasInteracted = false;
        }
    }

    new void Start()
    {
        base.Start();
        movements = movement;
        attacks = GameObject.FindGameObjectWithTag("Player").GetComponent<Attacks>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playgerism = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = playgerism.GetComponent<Rigidbody2D>();
        fondingNemo = GameObject.FindGameObjectWithTag("Enemy Manager").GetComponent<EnemyManager>();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (isResting)
        {
            player.velocity = new Vector2(0, 0);
            movements.anime.SetBool("isMoving", false);
            movements.anime.SetBool("IsGrounded", true);
        }
    }

    void Rest()
    {
        attacks.enabled = false;
        movements.enabled = false;
        health.HealUp(health.maxHealth);
        fondingNemo.ClearBois();
    }

    void UnRest()
    {
        attacks.enabled = true;
        movements.enabled = true;
    }

}
