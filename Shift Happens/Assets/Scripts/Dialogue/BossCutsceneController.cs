using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutsceneController : MonoBehaviour
{

    public LevelLoader levelLoader;
    public Animator playerAnimator;
    public Animator bossAnimator;
    public DialogueManager dialogueManager;
    public Dialogue dialogue1;

    int num = 0;
    bool go = true;
    string charName;

    public string bossName;


    public GameObject thingTodrop;

    public GameObject player;

    bool spawn = true;
    

    // Update is called once per frame
    void Update()
    {
        if (num == 0)
        {
            if (go)
            {
                dialogueManager.StartDialogue(dialogue1);
                go = false;
            }

            if (dialogueManager.DialogueOver())
            {
                num++;
                go = true;
            }

        }
        charName = dialogueManager.WhoIsTalking();

        if (num == 1)
        {
            bossAnimator.SetBool("die", true);
        }


        if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleOff") && spawn)
        { 
            thingTodrop.transform.position = new Vector3(0, 1, -6);
            Instantiate(thingTodrop);
            spawn = false;
            playerAnimator.SetBool("Walk", true);
        }


        if (player.transform.position.y == 1)
        {
            levelLoader.LoadNextLevel();
        }

    }
}

