using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class main : MonoBehaviour
{

    [Header("Player")]
    public GameObject Player;

    [Header("Enemy types")]
    public GameObject Small;
    public GameObject Medium;
    public GameObject Large;
    public GameObject Boss;

    //private List<GameObject> enemyList = new List<GameObject>();

    public List<GameObject> smallEnemyList = new List<GameObject>();
    private List<GameObject> mediumEnemyList = new List<GameObject>();
    private List<GameObject> largeEnemyList = new List<GameObject>();
    private List<GameObject> bossEnemyList = new List<GameObject>();

    GameObject currentPlayer;
    GameObject SmallEnemy;
    GameObject MediumEnemy;
    GameObject LargeEnemy;
    GameObject BossEnemy;

    // Start is called before the first frame update
    void Start()
    { 
        //enemyList.Add(null);
        //for
        //GameObject SmallEnemy = Instantiate(Small, new Vector3(0.0f, 2.0f, -5.0f), Quaternion.identity) as GameObject;//can run it through a for loop for many
        //smallEnemyList.Add(SmallEnemy);
        //
        //for
        //GameObject MediumEnemy = Instantiate(Medium, new Vector3(1.0f, 0.0f, -5.0f), Quaternion.identity) as GameObject;//can run it through a for loop for many
        //mediumEnemyList.Add(MediumEnemy);
        //
        //for
        //GameObject LargeEnemy = Instantiate(Large, new Vector3(-1.0f, 0.0f, -5.0f), Quaternion.identity) as GameObject;//can run it through a for loop for many
        //largeEnemyList.Add(LargeEnemy);
        //
        //for
        //GameObject BossEnemy = Instantiate(Boss, new Vector3(2.0f, 0.0f, -5.0f), Quaternion.identity) as GameObject;//can run it through a for loop for many
        //bossEnemyList.Add(BossEnemy);
        //
        //currentPlayer = Instantiate(Player, new Vector3(-8.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
    }

    /*
    public List<GameObject> getList()
    {
        return enemyList;
    }

    public int Length()
    {
        return enemyList.Count;
    }

    public GameObject atIndex(int i)
    {
        return enemyList[i];
    }
    */
    //small Access
    public List<GameObject> getSmallList()
    {
        return smallEnemyList;
    }

    public void addToSmallList(GameObject small)
    {
        smallEnemyList.Add(small);
    }

    public int smallLength()
    {
        return smallEnemyList.Count;
    }

    public GameObject atIndexSmall(int i)
    {
        return smallEnemyList[i];
    }
    
    //boss Access
    public List<GameObject> getBossList()
    {
        return bossEnemyList;
    }

    public int bossLength()
    {
        return bossEnemyList.Count;
    }

    public GameObject atIndexBoss(int i)
    {
        return bossEnemyList[i];
    }
    //
}


