using UnityEngine;
using System;
using UnityEngine.Events;
using Blaze.Property;

namespace Blaze.Dialogue
{
    [System.Serializable]
    public class Dialogue
    {
        public Item[] contents;

        [Serializable]
        public class Item
        {
            public MultiLineTranslatable content;
            public float duration;
            public AudioClip clip;
            public bool useClipDuration;
            public Actor actor;
            public bool waitForAction;
            public float chance;

            [CollapsedEvent]
            public UnityEvent onFinished;
        }
    }
}