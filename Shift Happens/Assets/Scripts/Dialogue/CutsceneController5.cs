using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController5 : MonoBehaviour
{

    public LevelLoader levelLoader;
    public Animator humanAnimator;
    public Animator alienAnimator;
    public Animator robotAnimator;
    public Animator shipAnimator;

    public DialogueManager dialogueManager;
    public Dialogue dialogue1;
    public Dialogue dialogue2;
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

       

        if (num == 1)
        {
           shipAnimator.SetBool("Repair", true);    
        }

        if (shipAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShipIdle 1") && num == 1)
        {
            if (go)
            {
                dialogueManager.StartDialogue(dialogue2);
                go = false;
            }

            if (dialogueManager.DialogueOver())
            {
                num++;
                go = true;
            }
        }

        if (num == 2)
        {
            alienAnimator.SetBool("Walk", true);
        }

        if (alienAnimator.GetCurrentAnimatorStateInfo(0).IsName("AlienIn"))
        {
            humanAnimator.SetBool("Walk", true);
        }


        if (humanAnimator.GetCurrentAnimatorStateInfo(0).IsName("HumanIn"))
        {
            robotAnimator.SetBool("Walk", true);
        }

        if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("RobotIn"))
        {
            shipAnimator.SetBool("Fly", true);
        }

        if (shipAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShipIdle 2"))
        {
            levelLoader.LevelComplete();
        }

    }
}
