using UnityEngine;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    [AddComponentMenu("Blaze/Dialogue/Dialogue Events")]
    public class BlazeDialogueEvents : MonoBehaviour
    {
        [Header("Settings")]
        public bool useDefaultTypingEffect = true;
        public float typingEffectDelay = 0.05f;

        [Header("Dialogue")]
        [CollapsedEvent]
        public BoolEvent onVisibilityChanged;
        [CollapsedEvent]
        public UnityEvent onDialogueShow;
        [CollapsedEvent]
        public UnityEvent onDialogueHide;

        [Header("Action")]

        [CollapsedEvent]
        public UnityEvent onDialogueActionShow;
        [CollapsedEvent]
        public UnityEvent onDialogueActionHide;

        [CollapsedEvent]
        public UnityEvent onDialogueAction;

        [Header("Text")]

        [CollapsedEvent]
        public StringEvent onDialogueContent;

        [Header("Line")]

        [CollapsedEvent]
        public UnityEvent onDialogueLineBegin;

        [CollapsedEvent]
        public UnityEvent onDialogueLineEnd;

        [Header("Audio")]

        [CollapsedEvent]
        public AudioEvent onDialogueAudio;


        [System.NonSerialized]
        public BlazeDialogue currentFocusDialogue;

        public static BlazeDialogueEvents Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void TriggerAction()
        {
            if (currentFocusDialogue)
            {
                currentFocusDialogue.TriggerAction();
                onDialogueAction.Invoke();
            }
        }

    }

    [System.Serializable]
    public class AudioEvent : UnityEvent<AudioClip>
    {

    }
}
