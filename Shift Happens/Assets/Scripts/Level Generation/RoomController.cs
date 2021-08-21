using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    public GameObject[] roomPrefabs;
    public GameObject[] miniBossRoomPrefabs;


    public GameObject startRoom;
    public GameObject bossRoom;


    public List<Room> loadedRooms = new List<Room>();

    public Room currentRoom;

    public bool allEnemiesCleared = true;

    private void Awake()
    {
        instance = this;
    }

    public void LoadRoom(int x, int y, string name, float width, float height)
    {
        GameObject newRoom;
        if (RoomExist(x, y))
        {
            return;
        }
        else if (x == 0 && y == 0)
        {
            newRoom = Instantiate(startRoom, new Vector3(x * width, y * height, 0), Quaternion.identity);
        }
        else if (name.StartsWith("Boss"))
        {
            newRoom = Instantiate(bossRoom, new Vector3(x * width, y * height, 0), Quaternion.identity);
        }
        else if (name.StartsWith("MiniBoss"))
        {
            newRoom = Instantiate(GetPrefabRoom(miniBossRoomPrefabs), new Vector3(x * width, y * height, 0), Quaternion.identity);
        }
        else
        {
            newRoom = Instantiate(GetPrefabRoom(roomPrefabs), new Vector3(x * width, y * height, 0), Quaternion.identity);
        }
        
        
        newRoom.name = name;
        newRoom.GetComponent<Room>().roomName = name;
        newRoom.GetComponent<Room>().x = x;
        newRoom.GetComponent<Room>().y = y;
        newRoom.GetComponent<Room>().width = width;
        newRoom.GetComponent<Room>().height = height;

        if (loadedRooms.Count == 0)
        {
            CameraController.instance.currentRoom = newRoom.GetComponent<Room>();
            currentRoom = newRoom.GetComponent<Room>();
        }

        loadedRooms.Add(newRoom.GetComponent<Room>());

    }

    public void checkDoors()
    {
        foreach(Room r in loadedRooms)
        {
            r.addDoors();
        }
    }

    public bool RoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.x == x && item.y == y) != null;
    }

    public GameObject GetPrefabRoom(GameObject[] arrayToPick)
    {
        return arrayToPick[Random.Range(0, arrayToPick.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        if (room.enemiesSpawner.Length > 0 && room != currentRoom && !room.alreadyBeaten)
        {
            room.pickSpawner();
            allEnemiesCleared = false;
        }

        if (room.GetComponent<AudioSource>() && GameObject.Find("Canvas").GetComponent<AudioSource>())
        {
            GameObject.Find("Canvas").GetComponent<AudioSource>().Stop();
            room.GetComponent<AudioSource>().Play();
        }
            
        currentRoom = room;

    }

    public void ClearRooms()
    {
        foreach(Room room in loadedRooms)
        {
            Destroy(room);
        }
        loadedRooms.Clear();
        loadedRooms.TrimExcess();
    }
}
