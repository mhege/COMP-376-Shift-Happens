using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] buttons;

    public void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 3);
        for (int i = 0; i < buttons.Length; i++)
        {            
            if (i + 2 > levelAt)
            {
                buttons[i].interactable = false;
                buttons[i].gameObject.GetComponent<Image>().color = new Color(195,195,195);
            }
        }

    }

    public void LoadLevel(string name)
    {
        Debug.Log("New Load level: " + name);
        Application.LoadLevel(name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
