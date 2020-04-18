using UnityEngine;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    public class BlazeDialogueEvents : MonoBehaviour
    {
        [Header("Events")]
        [CollapsedEvent]
        public BoolEvent onVisibilityChanged;
        [CollapsedEvent]
        public UnityEvent onDialogueShow;
        [CollapsedEvent]
        public UnityEvent onDialogueHide;
        [CollapsedEvent]
        public StringEvent onDialogueContent;

        [System.NonSerialized]
        public BlazeDialogue currentFocusDialogue;

        public static BlazeDialogueEvents Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void TriggerAction()
        {
            if (currentFocusDialogue){
                currentFocusDialogue.TriggerAction();
            }
        }
    }
}
