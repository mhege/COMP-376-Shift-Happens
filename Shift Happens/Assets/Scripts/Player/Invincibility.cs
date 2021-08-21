using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invincibility : PowerUp
{
    private Player player;
    public float duration;
    public Text powerUpText;
    private PowerUpBar powerUpBar;

    public override void activatePowerUp()
    {
        StartCoroutine(InvincibilityActivation());
    }

    public override void Start()
    {
        player = FindObjectOfType<Player>();
        powerUpText = FindObjectOfType<TextPowerUp>().currentText;
        powerUpBar = FindObjectOfType<PowerUpBar>();
    }

    IEnumerator InvincibilityActivation()
    {
        player.isInvincible = true;
        GetComponent<Image>().color = Color.gray;
        player.switchCharacter.switchAvailable = false;
        powerUpText.text = "Invincible";
        powerUpBar.allowDrop = false;

        yield return new WaitForSeconds(duration);


        player.switchCharacter.switchAvailable = true;
        player.isInvincible = false;
        powerUpText.text = "";
        powerUpBar.allowDrop = true;
        Destroy(gameObject);

    }

}
