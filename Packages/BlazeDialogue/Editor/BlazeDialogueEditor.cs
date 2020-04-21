using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;

namespace Blaze.Dialogue.Editor
{
    [CustomEditor(typeof(BlazeDialogue))]
    public class BlazeDialogueEditor : UnityEditor.Editor
    {
        private TreeViewState m_TreeViewState;
        private DialogueTreeView m_TreeView;

        private SerializedProperty contentsProp;

        public override void OnInspectorGUI()
        {
            if (contentsProp == null)
                contentsProp = serializedObject.FindProperty("dialogue").FindPropertyRelative("contents");

            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            if (m_TreeView == null)
                m_TreeView = new DialogueTreeView(m_TreeViewState, contentsProp);

            serializedObject.Update();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Trigger", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerType"));
            if (serializedObject.FindProperty("triggerType").enumValueIndex > 1)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("checkObjectTag"));
                if (serializedObject.FindProperty("checkObjectTag").boolValue)
                {
                    var tagProperty = serializedObject.FindProperty("targetObjectTag");
                    EditorGUI.BeginChangeCheck();
                    var temp = EditorGUILayout.TagField(tagProperty.stringValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        tagProperty.stringValue = temp;
                    }
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelDialogueOnExit"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("targetEvents"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onFinished"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Dialogue", EditorStyles.boldLabel);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var space = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(120));
                if (!m_TreeView.HasSelection() && contentsProp.arraySize > 0)
                    m_TreeView.SetSelection(new int[] { 0 });
                m_TreeView.OnGUI(space);
            }

            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add", EditorStyles.miniButtonLeft, GUILayout.Width(50)))
                {
                    AddNewItem(false);
                }
                if (GUILayout.Button("Insert", EditorStyles.miniButtonMid, GUILayout.Width(50)))
                {
                    AddNewItem(true);
                }
                EditorGUI.BeginDisabledGroup(!m_TreeView.HasSelection() || contentsProp.arraySize == 0 || m_TreeView.GetSelection()[0] >= contentsProp.arraySize);
                if (GUILayout.Button("Remove", EditorStyles.miniButtonRight, GUILayout.Width(58)))
                {
                    contentsProp.DeleteArrayElementAtIndex(m_TreeView.GetSelection()[0]);
                    if (m_TreeView.GetSelection()[0] >= contentsProp.arraySize)
                        m_TreeView.SetSelection(new int[] { contentsProp.arraySize - 1 });
                    m_TreeView.Reload();
                }
                EditorGUI.EndDisabledGroup();
            }

            if (m_TreeView.HasSelection() && m_TreeView.GetSelection()[0] < contentsProp.arraySize && contentsProp.arraySize > 0)
            {
                EditorGUI.BeginChangeCheck();
                var item = contentsProp.GetArrayElementAtIndex(m_TreeView.GetSelection()[0]);
                EditorGUILayout.PropertyField(item.FindPropertyRelative("content"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("clip"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("waitForAction"));
                if (!item.FindPropertyRelative("waitForAction").boolValue)
                {
                    // EditorGUILayout.PropertyField(item.FindPropertyRelative("useClipDuration"));
                    // if (!item.FindPropertyRelative("useClipDuration").boolValue)
                        EditorGUILayout.PropertyField(item.FindPropertyRelative("delay"));
                }
                EditorGUILayout.PropertyField(item.FindPropertyRelative("actor"));
                EditorGUILayout.PropertyField(item.FindPropertyRelative("chance"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_TreeView.Reload();
                }
            }

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        public void AddNewItem(bool insert)
        {
            var index = contentsProp.arraySize;

            if (insert && m_TreeView.HasSelection() && m_TreeView.GetSelection()[0] < contentsProp.arraySize)
            {
                index = m_TreeView.GetSelection()[0];
            }

            contentsProp.InsertArrayElementAtIndex(index);
            var array = contentsProp.GetArrayElementAtIndex(index).FindPropertyRelative("content").FindPropertyRelative("contents");
            array.ClearArray();
            array.arraySize = BlazeDialogueSettingsEditor.GetOrCreateSettings().languageDefinition.Length;

            m_TreeView.SetSelection(new int[] { index });

            m_TreeView.Reload();
        }
    }

    public class DialogueTreeView : TreeView
    {
        SerializedProperty m_Property;

        public Rect position;

        public DialogueTreeView(TreeViewState treeViewState, SerializedProperty property)
            : base(treeViewState)
        {
            useScrollView = true;
            // showBorder = true;
            // showAlternatingRowBackgrounds = true;

            m_Property = property;

            Reload();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var prop = m_Property.GetArrayElementAtIndex(args.item.id);
            args.rowRect.x += position.x;
            args.rowRect.y += position.y;
            // EditorGUI.PropertyField(args.rowRect, prop, GUIContent.none);

            if (string.IsNullOrEmpty(args.item.displayName))
                EditorGUI.LabelField(args.rowRect, "--Empty--", args.selected ? EditorStyles.whiteMiniLabel : EditorStyles.miniLabel);
            else
                EditorGUI.LabelField(args.rowRect, args.item.displayName, args.selected ? EditorStyles.whiteLabel : EditorStyles.label);
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };
            var allItems = new List<TreeViewItem>();

            for (int i = 0; i < m_Property.arraySize; i++)
            {
                allItems.Add(
                    new TreeViewItem
                    {
                        id = i,
                        depth = 0,
                        displayName = m_Property.GetArrayElementAtIndex(i)
                        .FindPropertyRelative("content")
                        .FindPropertyRelative("contents")
                        .GetArrayElementAtIndex(0).stringValue
                    }
                );
            }

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
            return root;
        }
    }
}
