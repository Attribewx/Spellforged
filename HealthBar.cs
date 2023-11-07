using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Slider slider2;
    private RectTransform sliderLength;
    [SerializeField] private bool doubleBar;

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider2.maxValue = maxHealth;
        slider2.value = maxHealth;
        sliderLength = slider.gameObject.GetComponent<RectTransform>();
        if (doubleBar)
            sliderLength.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth * 3);
        else
            sliderLength.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetHealthStart(float health)
    {
        slider.value = health;
        slider2.value = health;
    }

    void Update()
    {
        if(slider2.value - slider.value > .1f || slider2.value - slider.value < -.1f)
        {
            slider2.value = Mathf.Lerp(slider2.value, slider.value, .025f);
        }
        else
        {
            slider2.value = slider.value;
        }
    }
}
