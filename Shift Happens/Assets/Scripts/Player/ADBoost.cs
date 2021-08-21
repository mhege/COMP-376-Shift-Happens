using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADBoost : PowerUp
{
    private ShootingGun player;
    public float duration;
    public Text powerUpText;
    private PowerUpBar powerUpBar;

    public override void activatePowerUp()
    {
        StartCoroutine(ADBoostActivation());
    }

    public override void Start()
    {
        player = FindObjectOfType<ShootingGun>();
        powerUpText = FindObjectOfType<TextPowerUp>().currentText;
        powerUpBar = FindObjectOfType<PowerUpBar>();
    }

    IEnumerator ADBoostActivation()
    {        
        player.bulletToFire.GetComponent<PlayerBullet>().damageToGive *= 2;
        GetComponent<Image>().color = Color.gray;
        player.switchCharacterScript.switchAvailable = false;
        powerUpText.text = "Increased Attack Damage";
        powerUpBar.allowDrop = false;

        yield return new WaitForSeconds(duration);

        player.bulletToFire.GetComponent<PlayerBullet>().damageToGive = player.bulletToFire.GetComponent<PlayerBullet>().dftlDmgToGive;
        player.switchCharacterScript.switchAvailable = true;
        powerUpText.text = "";
        powerUpBar.allowDrop = true;
        Destroy(gameObject);
    }

}
