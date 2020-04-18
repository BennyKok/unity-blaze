using UnityEngine;

namespace Blaze.Dialogue
{
    [System.Serializable]
    public class Translatable
    {
        public string[] contents = new string[] { "" };

        public static implicit operator string(Translatable dialogueString)
        {
            if (BlazeDialogueManager.Instance.selectedLang == -1)
            {
                //BlazeDialogue not ready
                if (dialogueString.contents.Length == 0)
                    return null;
                else
                    return dialogueString.contents[0];
            }

            if (BlazeDialogueManager.Instance.selectedLang > dialogueString.contents.Length - 1)
            {
                //We don't have such translation, falling back to default
                return dialogueString.contents[0];
            }

            return dialogueString.contents[BlazeDialogueManager.Instance.selectedLang];
        }
    }
}