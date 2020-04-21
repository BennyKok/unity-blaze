using UnityEngine;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    [AddComponentMenu("Blaze/Dialogue/Dialogue Events")]
    public class BlazeDialogueEvents : MonoBehaviour
    {
        [Header("Settings")]
        public bool isGlobalEvents = true;
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


        [Header("Actor")]

        [CollapsedEvent]
        public StringEvent onActorName;
        [CollapsedEvent]
        public SpriteEvent onActorIcon;


        [System.NonSerialized]
        public BlazeDialogue currentFocusDialogue;

        public static BlazeDialogueEvents Instance;

        private void Awake()
        {
            if (isGlobalEvents)
            {
                if (Instance != null)
                    Debug.LogWarning("Already has 1 BlazeDialogueEvents marked as isGlobalEvents");
                else
                    Instance = this;
            }
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
