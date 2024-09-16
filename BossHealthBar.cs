using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField, Header("Boss Health Bar Setup")] private TMP_Text nombre;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider slider2;
    private GameObject boss;
    public void InitializeBar(string bossName, float healthValue)
    {
        nombre.text = bossName;
        healthBar.maxValue = healthValue;
        healthBar.value = healthValue;
        healthBar.gameObject.SetActive(true);
        slider2.maxValue = healthValue;
        slider2.value = healthValue;
        slider2.gameObject.SetActive(true);
    }

    public void UninitializeBar()
    {
        healthBar.gameObject.SetActive(false);
        slider2.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(boss == null && FindObjectOfType<Boss>())
        boss = FindObjectOfType<Boss>().gameObject;
    }

    public void setBar(float decreaseAMT)
    {
        healthBar.value = healthBar.value - decreaseAMT;
    }

    void Update()
    {
        if (slider2.value - healthBar.value > .1f || slider2.value - healthBar.value < -.1f)
        {
            slider2.value = Mathf.Lerp(slider2.value, healthBar.value, .025f);
        }
        else
        {
            slider2.value = healthBar.value;
        }
    }

    public void FindBoss()
    {
        boss = FindObjectOfType<Boss>().gameObject;
    }
}
