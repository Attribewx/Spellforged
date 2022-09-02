using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{

    public Attacks attacko;
    public int weaponNum;

    new void Start()
    {
        base.Start();
        attacko = GameObject.FindGameObjectWithTag("Player").GetComponent<Attacks>();
        if (attacko.allTehUnloks[weaponNum])
        {
            gameObject.SetActive(false);
        }

    }

    public override void Interaction()
    {
        base.Interaction();
        attacko.AddANood(weaponNum);
        gameObject.SetActive(false);
    }
}
