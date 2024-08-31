using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Purrcifer.UI
{
    public class DialogueManager : MonoBehaviour
    {
        private bool inUse = false; 
        private bool _displayStateChangedImage = false; 
        private bool _displayStateChangedText = false; 
        public UIImageFader displayFader;
        public UITextFader textFader;
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
            textFader.fadeOpComplete += FadeOpCompleteText;
            displayFader.fadeOpComplete += FadeOpCompleteImage;
            displayFader.FadeIn();
            textFader.FadeIn();

            while (!_displayStateChangedImage && !_displayStateChangedText)
                yield return new WaitForEndOfFrame();

            _displayStateChangedImage = _displayStateChangedText = false;

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

            displayFader.FadeIn();
            textFader.FadeIn();
            while (!_displayStateChangedImage && !_displayStateChangedText)
                yield return new WaitForEndOfFrame();

            textFader.fadeOpComplete -= FadeOpCompleteText;
            displayFader.fadeOpComplete -= FadeOpCompleteImage;
            inUse = false;
        }

        private void OnDisable()
        {
            textFader.fadeOpComplete -= FadeOpCompleteText;
            displayFader.fadeOpComplete -= FadeOpCompleteImage;
        }

        public void FadeOpCompleteText() => _displayStateChangedImage = true; 

        public void FadeOpCompleteImage() => _displayStateChangedImage = true;
    }
}