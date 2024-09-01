using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Purrcifer.UI
{
    public class UI_DialogueManager : MonoBehaviour
    {
        private bool inUse = false; 
        public UI_ImageFader displayFader;
        public UI_TextFader textFader;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public AudioClip dialogueAudio;
        private Queue<string> sentences;

        // Use this for initialization
        void Start()
        {
            sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            if (!inUse)
            {
                inUse = true;
                //Set name text to display. 
                nameText.text = dialogue.itemName;

                //Queue all sentences within the dialogue. 
                foreach (string sentence in dialogue.itemFlavourText)
                    sentences.Enqueue(sentence);

                //Start the Coroutine.
                StartCoroutine(DisplayFlavourText(dialogue));
            }
        }

        private IEnumerator DisplayFlavourText(Dialogue dialogue)
        {
            string currentSentence;

            //Enable display.
            displayFader.FadeIn();
            textFader.FadeIn();

            while (textFader.state != FadeState.IN && displayFader.state != FadeState.IN)
                yield return new WaitForEndOfFrame();

            while (sentences.Count > 0)
            {
                //Get the next line of dialogue. 
                currentSentence = sentences.Dequeue();
                dialogueText.text = "";
                foreach (char letter in currentSentence.ToCharArray())
                {
                    dialogueText.text += letter;
                    yield return null;
                }
            }

            //Disable display.
            sentences.Clear();

            displayFader.FadeOut();
            textFader.FadeOut();
            while (textFader.state != FadeState.OUT && displayFader.state != FadeState.OUT)
                yield return new WaitForEndOfFrame();
            inUse = false;
        }
    }
}