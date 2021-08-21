using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    public Image abilityHumanImage;
    public Image abilityAlienImage;
    public Image abilityRobotImage;

    public GameObject abilityHuman, abilityAlien, abilityRobot;

    // Functions for setting active ability on UI
    public void setabilityHuman(bool value)
    {
        abilityHuman.SetActive(value);
    }
    public void setabilityAlien(bool value)
    {
        abilityAlien.SetActive(value);
    }
    public void setabilityRobot(bool value)
    {
        abilityRobot.SetActive(value);
    }


}
