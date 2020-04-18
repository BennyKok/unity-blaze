using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Blaze.Property
{
    [CustomPropertyDrawer(typeof(StringProperty))]
    public class StringPropertyDrawer : TextBasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);

            tabs[2].contents.Add(
                new ItemContent(ItemType.GUI)
                {
                    enableIf = property.FindPropertyRelative("persistance"),
                    guiHeightCallback = () => EditorGUIUtility.singleLineHeight + verticalSpace,
                    guiDrawCallback = (rect) =>
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.TextField(rect, PlayerPrefs.GetString(property.FindPropertyRelative("key").stringValue));
                        EditorGUI.EndDisabledGroup();
                    }
                }
            );
        }
    }

}