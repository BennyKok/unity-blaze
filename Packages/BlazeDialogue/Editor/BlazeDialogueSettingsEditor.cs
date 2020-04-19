using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_2018_4
using UnityEngine.Experimental.UIElements;
#elif UNITY_2019_1
using UnityEngine.UIElements;
#endif

// Create a new type of Settings Asset.
namespace Blaze.Dialogue.Editor
{
    public class BlazeDialogueSettingsEditor
    {
        internal static BlazeDialogueSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<BlazeDialogueSettings>(BlazeDialogueSettings.settingsPath);
            if (settings == null)
            {
                if (!Directory.Exists(BlazeDialogueSettings.settingsFolderPath))
                    Directory.CreateDirectory(BlazeDialogueSettings.settingsFolderPath);

                settings = ScriptableObject.CreateInstance<BlazeDialogueSettings>();
                AssetDatabase.CreateAsset(settings, BlazeDialogueSettings.settingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    static class SlimDialogueIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Blaze/Dialogue", SettingsScope.Project)
            {
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = BlazeDialogueSettingsEditor.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("languageDefinition"), new GUIContent("Languages"), true);
                    settings.ApplyModifiedProperties();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "Translation" })
            };

            return provider;
        }
    }
}