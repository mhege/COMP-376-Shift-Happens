using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject powerUp;
    private SwitchCharacterScript player;
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        player = FindObjectOfType<SwitchCharacterScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int x =player.getActiveAvatar()-1;

        if (collision.CompareTag("Player"))
        {
            for(int i = x; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    //ITEM CAN BE ADDED TO INVENTORY
                    inventory.isFull[i] = true;
                    Instantiate(powerUp, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

}
