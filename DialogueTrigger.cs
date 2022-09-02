using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Interactable
{

    private int lineNumba = 0;
    public Dialogue dialogue;

    private Movement movements;
    private Attacks attacks;
    private Health health;
    private Transform playgerism;
    private Rigidbody2D player;

    new void Start()
    {
        base.Start();
        movements = movement;
        attacks = GameObject.FindGameObjectWithTag("Player").GetComponent<Attacks>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playgerism = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = playgerism.GetComponent<Rigidbody2D>();
    }

    new void Update()
    {
        base.Update();
        if(lineNumba != 0)
        {
            player.velocity = new Vector2(0, 0);
            movements.anime.SetBool("isMoving", false);
            movements.anime.SetBool("IsGrounded", true);
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void DisplayNexDialogue()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogue.frameLag);
    }

    public override void Interaction()
    {

        base.Interaction();
        Debug.Log("We good here");
        if (lineNumba == 0)
        {
            TriggerDialogue();
            Talking(false);
        }else
        {
            DisplayNexDialogue();
        }
        hasInteracted = false;
        lineNumba++;
        if(lineNumba > dialogue.sentences.Length)
        {
            lineNumba = 0;
            Talking(true);
        }
    }

    void Talking(bool yes)
    {
        attacks.enabled = yes;
        movements.enabled = yes;
        health.enabled = yes;
    }
}
