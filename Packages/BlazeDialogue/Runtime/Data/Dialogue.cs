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
            public float delay = 1;
            public AudioClip clip;
            public bool useClipDuration;
            public Actor actor;
            public bool waitForAction;
            [Range(0, 1)]
            public float chance = 1;

            [CollapsedEvent]
            public UnityEvent onFinished;
        }
    }
}