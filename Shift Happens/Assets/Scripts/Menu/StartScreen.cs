using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartScreen : MonoBehaviour
{
    public Canvas canvas;

    public void Disable()
    {
        canvas.enabled = false;
    }

}
