using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/**
 * This class allow you to create a dialogue between NPC, CutScene or Tutorial 
 */
public class DialogueSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textBox = null;

    [Header("Text")]
    [SerializeField] private string[] sentences = null;
    [SerializeField] private float typeSpeed = 0.05f;//Lower = faster
    [SerializeField] private float readTime = 2f;

    private Coroutine ongoingCoroutine = null;
    private float waitTime = 0f;
    private string originalText = "";

    private void Awake()
    {
        waitTime = typeSpeed;
        originalText = textBox.text;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ongoingCoroutine != null)
            StopCoroutine(ongoingCoroutine);
        ongoingCoroutine = StartCoroutine(StartDialogue());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine(ongoingCoroutine);
        ongoingCoroutine = null;
        textBox.text = originalText;
    }

    private IEnumerator StartDialogue()
    {
        int sentenceIndex = 0;
        string sentence = sentences[sentenceIndex];

        while (sentenceIndex < sentences.Length)
        {
            textBox.text = "";
            int textIndex = 0;
            //Have not displayed all the text in tat sentences
            while(textBox.text.Length != sentences[sentenceIndex].Length)
            {
                textBox.text += sentences[sentenceIndex][textIndex++];
                yield return new WaitForSeconds(waitTime);
            }
            waitTime = typeSpeed;
            ++sentenceIndex;
            yield return new WaitForSeconds(readTime);
        }
    }

    /// Speeds up the typing speed 
    public void Continue()
    {
        waitTime = typeSpeed * 0.5f;
    }
}
