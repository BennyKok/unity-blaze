using UnityEngine;

namespace Blaze.Dialogue
{
    [System.Serializable]
    public class Translatable
    {
        public string[] contents = new string[] { "" };

        public override string ToString()
        {
            if (BlazeDialogueManager.Instance.selectedLang == -1)
            {
                //BlazeDialogue not ready
                if (contents.Length == 0)
                    return null;
                else
                    return contents[0];
            }

            if (BlazeDialogueManager.Instance.selectedLang > contents.Length - 1)
            {
                //We don't have such translation, falling back to default
                return contents[0];
            }

            return contents[BlazeDialogueManager.Instance.selectedLang];
        }

        public static implicit operator string(Translatable dialogueString)
        {
            return dialogueString.ToString();
        }


    }
}