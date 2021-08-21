using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    public float width;

    public float height;

    public int x;

    public int y;

    public string roomName;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Door> doors = new List<Door>();

    public EnemiesSpawner[] enemiesSpawner;

    public bool alreadyBeaten = false;

    // Start is called before the first frame update
    void Start()
    {

        addDoors();
    }

    public void addDoors()
    {
        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }

        RemoveUnconnectedDoors();
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door d in doors)
        {
            switch (d.doorType)
            {
                case Door.DoorType.right:
                    if (GetRight())
                        d.gameObject.SetActive(false);
                    break;
                case Door.DoorType.left:
                    if (GetLeft())
                        d.gameObject.SetActive(false);
                    break;
                case Door.DoorType.top:
                    if (GetTop())
                        d.gameObject.SetActive(false);
                    break;
                case Door.DoorType.bottom:
                    if (GetBottom())
                        d.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public bool GetRight()
    {
        if(RoomController.instance.RoomExist(x + 1, y))
        {
            return false;
        }
        return true;
    }

    public bool GetLeft()
    {
        if (RoomController.instance.RoomExist(x - 1, y))
        {
            return false;
        }
        return true;
    }

    public bool GetTop()
    {
        if (RoomController.instance.RoomExist(x, y + 1))
        {
            return false;
        }
        return true;
    }

    public bool GetBottom()
    {
        if (RoomController.instance.RoomExist(x, y - 1))
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }


    public Vector3 GetRoomPosition()
    {
        return new Vector3(x * width, y * height);
    }

    //Picks a spawner to send waves of enemies
    public void pickSpawner()
    {
        GameObject.Find("RoomClear").GetComponent<Text>().text = "";
        int index = Random.Range(0, enemiesSpawner.Length);
        enemiesSpawner[index].SendWaves();

        //Destroy all the other spawner for freeing memory
        for(int i=0; i < enemiesSpawner.Length; i++)
        {
            if(i != index)
            {
               enemiesSpawner[i].DestroySpawner();
            }
        }
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
