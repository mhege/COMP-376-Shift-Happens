using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCameraController : MonoBehaviour
{
    public bool tut = true;
    public TutorialManager tutMan;

    public LevelLoader levelLoader;

    public static SpecialCameraController instance;

    public SpecialRoom currentRoom;
    public SpecialRoom[] rooms;
    int i = 0;

    public float cameraMoveSpeed;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if(currentRoom == null)
        {
            return;
        }

        Vector3 targetPosition = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
    }

    public void IncreseRoom()
    {
        i++;
        if(tut)
            tutMan.Next();
        currentRoom = rooms[i];
        if (currentRoom.boss)
            currentRoom.Spawn();
    }

    public void LastRoom()
    {
        currentRoom = rooms[i];
        levelLoader.LevelComplete();
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            return Vector3.zero;
        }
        Vector3 targetPosition = rooms[i].transform.position;
        targetPosition.z = transform.position.z;

        return targetPosition;
    }

    public bool isSwitchingRoom()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }

}
