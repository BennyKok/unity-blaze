using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Blaze.Property
{
    [System.Serializable]
    public abstract class BaseProperty<T> : Property
    {
        public GameObject target;

        private bool targetBound;

        //bool unBind
        public event Action<ResolveSate> onResolverUpdate;

        [System.NonSerialized]
        private Dictionary<GameObject, Action<ResolveSate>> handlerMap = new Dictionary<GameObject, Action<ResolveSate>>();

        [SerializeField]
        private T value;

        public virtual T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                if (onResolverUpdate != null)
                    onResolverUpdate.Invoke(ResolveSate.Update);

                if (!targetBound && target)
                {
                    targetBound = true;
                    BindListener(target);
                }
            }
        }

        internal void UpdateValueInternal(T value)
        {
            this.value = value;
        }

        public override bool BindListener(GameObject target, bool emitEvent = false)
        {
            if (target == null)
            {
                Debug.LogWarning("Target is null, nothing to bind");
                return false;
            }
            var handler = OnCreateListener(target);
            if (handler == null)
            {
                Debug.LogWarning("Failed at creating listener for (" + target.name + ")");
                return false;
            }

            handlerMap.Add(target, handler);
            onResolverUpdate += handler;

            //First invoke
            handler.Invoke(ResolveSate.Bind);
            handler.Invoke(ResolveSate.Update);

            if (emitEvent)
            {
                Value = Value;
            }

            return true;
        }

        public override void UnBindListener(GameObject target, bool emitEvent = false)
        {
            Action<ResolveSate> handler;
            handlerMap.TryGetValue(target, out handler);

            if (handler != null)
            {
                handler.Invoke(ResolveSate.UnBind);
                onResolverUpdate -= handler;
            }
            else
                Debug.LogWarning("Nothing to unbind from (" + target.name + ")");

            handlerMap.Remove(target);

            if (emitEvent)
            {
                Value = Value;
            }
        }

        public Action<ResolveSate> OnCreateListener(GameObject target)
        {
            Action<ResolveSate> handler = null;
            BeginResolvers(target);
            OnCreateResolver();

            if (resolvers.Count == 1)
            {
                handler = resolvers.FirstOrDefault().Value;
            }
            else if (resolvers.Count > 1)
            {
                var _resolvers = resolvers;
                handler = (state) =>
                {
                    foreach (KeyValuePair<Type, Action<ResolveSate>> entry in _resolvers)
                        entry.Value.Invoke(state);
                };
            }

            EndResolvers();
            return handler;
        }

        private Dictionary<Type, Action<ResolveSate>> resolvers;
        private GameObject tempTarget;

        public void BeginResolvers(GameObject target)
        {
            tempTarget = target;
            resolvers = new Dictionary<Type, Action<ResolveSate>>();
        }

        public void EndResolvers()
        {
            tempTarget = null;
            resolvers = null;
        }

        public void AddResolver<U>(Action<U, ResolveSate> onResolve) where U : Component
        {
            if (onResolve != null && !resolvers.ContainsKey(typeof(U)))
            {
                var component = tempTarget.GetComponent(typeof(U).Name) as U;
                if (component != null)
                {
                    resolvers.Add(typeof(U),
                    (state) =>
                    {
                        onResolve.Invoke(component, state);
                    }
                    );
                }
            }
        }

        public enum ResolveSate
        {
            Bind, UnBind, Update
        }

        public abstract void OnCreateResolver();

        public static implicit operator T(BaseProperty<T> prop)
        {
            return prop.Value;
        }

        public override bool CanBind()
        {
            return true;
        }
    }

}