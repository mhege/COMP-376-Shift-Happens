using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public Animator wave;
    public Animator ship;
    public LevelLoader levelLoader;
    public DialogueManager dialogueManager;
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    int num = 0;
    bool go = true;
    bool playedWave;

    void Start()
    {
        playedWave = false;
    }

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

        if (num == 1)
        {
            wave.SetBool("Start", true);
        }

        if (wave.GetCurrentAnimatorStateInfo(0).IsName("Wave"))
        {
            if(gameObject.GetComponent<AudioSource>() && !playedWave)
            {
                gameObject.GetComponent<AudioSource>().Play();
                playedWave = true;
            }
            ship.SetBool("Crashing", true);
           
        }

        if (wave.GetCurrentAnimatorStateInfo(0).IsName("WaveIdle2"))
        {
            if (go)
            {
                dialogueManager.StartDialogue(dialogue2);
                go = false;
            }
            if (dialogueManager.DialogueOver())
            {
                ship.SetBool("Crash", true);
            }

        }

        if (ship.GetCurrentAnimatorStateInfo(0).IsName("Off"))
        {
            levelLoader.LevelComplete();
        }
       
    }
}
