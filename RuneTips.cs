using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class RuneTips : MonoBehaviour
{
    public TextMeshProUGUI RuneName;
    public TextMeshProUGUI RuneDesc;

    public void SetText(string content, string header = "")
    {
        RuneDesc.text = content;
        RuneName.text = header;
    }
}
