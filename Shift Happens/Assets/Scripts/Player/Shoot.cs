using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Ammo
    int humanAmmo;
    int alienAmmo;
    int robotAmmo;

    int humanAmmoCapacity;
    int alienAmmoCapacity;
    int robotAmmoCapacity;

    int humanInitialAmmo = 75;
    int alienInitialAmmo = 50;
    int robotInitialAmmo = 25;

    int ammoCapacity = 0;
    int ammoCount = 0;
    int fireRate = 0;

    public WeaponBar weaponBar;
    public SwitchCharacterScript switchCharacter;
    public AbilityBar abilityBar;

    // Start is called before the first frame update
    void Start()
    {
        humanAmmo = humanInitialAmmo;
        alienAmmo = alienInitialAmmo;
        robotAmmo = robotInitialAmmo;

        humanAmmoCapacity = humanInitialAmmo;
        alienAmmoCapacity = alienInitialAmmo;
        robotAmmoCapacity = robotInitialAmmo;
        ammoCapacity = humanAmmoCapacity;
        ammoCount = humanAmmo;
        weaponBar.setWeapon1(true);
        weaponBar.setWeapon2(false);
        weaponBar.setWeapon3(false);
        weaponBar.setAmmoCapacity(ammoCapacity);
        weaponBar.setAmmoCount(ammoCount);
        abilityBar.setabilityHuman(true);
        abilityBar.setabilityAlien(false);
        abilityBar.setabilityRobot(false);
    }

    // Update is called once per frame
    void Update()
    {
            // Update the weapon bar
            if (switchCharacter.getActiveAvatar() == 1)
            {
                ammoCapacity = humanAmmoCapacity;
                ammoCount = humanAmmo;
                weaponBar.setWeapon1(true);
                weaponBar.setWeapon2(false);
                weaponBar.setWeapon3(false);
                weaponBar.setAmmoCapacity(ammoCapacity);
                weaponBar.setAmmoCount(ammoCount);
                abilityBar.setabilityHuman(true);
                abilityBar.setabilityAlien(false);
                abilityBar.setabilityRobot(false);
        }
            else if (switchCharacter.getActiveAvatar() == 2)
            {
                ammoCapacity = alienAmmoCapacity;
                ammoCount = alienAmmo;
                weaponBar.setWeapon1(false);
                weaponBar.setWeapon2(true);
                weaponBar.setWeapon3(false);
                weaponBar.setAmmoCapacity(ammoCapacity);
                weaponBar.setAmmoCount(ammoCount);
                abilityBar.setabilityHuman(false);
                abilityBar.setabilityAlien(true);
                abilityBar.setabilityRobot(false);
        }
            else if (switchCharacter.getActiveAvatar() == 3)
            {
                ammoCapacity = robotAmmoCapacity;
                ammoCount = robotAmmo;
                weaponBar.setWeapon1(false);
                weaponBar.setWeapon2(false);
                weaponBar.setWeapon3(true);
                weaponBar.setAmmoCapacity(ammoCapacity);
                weaponBar.setAmmoCount(ammoCount);
                abilityBar.setabilityHuman(false);
                abilityBar.setabilityAlien(false);
                abilityBar.setabilityRobot(true);
        }
    }
}
