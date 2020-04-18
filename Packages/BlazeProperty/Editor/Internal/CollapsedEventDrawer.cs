using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

namespace Blaze.Property.Editor
{
    [CustomPropertyDrawer(typeof(CollapsedEventAttribute))]
    [CustomPropertyDrawer(typeof(IntEvent))]
    [CustomPropertyDrawer(typeof(Int2Event))]
    [CustomPropertyDrawer(typeof(StringEvent))]
    [CustomPropertyDrawer(typeof(FloatEvent))]
    [CustomPropertyDrawer(typeof(Float2Event))]
    [CustomPropertyDrawer(typeof(BoolEvent))]
    [CustomPropertyDrawer(typeof(SpriteEvent))]

    public class CollapsedEventDrawer : UnityEventDrawer
    {
        public AnimBool visible;

        public void InitAnimBool(SerializedProperty property)
        {
            if (visible == null)
            {
                visible = new AnimBool();
                visible.speed = BlazeDrawerUtil.AnimSpeed;
                visible.valueChanged.AddListener(() => { BlazeDrawerUtil.RepaintInspector(property.serializedObject); });
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitAnimBool(property);

            // EditorGUI.indentLevel--;
            // Debug.Log(position);

            // position = EditorGUI.IndentedRect(position);

            // Debug.Log(position);

            position.x += 4;
            position.width -= 8;

            position.height = EditorGUIUtility.singleLineHeight;
            var temp = new GUIContent(label);

            SerializedProperty persistentCalls = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
            if (persistentCalls != null)
                temp.text += " (" + persistentCalls.arraySize + ")";

            // visible.target = EditorGUI.Foldout(position, visible.value, temp, true);

#if UNITY_2019_1_OR_NEWER
            var hum = EditorGUIUtility.hierarchyMode;
            EditorGUIUtility.hierarchyMode = false;
            visible.target = EditorGUI.BeginFoldoutHeaderGroup(position, visible.target, temp);
            EditorGUIUtility.hierarchyMode = hum;
#else
            visible.target = EditorGUI.Foldout(position, visible.target, temp, true);
#endif
            if (BlazeDrawerUtil.BeginFade(visible))
            {
                label.text = null;
                position.height = base.GetPropertyHeight(property, label);
                position.y += EditorGUIUtility.singleLineHeight;
                base.OnGUI(position, property, label);
            }
            BlazeDrawerUtil.EndFade();
#if UNITY_2019_1_OR_NEWER
            EditorGUI.EndFoldoutHeaderGroup();
#endif
            // EditorGUI.indentLevel++;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitAnimBool(property);
            return base.GetPropertyHeight(property, label) * visible.faded + EditorGUIUtility.singleLineHeight;
        }

    }
}
