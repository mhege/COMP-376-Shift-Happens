using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBoost : PowerUp
{
    private MoveScript player;
    public float duration;
    public Text powerUpText;
    private PowerUpBar powerUpBar;

    public override void activatePowerUp()
    {
        StartCoroutine(dashActivation());
    }

    IEnumerator dashActivation()
    {
        switch (player.switchCharacterScript.getActiveAvatar())
        {
            case 1:
                player.humanDashSpeed *= 1.5f;
                break;
            case 2:
                player.alienDashSpeed *= 1.5f;
                break;
            case 3:
                player.robotDashSpeed *= 3f;
                break;
        }
        player.switchCharacterScript.switchAvailable = false;
        GetComponent<Image>().color = Color.gray;
        powerUpText.text = "Increased Dash Speed";
        powerUpBar.allowDrop = false;
        yield return new WaitForSeconds(duration);
        // Set animator and movement speed according to selected character
        if (player.switchCharacterScript.getActiveAvatar() == 1)
        {
            player.humanDashSpeed = player.dfltDashHuman;
        }
        else if (player.switchCharacterScript.getActiveAvatar() == 2)
        {
            player.alienDashSpeed = player.dfltDashAlien;
        }
        else if (player.switchCharacterScript.getActiveAvatar() == 3)
        {
            player.robotDashSpeed = player.dfltDashRobot;
        }
        player.switchCharacterScript.switchAvailable = true;
        powerUpText.text = "";
        powerUpBar.allowDrop = true;
        Destroy(gameObject);
    }



    public override void Start()
    {
        player = FindObjectOfType<MoveScript>();
        powerUpBar = FindObjectOfType<PowerUpBar>();
        powerUpText = FindObjectOfType<TextPowerUp>().currentText;
    }
}
