using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using System;

namespace Blaze.Property.Editor
{
    [CustomPropertyDrawer(typeof(PrefabListProperty))]
    
    public class PrefabListPropertyDrawer : BasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);

            var tab1 = new PropertyTab();
            tab1.icon = new GUIContent("UI");//EditorGUIUtility.IconContent("Font Icon");
            tab1.contents = new List<ItemContent>()
            {
                new ItemContent(property, ItemType.Header, "UI Binding"){
                    enableCallback = ()=> !isEditingScriptableObject(property)
                },
                new ItemContent(property,ItemType.Property,"target")
            };
            tabs.Add(tab1);
        }
    }

}