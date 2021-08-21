using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    public Image powerUpImage1, powerUpImage2, powerUpImage3;
    bool slot1Full, slot2Full, slot3Full;
    public Slot[] slots;
    private SwitchCharacterScript player;
    public bool allowDrop = true;

    // Start is called before the first frame update
    void Start()
    {
        powerUpImage1.enabled = true;
        powerUpImage2.color = Color.gray;
        powerUpImage3.color = Color.gray;
        player = FindObjectOfType<SwitchCharacterScript>();
        slot1Full = false;
        slot2Full = false;
        slot3Full = false;
    }

    // Update is called once per frame
    void Update()
    {

        int currentSlot=player.getActiveAvatar()-1;

        for (int i = 0;i<slots.Length;i++)
        {
            if (i != currentSlot)
            {
                slots[i].GetComponent<Image>().color= Color.gray;
            }
            else
            {
                slots[i].GetComponent<Image>().color = Color.white;
            }
        }

        if (allowDrop)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                slots[0].DropItem(0.2f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                slots[1].DropItem(0.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                slots[2].DropItem(-0.2f);
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<AudioSource>().Play();
            slots[currentSlot].UsePowerUP();
        }
    }

    public void PickUpPowerUp(int slot, Image powerup)
    {
       
    }


}
