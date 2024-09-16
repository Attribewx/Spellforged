using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int[] inv;

    public InventoryData (Inventory inven)
    {
        inv = new int[inven.items.Length];
        for (int i = 0; i < inven.items.Length; i++)
        {
            inv[i] = inven.items[i];
        }
    }
}
