using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTutorial : MonoBehaviour
{
    public LevelLoader levelLoader;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Y");
        collision.GetComponentInParent<Player>().enabled = false;
        levelLoader.LevelComplete();
    }
}
