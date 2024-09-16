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
        if (attacko.magicNumber == 6 && weaponNum != 6)
        {
            attacko.SwordEffects(false);
        }
        attacko.magicNumber = weaponNum;
        if (weaponNum == 6) //Lightblade
        {
            attacko.SwordEffects(true);
        }
        gameObject.SetActive(false);
    }
}
