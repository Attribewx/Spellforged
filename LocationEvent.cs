using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationEvent : MonoBehaviour
{

    public bool interactionState;

    public virtual void playEvent()
    {
        Debug.Log("Something is Happening");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactionState = true;
            playEvent();
        }
    }
}
