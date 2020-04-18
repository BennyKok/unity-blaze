using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blaze.Property
{
    [System.Serializable]
    public abstract class PersistanceProperty<T> : BaseProperty<T>
    {
        public bool persistance;

        public string key;

        [System.NonSerialized]
        public bool loaded;

        public override T Value
        {
            get
            {
                if (persistance && !loaded)
                {
                    var saved = Load(key, base.Value);
                    loaded = true;
                    UpdateValueInternal(saved);
                    return saved;
                }
                return base.Value;
            }
            set
            {
                base.Value = value;
                if (persistance)
                {
                    Save(key, value);
                }
            }
        }

        public abstract void Save(string key, T value);

        public abstract T Load(string key, T defaultValue);
    }

}