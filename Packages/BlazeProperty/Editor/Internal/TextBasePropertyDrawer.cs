using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using System;

namespace Blaze.Property.Editor
{
    public class TextBasePropertyDrawer : BasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);

            var tab1 = new PropertyTab();
            tab1.icon = EditorGUIUtility.IconContent("Font Icon");
            tab1.icon.text = "UI";
            tab1.contents = new List<ItemContent>()
            {
                new ItemContent(property, ItemType.HeaderButton, "UI Binding","Find",()=>{
                    var attempt = GameObject.Find(property.name);

                    if (attempt)
                    {
                        tab1.contents[1].property.objectReferenceValue = attempt;
                        Debug.Log("Bind (" + property.name + ")");
                    }
                    else
                    {
                        Debug.LogWarning("Couldn't find any matched UI for (" + property.name + ")");
                    }
                }){
                    enableCallback = ()=> !isEditingScriptableObject(property)
                },
                new ItemContent(property, ItemType.Property, "target"){
                    enableCallback = ()=> !isEditingScriptableObject(property)
                },

                new ItemContent(property, ItemType.Header, "Extra"),
                new ItemContent(property, ItemType.Property, "prefix"),
                new ItemContent(property, ItemType.Property, "suffix")
            };
            tabs.Add(tab1);

            tabs.Add(new PropertyTab()
            {
                icon = new GUIContent("Events"),//EditorGUIUtility.IconContent("EventSystem Icon"),
                contents = new List<ItemContent>()
                {
                    new ItemContent(property, ItemType.Header, "Event"),
                    new ItemContent(property, ItemType.Property, "valueChanged")
                }
            });

            var tab3 = new PropertyTab();
            tab3.icon = new GUIContent("Save");//EditorGUIUtility.IconContent("_Popup");
            tab3.contents = new List<ItemContent>()
            {
                new ItemContent(property, ItemType.HeaderButton, "Persistance","Auto",()=>{
                    tab3.contents[1].property.boolValue = true;

                    var name = property.serializedObject.targetObject.name;
                    tab3.contents[2].property.stringValue = ToCamelCase(name) + "." + property.propertyPath;
                    // ToCamelCase(name) + property.displayName.Replace(" ","");
                }),
                new ItemContent(property, ItemType.Property, "persistance"),
                new ItemContent(property, ItemType.Property, "key"){
                    enableIf = property.FindPropertyRelative("persistance"),
                },
                new ItemContent(property, ItemType.HeaderButton, "Player Prefs", "Reset", () =>
                {
                    PlayerPrefs.DeleteKey(property.FindPropertyRelative("key").stringValue);
                })
                {
                    enableIf = property.FindPropertyRelative("persistance"),
                }
            };
            tabs.Add(tab3);
        }

        public static string ToCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1).Replace(" ", "");
            }
            return str;
        }
    }

}