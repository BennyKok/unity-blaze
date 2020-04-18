using UnityEngine;
using UnityEditor;

namespace Blaze.Dialogue.Editor
{
    [CustomPropertyDrawer(typeof(MultiLineTranslatable))]
    public class MultiLineTranslatableEditor : TranslatableEditor
    {
        public override int GetLineCount()
        {
            return 2;
        }
    }
}