using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ASpBoost : PowerUp
{
    private ShootingGun player;
    public float duration;
    public Text powerUpText;
    private PowerUpBar powerUpBar;

    public override void activatePowerUp()
    {
        StartCoroutine(ASpBoostActivation());
    }

    public override void Start()
    {
        player = FindObjectOfType<ShootingGun>();
        powerUpText = FindObjectOfType<TextPowerUp>().currentText;
        powerUpBar = FindObjectOfType<PowerUpBar>();
    }

    IEnumerator ASpBoostActivation()
    {
        player.timeBetweenShots /= 1.5f;

        GetComponent<Image>().color = Color.gray;
        player.switchCharacterScript.switchAvailable = false;
        powerUpText.text = "Attack Speed";
        powerUpBar.allowDrop = false;

        yield return new WaitForSeconds(duration);        

        player.timeBetweenShots = player.dfltTimeBetweenShots;
        player.switchCharacterScript.switchAvailable = true;
        powerUpText.text = "";
        powerUpBar.allowDrop = true;
        Destroy(gameObject);
    }

}
