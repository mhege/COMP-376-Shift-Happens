using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGun : MonoBehaviour
{
    // Firepoint variables
    public Transform firePointDown, firePointUp, firePointLeft, firePointRight;
    public Transform firePoint;


    public GameObject bulletToFire;
    public float timeBetweenShots;
    public float dfltTimeBetweenShots;
    private float shotCounter; //keeps track of how fast shots should fire
    public WeaponBar weaponBar;
    int ammoCount = 0;
    int bulletsShoot = 0;

    // Variables for swapping 
    public SwitchCharacterScript switchCharacterScript; 
    Animator animator;
    public MoveScript moveScript;

    bool outOfAmmo =false;

    private void Start()
    {
        firePoint = firePointDown;
        dfltTimeBetweenShots = timeBetweenShots;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(moveScript.transform.localPosition);
        float localX = firePointDown.localScale.x;
        float localY = firePointDown.localScale.y;

        
        // switch direction based of gun 
        animator = switchCharacterScript.getActiveAnimator();

        // Change fire point 
        if(moveScript.getFacingHorizontal())
        if (mousePos.x < screenPoint.x)
        {
            animator.SetFloat("Horizontal Look", -1);
            firePoint = firePointLeft;
        }
        else 
        {
            animator.SetFloat("Horizontal Look", 1);
            firePoint = firePointRight;
        }

        if (moveScript.getFacingVertical())
        if (mousePos.y < screenPoint.y)
        {
            animator.SetFloat("Vertical Look", -1);
            firePoint = firePointDown;
        }
        else
        {
            animator.SetFloat("Vertical Look", 1);
            firePoint = firePointUp;
        }

        // Fire point rotation 
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);


        ammoCount = weaponBar.getAmmoCount();


        if ((ammoCount - bulletsShoot == -1) && !outOfAmmo)
        {            
            StartCoroutine(reload());
        }

        if (!outOfAmmo)
        {
            weaponBar.reloadText.text = "";
            if (Input.GetMouseButton(0)) //continuous shooting
            {
                shotCounter -= Time.deltaTime;
                weaponBar.currentAmmo(ammoCount - bulletsShoot);
                if (shotCounter <= 0)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    bulletsShoot++;
                    shotCounter = timeBetweenShots;
                }
            }
            else
            {
                weaponBar.currentAmmo(ammoCount - bulletsShoot);
            }
        }
        else
        {
            weaponBar.ammoCapacityText.text = "";
            weaponBar.ammoCountText.text = "";
            weaponBar.reloadText.text = "RELOADING";
        }
        
       
    }

    IEnumerator reload()
    {
        outOfAmmo = true;
        yield return new WaitForSeconds(1.5f);
        bulletsShoot = 0;
        outOfAmmo = false;
        
    }

}