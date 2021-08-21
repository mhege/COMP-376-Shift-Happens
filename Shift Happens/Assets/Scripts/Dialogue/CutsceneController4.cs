using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController4 : MonoBehaviour
{

    public LevelLoader levelLoader;
    public Animator playerAnimator;
    public DialogueManager dialogueManager;
    public Dialogue dialogue1;
    int num = 0;
    bool go = true;
    string charName; 
    // Start is called before the first frame update


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

       
        if (charName.StartsWith("Z"))
        {
            playerAnimator.SetBool("HumanToAlien", true);
            playerAnimator.SetBool("AlienToRobot", false);
            playerAnimator.SetBool("RobotToHuman", false);
        }
        else if (charName.StartsWith("X"))
        {
            playerAnimator.SetBool("HumanToAlien", false);
            playerAnimator.SetBool("AlienToRobot", true);
            playerAnimator.SetBool("RobotToHuman", false);
        }
        else if (charName.StartsWith("T"))
        {
            playerAnimator.SetBool("HumanToAlien", false);
            playerAnimator.SetBool("AlienToRobot", false);
            playerAnimator.SetBool("RobotToHuman", true);
        }

        if (num == 1)
        {
            playerAnimator.SetBool("Walk", true);    
        }


        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
        {
            levelLoader.LevelComplete();
        }
    }
}
