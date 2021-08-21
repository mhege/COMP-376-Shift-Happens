using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoom : MonoBehaviour
{

    public SpecialCameraController cam;

    public float width;

    public float height;

    public int x;

    public int y;

    public string roomName;

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public SpecialRoom bottom;
    public SpecialRoom top;

    public List<Door> doors = new List<Door>();

    public EnemiesSpawner[] enemiesSpawner;

    public bool alreadyBeaten = false;

    public bool boss;

    public BossEnemy bossEnemy;

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

    public bool GetTop()
    {
        if (top)
        {
            return false;
        }
        return true;
    }

    public bool GetBottom()
    {
        if (bottom)
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
        return new Vector3(transform.position.x * width, transform.position.y * height);
    }

    //Picks a spawner to send waves of enemies
    public void Spawn()
    {
        if (gameObject.GetComponent<AudioSource>())
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        Instantiate(bossEnemy, transform);
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
