using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public Image image, image1, image2, image3, image4;
   
    public Text nameText;
    public Text dialogueText;
    Queue<string> sentences;
    Queue<string> names;
    string currentName; 
    bool end = false;

    public bool hasBoss;
    public string bossName;

    private void Start()
    {
        image.sprite = image1.sprite;
        currentName = "Tiberius";
    }


    public void StartDialogue (Dialogue dialogue)
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        animator.SetBool("IsOpen", true);
        end = false; 

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }
       DisplayNextSentence();
    }

    IEnumerator TypeSentence (string sentence)
    {
        if(gameObject.GetComponent<AudioSource>())
            gameObject.GetComponent<AudioSource>().Play();

        dialogueText.text = " ";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        if (gameObject.GetComponent<AudioSource>())
            gameObject.GetComponent<AudioSource>().Stop();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
       
        string name = names.Dequeue();
        string sentence = sentences.Dequeue();
        nameText.text = name;
        currentName = name;

        if (name == "Tiberius") 
            image.sprite = image1.sprite;
        else if (name.StartsWith("Z"))
            image.sprite = image2.sprite;
        else if (name.StartsWith("X"))
            image.sprite = image3.sprite;
        else if (hasBoss && name == bossName)
            image.sprite = image4.sprite;


        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }


    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        end = true;
    }


    public string WhoIsTalking()
    {
        return currentName;
    }


    public bool DialogueOver()
    {
        return end;
    }
}
