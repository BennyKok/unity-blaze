using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blaze.Property
{
    [System.Serializable]
    public class IntProperty : NumericBaseProperty<int>
    {
        public IntEvent valueChanged;
        public Int2Event valueIncreased;
        public Int2Event valueDecreased;

        public override int Value
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

        public override int Load(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public override void Save(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    }

    [System.Serializable]
    public class IntEvent : UnityEvent<int>
    {
    }

    [System.Serializable]
    public class Int2Event : UnityEvent<int, int>
    {
    }
}