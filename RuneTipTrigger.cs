using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneTipTrigger : MonoBehaviour, IPointerEnterHandler
{
    public string content;
    public string header;
   public void OnPointerEnter(PointerEventData eventData)
    {
        RuneTipsSystem.Show(content, header);
    }
}
