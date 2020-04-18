using UnityEngine;
using UnityEditor;

namespace Blaze.Dialogue.Editor
{
    [CustomPropertyDrawer(typeof(Translatable))]
    public class TranslatableEditor : PropertyDrawer
    {
        private bool showTranslation;

        private int languageCount;

        public int lineCount = 1;

        private int itemSpacing = 2;

        public virtual int GetLineCount()
        {
            return lineCount;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            languageCount = BlazeDialogueSettingsEditor.GetOrCreateSettings().languageDefinition.Length;

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label, EditorStyles.miniBoldLabel);

            if (GUI.Button(new Rect(position.width - 15, position.y + EditorGUIUtility.singleLineHeight, 30, EditorGUIUtility.singleLineHeight * GetLineCount()), EditorGUIUtility.IconContent("_Popup")))
            {
                showTranslation = !showTranslation;
            }

            var tempRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width - 30, EditorGUIUtility.singleLineHeight * GetLineCount());
            var contents = property.FindPropertyRelative("contents");

            if (contents.arraySize != languageCount)
                contents.arraySize = languageCount;

            for (int i = 0; i < contents.arraySize; i++)
            {
                EditorGUI.PropertyField(tempRect, contents.GetArrayElementAtIndex(i), GUIContent.none);
                tempRect.y += (EditorGUIUtility.singleLineHeight + itemSpacing) * GetLineCount();

                if (!showTranslation && i == 0)
                {
                    //only draw the first item if we don't show the translation
                    break;
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var h = EditorGUIUtility.singleLineHeight;

            var size = property.FindPropertyRelative("contents").arraySize;

            h += !showTranslation && size > 1 ? EditorGUIUtility.singleLineHeight * GetLineCount() + itemSpacing * 2:
            size * (EditorGUIUtility.singleLineHeight + itemSpacing) * GetLineCount();

            return h;
        }
    }
}