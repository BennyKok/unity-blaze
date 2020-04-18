using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Blaze.Property.Editor
{
    [CustomEditor(typeof(PropertyBindTarget))]
    public class PropertyBinderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var source = serializedObject.FindProperty("source");
            var bindChild = serializedObject.FindProperty("bindChild");
            var bindOnStart = serializedObject.FindProperty("bindOnStart");
            var sourceObject = serializedObject.FindProperty("sourceObject");
            var targetProperty = serializedObject.FindProperty("targetProperty");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sourceObject);
            EditorGUILayout.PropertyField(bindChild);
            EditorGUILayout.PropertyField(bindOnStart);
            if (EditorGUI.EndChangeCheck())
            {
                if (!sourceObject.objectReferenceValue)
                {
                    source.objectReferenceValue = null;
                }
            }

            if (sourceObject.objectReferenceValue && sourceObject.objectReferenceValue is GameObject)
            {
                var coms = (sourceObject.objectReferenceValue as GameObject).GetComponents<MonoBehaviour>();
                // Debug.Log(coms.Length);
                EditorGUI.BeginChangeCheck();
                var index = EditorGUILayout.Popup(0, coms.ToList().ConvertAll<string>(x => x.GetType().Name).ToArray());
                if (EditorGUI.EndChangeCheck() || !source.objectReferenceValue)
                {
                    source.objectReferenceValue = coms[index];
                }
            }
            else
            {
                source.objectReferenceValue = sourceObject.objectReferenceValue;
            }

            if (source.objectReferenceValue && sourceObject.objectReferenceValue is GameObject)
            {
                var targetObject = source.objectReferenceValue;
                Type myObjectType = targetObject.GetType();
                var matched = new List<FieldInfo>();
                var displayOptions = new List<string>();
                var selectedIndex = 0;
                int i = 0;
                foreach (var any in myObjectType.GetFields())
                {
                    try
                    {
                        var prop = any.GetValue(targetObject) as Property;
                        if (prop != null)
                        {
                            matched.Add(any);
                            displayOptions.Add(any.Name);

                            if (targetProperty.stringValue == any.Name)
                            {
                                selectedIndex = i;
                            }
                            i++;

                            // EditorGUILayout.LabelField(any.Name);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                EditorGUI.BeginChangeCheck();
                var index = EditorGUILayout.Popup(selectedIndex, displayOptions.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    targetProperty.stringValue = displayOptions[index];
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}