using System;
using UnityEngine;

namespace Blaze.Property
{
    public abstract class Property
    {
        public abstract bool CanBind();
        public abstract bool BindListener(GameObject target, bool emitEvent = false);
        public abstract void UnBindListener(GameObject target, bool emitEvent = false);

    }
}