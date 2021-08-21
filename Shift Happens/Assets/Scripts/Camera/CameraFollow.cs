using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //we store current camera's position in variable temp
        Vector3 temp = transform.position;

        //we set camera's position x to be equal to the player's postion x
        temp.x = playerTransform.position.x;

        //we set camera's position y to be equal to the player's postion y
        temp.y = playerTransform.position.y;

        // we set back the camera's temp position to the camera's current positon
        transform.position = temp;
    }
}
