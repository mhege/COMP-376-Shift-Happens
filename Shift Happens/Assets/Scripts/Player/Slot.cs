using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;

    public void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void Update()
    {
        if (transform.childCount <=0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem(float offset)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem(offset);
            GameObject.Destroy(child.gameObject);
        }
    }

    public void UsePowerUP()
    {
        foreach (Transform child in transform)
        {                        
            child.GetComponent<PowerUp>().activatePowerUp();            
        }
    }




}
