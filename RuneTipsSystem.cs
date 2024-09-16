using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneTipsSystem : MonoBehaviour
{
    private static RuneTipsSystem current;
    public RuneTips runetip;

    public void Awake()
    {
        if(!current)
        current = this;
    }

    public static void Show(string content, string header = "")
    {
        current.runetip.SetText(content, header);
        current.runetip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.runetip.gameObject.SetActive(false);
    }
}
