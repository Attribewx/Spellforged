using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{

    public Runes item;


    new void Start()
    {
        base.Start();
        foreach (int anItem in Inventory.instance.items)
        {
            if(anItem == ((int)item))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public override void Interaction()
    {
        base.Interaction();
        Inventory.instance.Add(((int)item));
        gameObject.SetActive(false);
    }
}
