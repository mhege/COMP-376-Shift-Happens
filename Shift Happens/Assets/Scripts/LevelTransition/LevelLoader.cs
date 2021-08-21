using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    bool levelComplete = false;
    public Animator transition;

    public Animator levelName;

    public bool hasName;
    public string name;
    public Text levelText;


    public bool shouldNotSave;
    SaveData saveData;


    private void Start()
    {
        if (hasName)
        {
            levelText.text = name;
        }
        // save

        else
        {
            levelName = new Animator();
        }

        if (!shouldNotSave)
        {
            Data data = new Data();
            data.level = SceneManager.GetActiveScene().buildIndex;
            
            SaveSystem.SaveData(data);
        }

    }


    // Update is called once per frame
    void Update()
    {
        if(hasName)
            if (levelName.GetCurrentAnimatorStateInfo(0).IsName("IdleOff"))
                levelText.enabled = false;


        if (levelComplete)
            LoadNextLevel();
    }


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel (SceneManager.GetActiveScene().buildIndex + 1, 1));
    }

    IEnumerator LoadLevel(int levelIndex, int transitionTime)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void LevelComplete()
    {
        levelComplete = true;
    }




    public void LoadSpecificLevel()
    {
        saveData = SaveSystem.LoadData();
        StartCoroutine(LoadLevel(saveData.level, 1));
    
    }
}
