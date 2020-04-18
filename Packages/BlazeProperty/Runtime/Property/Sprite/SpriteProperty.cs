using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Blaze.Property
{
    [System.Serializable]
    public class SpriteProperty : BaseProperty<Sprite>
    {
        public SpriteEvent valueChanged;

        public override Sprite Value
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

        public override void OnCreateResolver()
        {
            AddResolver<SpriteRenderer>((renderer, state) =>
            {
                switch (state)
                {
                    case ResolveSate.Update:
                        renderer.sprite = Value;
                        break;
                    case ResolveSate.UnBind:
                        renderer.sprite = null;
                        break;
                }
            });

            AddResolver<Image>((renderer, state) =>
            {
                switch (state)
                {
                    case ResolveSate.Update:
                        renderer.sprite = Value;
                        break;
                    case ResolveSate.UnBind:
                        renderer.sprite = null;
                        break;
                }
            });
        }
    }

    [System.Serializable]
    public class SpriteEvent : UnityEvent<Sprite>
    {
    }
}