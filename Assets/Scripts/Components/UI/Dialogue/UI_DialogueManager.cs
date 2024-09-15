using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void StartDialogue(ItemDialogue dialogue)
        {
            if (!inUse)
            {
                inUse = true;
                //Set name text to display. 
                nameText.text = dialogue.itemName;

                //Start the Coroutine.
                StartCoroutine(DisplayFlavourText(dialogue));
            }
        }

        private IEnumerator DisplayFlavourText(ItemDialogue dialogue)
        {
            //Enable display.
            ClearText();
            displayFader.FadeIn();
            textFader.FadeIn();

            //Set the text. 
            nameText.text = dialogue.itemName;
            dialogueText.text = dialogue.itemFlavourText.ToString();

            while (textFader.state != FadeState.IN && displayFader.state != FadeState.IN)
                yield return new WaitForEndOfFrame();

            //Delay for reading time.
            yield return new WaitForSeconds(1F);

            //Fade transition out. 
            displayFader.FadeOut();
            textFader.FadeOut();
            while (textFader.state != FadeState.OUT && displayFader.state != FadeState.OUT)
                yield return new WaitForEndOfFrame();

            //Clear set text. 
            ClearText();

            //Set to not be in use.
            inUse = false;
        }

        private void ClearText()
        {
            nameText.text = "";
            dialogueText.text = "";
        }
    }
}