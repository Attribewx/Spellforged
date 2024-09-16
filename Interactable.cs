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
    public GameObject keyIcon;
    private SpriteRenderer Sr;

    
    public virtual void Interaction()
    {
        Debug.Log("Interacting with " + transform.name);
        //Interaction Code goes in here
    }


    public void Start()
    {
        if(keyIcon)
        {
            Sr = keyIcon.GetComponent<SpriteRenderer>();
        }
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }


    public void Update()
    {
        if (keyIcon)
        {
            //Sr.color = new Color(255,255,255,Mathf.Lerp(Sr.color.a, 0, .1f));
            //keyIcon.SetActive(false);

        }
        else
            Debug.LogWarning("Key Icon is not Assigned for:       " + gameObject.name);


        //Gets the distance between player and interactable object
        dist = Vector3.Distance(transform.position, playerPos.position);
        if (dist <= interactionRange && !hasInteracted)
        {
            if (keyIcon)
            {
                keyIcon.SetActive(true);
                Sr.color = new Color(255, 255, 255, Mathf.Lerp(Sr.color.a, 255, .0002f));
            }
            else
                Debug.LogWarning("Key Icon is not Assigned for:       " + gameObject.name);
            if (Input.GetButtonDown("Interact") && movement.IsGrounds())
            {
                hasInteracted = true;
                Interaction();
            }
        }
        else
        {
            if (keyIcon)
            {
                Sr.color = new Color(255, 255, 255, Mathf.Lerp(Sr.color.a, 0, .1f));
            }
        }
    }
}
