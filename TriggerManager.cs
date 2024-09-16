using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private int[] triggers = new int[0];
    public static TriggerManager activeMan;

    void Start()
    {
        if (!activeMan)
        {
            activeMan = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTrigger(int TriggerInst)
    {
        int[] temps = new int[triggers.Length + 1];
        for (int i = 0; i < triggers.Length; i++)
        {
            temps[i] = triggers[i];
        }
        temps[triggers.Length] = TriggerInst;
        triggers = temps;
    }

    public bool FindTrigger(int TriggerInst)
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i] == TriggerInst)
                return true;
        }
        return false;

    }
}
