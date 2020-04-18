using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blaze.Property
{
    [System.Serializable]
    public class BoolProperty : TextBaseProperty<bool>
    {
        public BoolEvent valueChanged;

        public string yes;
        public string no;

        public override bool Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                valueChanged.Invoke(base.Value);
            }
        }

        public override bool Load(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, (defaultValue ? 1 : 0)) != 0;
        }

        public override void Save(string key, bool value)
        {
            PlayerPrefs.SetInt(key, (value ? 1 : 0));
        }

        public override string GetTextForDisplay()
        {
            return Value ? yes : no;
        }
        

    }

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool>
    {
    }
}