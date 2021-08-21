using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
   
    public Text ammoCountText;
    public Text ammoCapacityText;
    public Text reloadText;
    public Image weaponHumanImage;
    public Image weaponAlienImage;
    public Image weaponRobotImage;
    int ammoCountNum = 0;
    int ammoCapacityNum = 0;

    public GameObject weapon1, weapon2, weapon3;
    

    // Functions for setting ammo on UI
    public void setAmmoCount(int ammoCount)
    {
        ammoCountNum = ammoCount;
    }

    public void currentAmmo(int ammo)
    {        
        ammoCountNum = ammo;
        ammoCountText.text = ammoCountNum.ToString();
        ammoCapacityText.text = "/" + ammoCapacityNum.ToString();
    }
    public int getAmmoCount()
    {
        return ammoCountNum;
    }
    public void setAmmoCapacity(int ammoCapacity)
    {
        ammoCapacityNum = ammoCapacity;
    }

    // Functions for setting active weapon on UI
    public void setWeapon1(bool value )
    {
        weapon1.SetActive(value);
    }
    public void setWeapon2(bool value)
    {
        weapon2.SetActive(value);
    }
    public void setWeapon3(bool value)
    {
        weapon3.SetActive(value);
    }



    public void isReloading()
    {
        ammoCapacityText.text = "RELOADING";
    }



}
