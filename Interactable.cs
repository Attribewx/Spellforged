using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactionRange = 3f;
    Transform playerPos;
    public Movement movement;
    public bool hasInteracted;
    float dist;

    
    public virtual void Interaction()
    {
        Debug.Log("Interacting with " + transform.name);
        //Interaction Code goes in here
    }


    public void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }


    public void Update()
    {
        //Gets the distance between player and interactable object
        dist = Vector3.Distance(transform.position, playerPos.position);
        if (dist <= interactionRange && !hasInteracted)
        {
            if(Input.GetButtonDown("Interact") && movement.IsGrounds())
            {
                hasInteracted = true;
                Interaction();
            }
        }
    }
}
