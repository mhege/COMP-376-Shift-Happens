using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text text;
    public Animator tut;


    [TextArea(3, 10)]
    public string[] tutorialInfo;

    int index = 0;
    public bool next = false;

    // Start is called before the first frame update
    void Start()
    {
        text.text = tutorialInfo[index];
     
    }

    // Update is called once per frame
    void Update()
    {
     
        if (next)
        {
            tut.SetBool("NextRoom", true);
            next = false;
            index++;
        }

        if (tut.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            text.text = tutorialInfo[index];
            tut.SetBool("NextRoom", false);
            tut.SetBool("Start", true);
        }

      
    }


    public void Next()
    {
        next = true;
    }
}
