using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanAbility : MonoBehaviour
{
    // Start is called before the first frame update
    public ShootingGun player;
    public float cooldownTime = 15;
    public Image imageCooldown;
    bool isColdown = false;
    public ProjectileBehaviour lauchableProjectilePrefab;
    Transform firePoint;
    void Start()
    {
        cooldownTime = 9;
        firePoint = player.firePoint;
    }

    // Update is called once per frame
    void Update()
    {
        firePoint = player.firePoint;
        if (!isColdown)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Instantiate(lauchableProjectilePrefab, firePoint.position, firePoint.rotation);
                isColdown = true;
            }
        }
        if (isColdown)
        {
            imageCooldown.fillAmount += 1 / cooldownTime * Time.deltaTime;
            if (imageCooldown.fillAmount >= 1)
            {
                imageCooldown.fillAmount = 0;
                isColdown = false;
            }
        }
    }
}
