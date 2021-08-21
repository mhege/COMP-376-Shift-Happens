using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Slider for the health bar
    public Slider slider;
   
    public void SetHealth(int health)
    {
        slider.value = health;
    }
   
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }


    public float HealthAmount()
    {
        return slider.value;
    }

}
