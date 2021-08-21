using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDoor : MonoBehaviour
{
    public bool lastDoor;

    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 playerPosition = collision.GetComponentInParent<Player>().transform.position;
            switch (this.doorType)
            {
                case DoorType.left:
                    playerPosition.x += 1.1f;
                    collision.GetComponentInParent<Player>().transform.position = playerPosition;
                    break;
                case DoorType.right:
                    playerPosition.x -= 1.1f;
                    collision.GetComponentInParent<Player>().transform.position = playerPosition;
                    break;
                case DoorType.bottom:
                    playerPosition.y += 2.6f;
                    collision.GetComponentInParent<Player>().transform.position = playerPosition;
                    break;
                case DoorType.top:
                    playerPosition.y -= 2.6f;
                    collision.GetComponentInParent<Player>().transform.position = playerPosition;
                    break;
            }

            if (!lastDoor)
                SpecialCameraController.instance.IncreseRoom();
            else
                SpecialCameraController.instance.LastRoom();
        }
    }

}
