using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;

    // Update is called once per frame
    void Start()
    {
        Time.timeScale = 1f;
    }


    public void GameOver()
    {
        gameOver.SetActive(true);
        if (GameObject.Find("Canvas").GetComponent<AudioSource>() && GameObject.Find("Canvas").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("Canvas").GetComponent<AudioSource>().Stop();
        }
        else if (GameObject.FindWithTag("bossRoom") && GameObject.FindWithTag("bossRoom").GetComponent<AudioSource>())
        {
            GameObject.FindWithTag("bossRoom").GetComponent<AudioSource>().Stop();
        }
        
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
