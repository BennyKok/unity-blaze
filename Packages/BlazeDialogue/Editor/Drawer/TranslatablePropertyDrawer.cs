using UnityEngine;
using UnityEditor;
using Blaze.Property.Editor;
using System.Collections.Generic;

namespace Blaze.Dialogue.Editor
{
    [CustomPropertyDrawer(typeof(TranslatableProperty))]
    public class TranslatablePropertyDrawer : TextBasePropertyDrawer
    {
        public override void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            base.InitTab(property, tabs);

            //Remove the two tabs
            tabs.RemoveRange(1, 2);
        }
    }
}