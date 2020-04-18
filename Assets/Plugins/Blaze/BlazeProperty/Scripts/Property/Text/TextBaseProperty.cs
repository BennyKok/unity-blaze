using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blaze.Property
{
    [System.Serializable]
    public abstract class TextBaseProperty<T> : PersistanceProperty<T>
    {
        public TMPro.TextMeshProUGUI targetText;

        public string prefix;
        public string suffix;

        public override T Value { get => base.Value; set => base.Value = value; }

        public override void OnCreateResolver()
        {
            AddResolver<TMPro.TextMeshProUGUI>((label, state) =>
            {
                switch (state)
                {
                    case ResolveSate.Update:
                        targetText = label;
                        label.text = GetFinalTextDisplay();
                        break;
                    case ResolveSate.UnBind:
                        targetText = null;
                        label.text = null;
                        break;
                }
            });
        }

        public string GetFinalTextDisplay()
        {
            return prefix + GetTextForDisplay() + suffix;
        }

        public virtual string GetTextForDisplay()
        {
            return Value.ToString();
        }
    }

}