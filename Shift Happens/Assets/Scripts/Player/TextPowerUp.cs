﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    public Text currentText;

    void Start()
    {
        currentText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string powerup)
    {
        currentText.text = powerup;
    }

}
