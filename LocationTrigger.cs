using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationTrigger : MonoBehaviour
{

    [HideInInspector]public BoxCollider2D area;
    public virtual void TriggerEvent(Collider2D other)
    {
        Debug.Log("Trigger Started location event at: " + gameObject.name);
    }

    public virtual void TriggeringEvent(Collider2D other)
    {
        Debug.Log("Triggering location event at: " + gameObject.name);
    }

    public virtual void TriggeredEvent(Collider2D other)
    {
        Debug.Log("Triggered location event at: " + gameObject.name);
    }

    public void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEvent(other);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        TriggeringEvent(other);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        TriggeredEvent(other);
    }
}
