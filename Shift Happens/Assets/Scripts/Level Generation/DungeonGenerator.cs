using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    [SerializeField]
    public int numberOfCrawler;
    [SerializeField]
    public int minimumIterations;
    [SerializeField]
    public int maximumIterations;
    [SerializeField]
    private int minimunNumberOfRooms;

    public List<Vector2Int> dungeonRoom;

    private float roomHeight = 6.0f;
    private float roomWidth = 10.666667f;

    private string roomName;

    private bool bossGenerated = false;
    private int miniBoss = 0;


    private void Start()
    {
        if(maximumIterations >= minimumIterations && minimumIterations > 0)
        {
            GenerateNewDungeon();
        }
        else
        {
            Debug.Log("Max number of iterations should be bigger than min, and min > 0");
        }
            
    }

    private void GenerateNewDungeon()
    {
        dungeonRoom = DungeonCrawlerController.GenerateDungeon(numberOfCrawler, minimumIterations, maximumIterations);
        SpawnRooms(dungeonRoom);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        //Starting Room
        RoomController.instance.LoadRoom(0, 0, "Starting Room 0, 0", roomWidth, roomHeight);

        int miniBossFloors = (dungeonRoom.Count - 1) / 4;
        int miniIter = miniBossFloors;


        foreach (Vector2Int roomLocation in rooms)
        {
            if(roomLocation == dungeonRoom[dungeonRoom.Count - 1] && farEnough(roomLocation))
            {
                bossGenerated = true;
                roomName = "Boss " + roomLocation.x + ", " + roomLocation.y;
                RoomController.instance.LoadRoom(roomLocation.x, roomLocation.y, roomName, roomWidth, roomHeight);
            }
            else if (roomLocation == dungeonRoom[miniBossFloors] && !(roomLocation == Vector2Int.zero) && miniBoss <= 2)
            {
                miniBoss++;
                miniBossFloors = miniBossFloors + miniIter;
                roomName = "MiniBoss " + roomLocation.x + ", " + roomLocation.y;
                RoomController.instance.LoadRoom(roomLocation.x, roomLocation.y, roomName, roomWidth, roomHeight);
            }
            else
            {
                roomName = "Room " + roomLocation.x + ", " + roomLocation.y;
                RoomController.instance.LoadRoom(roomLocation.x, roomLocation.y, roomName, roomWidth, roomHeight);
            }
            
        }

        

        if (!bossGenerated || RoomController.instance.loadedRooms.Count < minimunNumberOfRooms)
        {
            //Debug.Log("No boss was generated or the number of rooms was too small");
            RoomController.instance.ClearRooms();
            miniBoss = 0;
            dungeonRoom.Clear();
            GenerateNewDungeon();
        }
        else
        {
            RoomController.instance.checkDoors();
        }
       
    }

    private bool farEnough(Vector2Int bossRoomPosition)
    {
        int distance = Mathf.Abs(bossRoomPosition.x) + Mathf.Abs(bossRoomPosition.y);
        if(distance < 3)
        {
            return false;
        }
        return true;
    }
}
