using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Blaze.Property
{
    [CustomPropertyDrawer(typeof(SpriteProperty))]
    public class SpritePropertyDrawer : BasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);
            var tab1 = new PropertyTab();
            tab1.icon = new GUIContent("UI"); //EditorGUIUtility.IconContent("Font Icon");
            tab1.contents = new List<ItemContent>()
            {
                new ItemContent(property, ItemType.HeaderButton, "Binding","Find",()=>{
                    var attempt = GameObject.Find(property.displayName);

                    TMPro.TextMeshProUGUI temp;
                    if (attempt && (temp = attempt.GetComponent<TMPro.TextMeshProUGUI>()))
                    {
                        tab1.contents[1].property.objectReferenceValue = temp;
                    }
                    else
                    {
                        Debug.LogWarning("Couldn't find any matched SpriteRenderer for (" + property.displayName + ")");
                    }
                }){
                    enableCallback = ()=> !isEditingScriptableObject(property)
                },
                new ItemContent(property, ItemType.Property, "target"){
                    enableCallback = ()=> !isEditingScriptableObject(property)
                },
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
        }
    }

}