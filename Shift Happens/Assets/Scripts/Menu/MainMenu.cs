using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public Canvas canvas;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenuItems;

    private void Awake()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
            canvas.enabled = false; 
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SettingsGame()
    {
        settingsMenu.gameObject.SetActive(true);
        mainMenuItems.gameObject.SetActive(false);
    }

    public void BackMainMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        mainMenuItems.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void Skip()
    {
        canvas.enabled = false;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

}
