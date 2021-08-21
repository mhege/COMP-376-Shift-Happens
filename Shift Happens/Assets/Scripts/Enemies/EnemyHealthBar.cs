using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 Offset;
    public bool isBoss;
    public string bossName;

    public Text nameText;

    public void Start()
    {
        if (isBoss)
        {
            nameText.text = bossName;
            slider.gameObject.SetActive(true);
        }
            
        
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBoss)
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        }
        
    }
}
