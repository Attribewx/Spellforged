using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatables
{
    [SerializeField] private float speed;
    [SerializeField] private bool isOpenForever;
    [SerializeField] private Triggers trigger;

    public override void Start()
    {
        base.Start();
        if(isOpenForever)
        {
            if(TriggerManager.activeMan.FindTrigger((int)trigger))
                Open();
        }
    }

    public void Open()
    {
        if(!multiActivational)
        {
            if(!TriggerManager.activeMan.FindTrigger((int)trigger))
                TriggerManager.activeMan.AddTrigger((int)trigger);
        }
        boxy.enabled = false;
    }
}
