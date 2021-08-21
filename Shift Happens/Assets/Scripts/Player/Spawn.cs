using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem(float offset)
    {
        Vector3 playerPos = new Vector3(player.position.x + 0.3f, player.position.y+offset,player.position.z);
        Instantiate(item, playerPos, Quaternion.identity);
    }

}
