using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public int[] items = new int[0];
    public int moneys = 0;
    public int[] equipment = new int[0];
    [SerializeField]private int MaxRunes = 3;
    private GameObject player;

    [Header("Rune Effect Attributes"), Space(10), SerializeField] int RuneOfHealthAMT = 100;
    [SerializeField] private int RuneOfManaAMT = 50;
    [SerializeField] private int RuneOfLifeAMT = 150;
    [SerializeField] private int RuneOfSacrificeHP = 200;
    [SerializeField] private int RuneOfSacrificeMana = -50;
    [SerializeField] private int RuneOfSpectreDashMana = 30;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        equipment = new int[MaxRunes];
        for (int i = 0; i < equipment.Length; i++)
        {
            equipment[i] = -1;
        }

        player = FindObjectOfType<Movement>().gameObject;
    }


    public void Add(int item)
    {
        int[] temps = new int[items.Length + 1];
        for (int i = 0; i < items.Length; i++)
        {
            temps[i] = items[i];
        }
        temps[items.Length] = item;
        items = temps;
    }

    public void AddMoneys(int amt)
    {
        moneys += amt;
    }

    public bool FindEquippedRune(Runes rune)
    {
        foreach (var item in equipment)
        {
            if (item == ((int)rune))
                return true;
        }
        return false;
    }

    public void EquipRune(int rune)
    {
        for (int i = 0; i < equipment.Length; i++)
        {
            if(equipment[i] == rune)
            {
                equipment[i] = -1;
                OnUnequipEffects(rune);
                return;
            }
        }

        for (int i = 0; i < equipment.Length; i++)
        {
            if(equipment[i] < 0)
            {
                equipment[i] = rune;
                OnEquipEffects(rune);
                return;
            }
        }
    }

    public void OnEquipEffects(int rune)
    {
        Health Hp = player.GetComponent<Health>();
        Attacks attack = player.GetComponent<Attacks>();
        switch (rune)
        {
            case ((int)Runes.Rune_of_Health):
                Hp.IncreaseMaxHealth(RuneOfHealthAMT);
                Hp.HealUp(Hp.maxHealth);
                break;
            case (int)Runes.Rune_of_Mana:
                attack.IncreaseMaxMana(RuneOfManaAMT);
                attack.ManaUp((int)attack.maxMana);
                break;
            case (int)Runes.Rune_of_Life:
                Hp.IncreaseMaxHealth(RuneOfLifeAMT);
                Hp.HealUp(Hp.maxHealth);
                break;
            case (int)Runes.Rune_of_Sacrifice:
                Hp.IncreaseMaxHealth(RuneOfSacrificeHP);
                Hp.HealUp(RuneOfSacrificeHP);
                attack.IncreaseMaxMana(RuneOfSacrificeMana);
                attack.ManaUp(RuneOfSacrificeMana);
                break;
        }
    }

    public void OnUnequipEffects(int rune)
    {
        Health Hp = player.GetComponent<Health>();
        Attacks attack = player.GetComponent<Attacks>();
        switch (rune)
        {
            case (int)Runes.Rune_of_Health:
                Hp.IncreaseMaxHealth(-RuneOfHealthAMT);
                Hp.HealUp(Hp.maxHealth);
                break;
            case (int)Runes.Rune_of_Mana:
                attack.IncreaseMaxMana(-RuneOfManaAMT);
                attack.ManaUp((int)attack.maxMana);
                break;
            case (int)Runes.Rune_of_Life:
                Hp.IncreaseMaxHealth(-RuneOfLifeAMT);
                Hp.HealUp(Hp.maxHealth);
                break;
            case (int)Runes.Rune_of_Sacrifice:
                Hp.IncreaseMaxHealth(-RuneOfSacrificeHP);
                Hp.HealUp(-RuneOfSacrificeHP);
                attack.IncreaseMaxMana(-RuneOfSacrificeMana);
                attack.ManaUp(-RuneOfSacrificeMana);
                break;
        }
    }

    public void UnlockAllRunes()
    {
        for (int i = 0; i < 100; i++)
        {
            Add(i);
        }
    }

    public int GetSpectre()
    {
        return RuneOfSpectreDashMana;
    }
}
