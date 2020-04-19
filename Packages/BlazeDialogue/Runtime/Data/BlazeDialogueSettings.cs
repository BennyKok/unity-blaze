using UnityEngine;

namespace Blaze.Dialogue
{
    public class BlazeDialogueSettings : ScriptableObject
    {
        public const string settingsAssetName = "BlazeDialogue Settings";

        public const string settingsFolderPath = "Assets/Blaze/Resources/";

        public const string settingsPath = settingsFolderPath + settingsAssetName + ".asset";
        public string[] languageDefinition = { "English" };
    }
}