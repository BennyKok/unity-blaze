using UnityEngine;

namespace Blaze.Dialogue
{
    public class BlazeDialogueSettings : ScriptableObject
    {
        public const string settingsAssetName = "BlazeDialogue Settings";
        public const string settingsPath = "Assets/Resources/" + settingsAssetName + ".asset";
        public string[] languageDefinition = { "English" };
    }
}