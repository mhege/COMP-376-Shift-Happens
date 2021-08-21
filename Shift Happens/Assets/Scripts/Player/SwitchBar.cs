using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBar : MonoBehaviour
{
    // Slider for the health bar
    public Slider slider;
   
    public void SetCooldown(float health)
    {
        slider.value = health;
    }

    public void increment(float amount)
    {
        slider.value = slider.value + amount;
    }
   
    public void SetMaxCooldown(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

}
