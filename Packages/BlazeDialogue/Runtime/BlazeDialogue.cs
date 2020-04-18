using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    public class BlazeDialogue : MonoBehaviour
    {
        public Dialogue dialogue;
        
        [CollapsedEvent]
        public UnityEvent onFinished;

        public bool playOnAwake;

        private bool receivedAction;

        private void Start()
        {
            if (playOnAwake)
                StartDialogue();
        }

        public void StartDialogue()
        {
            if (!BlazeDialogueEvents.Instance)
            {
                Debug.LogError("BlazeDialogueEvents doesn't exist, the dialogue couldn't be played.");
                return;
            }
            BlazeDialogueEvents.Instance.currentFocusDialogue = this;
            StopCoroutine("StartDialogueCoroutine");
            StartCoroutine("StartDialogueCoroutine");
        }

        public void StopDialogue()
        {
            StopCoroutine("StartDialogueCoroutine");
        }

        public void TriggerAction()
        {
            receivedAction = true;
        }

        public IEnumerator StartDialogueCoroutine()
        {
            var events = BlazeDialogueEvents.Instance;

            //the dialog should be displayed
            events.onVisibilityChanged.Invoke(true);
            events.onDialogueShow.Invoke();
            foreach (var item in dialogue.contents)
            {
                events.onDialogueContent.Invoke(item.content);
                if (item.waitForAction)
                {
                    yield return new WaitUntil(() => receivedAction == true);
                    receivedAction = false;
                }
                else
                {
                    yield return new WaitForSeconds(item.duration);
                }
            }
            onFinished.Invoke();
            events.onVisibilityChanged.Invoke(false);
            events.onDialogueHide.Invoke();
        }
    }
}