using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Blaze.Property
{
    [System.Serializable]
    public class FloatProperty : NumericBaseProperty<float>
    {
        public int roundDigit = 2;

        public FloatEvent valueChanged;
        public Float2Event valueIncreased;
        public Float2Event valueDecreased;

        public override float Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                var oldValue = base.Value;
                base.Value = value;

                valueChanged.Invoke(base.Value);

                if (value > oldValue)
                    valueIncreased.Invoke(value, value - oldValue);
                else
                    valueDecreased.Invoke(value, value - oldValue);
            }
        }

        public override void OnCreateResolver()
        {
            AddResolver<Image>((fillTarget, state) =>
            {
                switch (state)
                {
                    case ResolveSate.Update:
                        fillTarget.fillAmount = Value;
                        break;
                    case ResolveSate.UnBind:
                        fillTarget.fillAmount = 0;
                        break;
                }
            });

            base.OnCreateResolver();
        }


        public override string GetTextForDisplay()
        {
            return (roundDigit > -1 ? Value : (float)System.Math.Round(Value, roundDigit)).ToString();
        }

        public override float Load(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public override void Save(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

    }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float>
    {
    }

    [System.Serializable]
    public class Float2Event : UnityEvent<float, float>
    {
    }
}