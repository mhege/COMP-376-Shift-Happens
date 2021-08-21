using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController6 : MonoBehaviour
{

    public LevelLoader levelLoader;
    public Animator alienAnimator;
    public Animator robotAnimator;
    public DialogueManager dialogueManager;
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    public Dialogue dialogue3;
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


        if(num == 1)
        {
            alienAnimator.SetBool("Shift", true);
        }

        if(alienAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleOn") && num == 1)
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

        if(num == 2)
        {
            robotAnimator.SetBool("Shift", true);
        }

        if (robotAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleOn 1") && num == 2)
        {
            if (go)
            {
                dialogueManager.StartDialogue(dialogue3);
                go = false;
            }

            if (dialogueManager.DialogueOver())
            {
                num++;
                go = true;
            }
        }

        if (num == 3)
        {
            levelLoader.LevelComplete();
        }
    }
}
