using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Blaze.Property
{
    [CustomPropertyDrawer(typeof(IntProperty))]
    public class IntPropertyDrawer : TextBasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);

            tabs[1].contents.Add(new ItemContent(property, ItemType.Property, "valueIncreased"));
            tabs[1].contents.Add(new ItemContent(property, ItemType.Property, "valueDecreased"));


            tabs[2].contents.Add(
                new ItemContent(ItemType.GUI)
                {
                    enableIf = property.FindPropertyRelative("persistance"),
                    guiHeightCallback = () => EditorGUIUtility.singleLineHeight + verticalSpace,
                    guiDrawCallback = (rect) =>
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.TextField(rect, PlayerPrefs.GetInt(property.FindPropertyRelative("key").stringValue).ToString());
                        EditorGUI.EndDisabledGroup();
                    }
                }
            );
        }
    }

}