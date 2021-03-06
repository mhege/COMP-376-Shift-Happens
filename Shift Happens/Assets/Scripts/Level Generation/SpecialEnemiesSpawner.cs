using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemiesSpawner : MonoBehaviour
{

    private enum State
    {
        Idle,
        Active,
        RoomCleared,
    }

    [SerializeField] private Wave[] waves;
    [SerializeField] private bool shouldWaitPreviousWave;

    private State state;

    private void Awake()
    {
        state = State.Idle;
        Wave.height = this.GetComponentInParent<SpecialRoom>().height;
        Wave.width = this.GetComponentInParent<SpecialRoom>().width;

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Active:
                //foreach (Wave wave in waves)
                for(int i = 0; i < waves.Length; i++)
                {
                    waves[i].x = this.GetComponentInParent<SpecialRoom>().x;
                    waves[i].y = this.GetComponentInParent<SpecialRoom>().y;
                    if (shouldWaitPreviousWave && i !=0)
                    {
                        if (waves[i - 1].IsWaveOver() && !waves[i].alreaySent)
                        {
                            waves[i].alreaySent = true;
                            waves[i].SpawnEnemies();
                        }
                    }
                    else
                    {
                        waves[i].CheckTimer();
                    }
                   
                    
                    //Check if the room is cleared only every 10 frames
                    if(Time.frameCount % 10 == 0)
                    {
                        checkIfRoomCleared();
                    }
                   
                }
                break;
        }
        
    }

    public void SendWaves()
    {
        if (state == State.Idle)
        {
            state = State.Active;
        }
    }

    private void checkIfRoomCleared()
    {
        if(state == State.Active)
        {
            if (isRoomCleared())
            {
                Debug.Log("Room Cleared");
                state = State.RoomCleared;
                RoomController.instance.allEnemiesCleared = true;
                this.GetComponentInParent<Room>().alreadyBeaten = true;
                DestroySpawner();
            }
        }
    }

    private bool isRoomCleared()
    {
        foreach (Wave wave in waves)
        {
            if (!wave.IsWaveOver())
            {
                return false;
            }
        }
        return true;
    }

    public void DestroySpawner()
    {
        Destroy(gameObject);
    }

    [System.Serializable]
    private class Wave
    {
        [SerializeField] private GameObject[] enemyToSpawn;
        [SerializeField] private float timer;
        private List<GameObject> enemiesSpawned = new List<GameObject>();
        [HideInInspector] public float x;
        [HideInInspector] public float y;
        [HideInInspector] public static float width;
        [HideInInspector] public static float height;
        [HideInInspector] public bool alreaySent = false;

        main mainScript;

        //Needs to be between +/- Width/2 and +/- Height/2
        [SerializeField] private Vector2[] spawnPoints;

        private void Start()
        {
            mainScript = GameObject.FindGameObjectWithTag("main").GetComponent<main>();
        }

        public void CheckTimer()
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    SpawnEnemies();
                }
            }
        }

        public void SpawnEnemies()
        {
            timer = -1;
            foreach (GameObject enemy in enemyToSpawn)
            {
                GameObject myEnemy = Instantiate(enemy, new Vector3(x * width + pickSpawnPoint().x, y * height + pickSpawnPoint().y, -5.0f ), Quaternion.identity);
                if(myEnemy.tag == "Small")
                {
                    mainScript.addToSmallList(myEnemy);
                }
                
                enemiesSpawned.Add(myEnemy);
            }
        }

        public bool IsWaveOver()
        {
            if(timer < 0)
            {
                foreach(GameObject enemy in enemiesSpawned)
                {
                    if(enemy != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private Vector2 pickSpawnPoint()
        {
            if(spawnPoints.Length > 0)
            {
                int index = Random.Range(0, spawnPoints.Length);
                return spawnPoints[index];
            }
            else
            {
                return Vector2.zero;
            }
           
        }
    }
}
