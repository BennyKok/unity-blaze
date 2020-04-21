using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    [AddComponentMenu("Blaze/Dialogue/Dialogue")]
    public class BlazeDialogue : MonoBehaviour
    {
        public enum DialogueTriggerType
        {
            None, OnStart, OnTrigger, OnCollision
        }

        public BlazeDialogueEvents targetEvents;

        public Dialogue dialogue;

        public DialogueTriggerType triggerType;


        public bool cancelDialogueOnExit;

        public bool checkObjectTag;

        public string targetObjectTag;

        [CollapsedEvent]
        public UnityEvent onFinished;

        private bool receivedAction;

        private BlazeDialogueEvents events;

        public void StartDialogue()
        {
            if (targetEvents)
            {
                events = targetEvents;
            }
            else
            {
                events = BlazeDialogueEvents.Instance;
            }

            if (!events)
            {
                Debug.LogError("BlazeDialogueEvents doesn't exist, the dialogue couldn't be played.");
                return;
            }
            events.currentFocusDialogue = this;
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
            //the dialog should be displayed
            events.onVisibilityChanged.Invoke(true);
            events.onDialogueShow.Invoke();
            foreach (var item in dialogue.contents)
            {
                //Out of luck, we skip this line
                if (Random.value > item.chance)
                    continue;

                events.onDialogueLineBegin.Invoke();

                if (item.actor)
                {
                    events.onActorName.Invoke(item.actor.actorName);
                    events.onActorIcon.Invoke(item.actor.icon);
                }

                if (item.clip)
                    events.onDialogueAudio.Invoke(item.clip);
                if (events.useDefaultTypingEffect)
                {
                    var time = Time.time;
                    yield return StartCoroutine(TypeText(item.content));
                    var timeUsedForTyping = Time.time - time;
                }
                else
                {
                    events.onDialogueContent.Invoke(item.content);
                }
                events.onDialogueLineEnd.Invoke();

                if (item.waitForAction)
                {
                    events.onDialogueActionShow.Invoke();
                    yield return new WaitUntil(() => receivedAction == true);
                    receivedAction = false;
                    events.onDialogueActionHide.Invoke();
                }
                else
                {
                    // if (item.delay - timeUsedForTyping > 0)
                    yield return new WaitForSeconds(item.delay);
                }
            }
            onFinished.Invoke();
            events.onVisibilityChanged.Invoke(false);
            events.onDialogueHide.Invoke();
        }

        public IEnumerator TypeText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                yield return new WaitForSeconds(events.typingEffectDelay);
                if (text.Substring(i, 1) == " ")
                {
                    continue;
                }
                events.onDialogueContent.Invoke(text.Substring(0, i + 1));
            }
        }

        /////////Lifecycle hook

        private void Start()
        {
            if (triggerType == DialogueTriggerType.OnStart)
                StartDialogue();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerType == DialogueTriggerType.OnTrigger && MatchTag(other.gameObject))
                StartDialogue();
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerType == DialogueTriggerType.OnTrigger && MatchTag(other.gameObject))
                StopDialogue();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggerType == DialogueTriggerType.OnTrigger && MatchTag(other.gameObject))
                StartDialogue();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (triggerType == DialogueTriggerType.OnTrigger && MatchTag(other.gameObject))
                StopDialogue();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (triggerType == DialogueTriggerType.OnCollision && MatchTag(other.gameObject))
                StartDialogue();
        }

        private void OnCollisionExit(Collision other)
        {
            if (triggerType == DialogueTriggerType.OnCollision && MatchTag(other.gameObject))
                StopDialogue();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (triggerType == DialogueTriggerType.OnCollision && MatchTag(other.gameObject))
                StartDialogue();
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (triggerType == DialogueTriggerType.OnCollision && MatchTag(other.gameObject))
                StopDialogue();
        }

        public bool MatchTag(GameObject other)
        {
            if (!checkObjectTag)
                return true;

            return other.CompareTag(targetObjectTag);
        }
    }
}