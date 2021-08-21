using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBost : PowerUp
{
    // Start is called before the first frame update
    private MoveScript player;
    public float duration;
    public Text powerUpText;
    private PowerUpBar powerUpBar;
    public override void Start()
    {
        player = FindObjectOfType<MoveScript>();
        powerUpBar = FindObjectOfType<PowerUpBar>();
        powerUpText = FindObjectOfType<TextPowerUp>().currentText;
    }

    IEnumerator speedActivation()
    {
        switch (player.switchCharacterScript.getActiveAvatar())
        {
            case 1:
                player.humanMoveSpeed *= 2f;
                break;
            case 2:
                player.alienMoveSpeed *= 1.5f;
                break;
            case 3:
                player.robotMoveSpeed *= 3f;
                break;
        }
        player.switchCharacterScript.switchAvailable = false;
        GetComponent<Image>().color = Color.gray;
        powerUpText.text = "Movement Speed";
        powerUpBar.allowDrop = false;
        yield return new WaitForSeconds(duration);
        // Set animator and movement speed according to selected character
        if (player.switchCharacterScript.getActiveAvatar() == 1)
        {            
            player.humanMoveSpeed = player.dfltSpeedHuman;
        }
        else if (player.switchCharacterScript.getActiveAvatar() == 2)
        {
            player.alienMoveSpeed = player.dfltSpeedAlien;
        }
        else if (player.switchCharacterScript.getActiveAvatar() == 3)
        {
            player.robotMoveSpeed = player.dfltSpeedRobot;
        }
        player.switchCharacterScript.switchAvailable = true;
        powerUpText.text = "";
        powerUpBar.allowDrop = true;
        Destroy(gameObject);
    }

    public override void activatePowerUp()
    {
        StartCoroutine(speedActivation());
    }
       
}
