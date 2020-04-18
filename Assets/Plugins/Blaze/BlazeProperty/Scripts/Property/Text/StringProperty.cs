using UnityEngine;
using UnityEngine.Events;

namespace Blaze.Property
{
    [System.Serializable]
    public class StringProperty : TextBaseProperty<string>
    {
        public StringEvent valueChanged;

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                valueChanged.Invoke(value);
            }
        }

        public override string Load(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public override void Save(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }

    [System.Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }
}