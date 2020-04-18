using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Blaze.Property.Editor
{
    public class BlazeDrawerUtil
    {
        public static float AnimSpeed = 2.5f;
        private static Stack<Color> cacheColors = new Stack<Color>();

        public static bool BeginFade(AnimBool anim)
        {
            cacheColors.Push(GUI.color);

            if ((double)anim.faded == 0.0)
                return false;
            if ((double)anim.faded == 1.0)
                return true;

            var c = GUI.color;
            c.a = anim.faded;
            GUI.color = c;

            if ((double)anim.faded != 0.0 && (double)anim.faded != 1.0)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    Event.current.Use();
                }

                GUI.FocusControl(null);
            }

            return (double)anim.faded != 0.0;
        }

        public static void EndFade()
        {
            GUI.color = cacheColors.Pop();
        }

        public static void RepaintInspector(SerializedObject BaseObject)
        {
            foreach (var item in ActiveEditorTracker.sharedTracker.activeEditors)
                if (item.serializedObject == BaseObject)
                { item.Repaint(); return; }
        }
    }
}