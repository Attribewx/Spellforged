using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dummyText;


    public Animator anim;
    public GameObject UIBoy;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        UIBoy = GameObject.FindGameObjectWithTag("UIController");
        if(UIBoy != null)
            dummyText = UIBoy.GetComponentsInChildren<TMP_Text>()[0];
        if (UIBoy != null)
            nameText = UIBoy.GetComponentsInChildren<TMP_Text>()[1];
        if (UIBoy != null)
            anim = UIBoy.GetComponentInChildren<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        anim.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogue.frameLag);
    }

    public void DisplayNextSentence(int frames)
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //dummyText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence,frames));
    }

    IEnumerator TypeSentence (string sentence, int frameRate)
    {
        dummyText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dummyText.text += letter;
            for (int i = 0; i < frameRate; i++)
            {
                yield return null;
            }
        }
    }

    void EndDialogue()
    {
        Debug.Log("End Convo");
        anim.SetBool("IsOpen", false);
    }
}
