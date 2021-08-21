using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{    
    public GameObject[] powerUps;
    GameObject item;

    public void Start()
    {
        int random = Random.Range(0, 5);
        item = powerUps[random];
    }

    public void DropItem()
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }
    
}
