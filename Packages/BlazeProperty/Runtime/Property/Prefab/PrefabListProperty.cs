using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaze.Property
{
    [System.Serializable]
    public class PrefabListProperty : BaseProperty<GameObject>
    {
        public void Create(object[] data, bool autoBind, Action<int, GameObject> callback)
        {
            Clear();
            for (int i = 0; i < data.Length; i++)
            {
                var o = Add();
                if (autoBind)
                {
                    o.DataBindChildAuto(data[i]);
                }
                if (callback != null)
                    callback.Invoke(i, o);
            }
        }

        public GameObject Add()
        {
            var o = GameObject.Instantiate(Value);
            o.transform.SetParent(target.transform);
            o.transform.localScale = Vector3.one;
            o.transform.localPosition = Vector3.zero;
            o.transform.localEulerAngles = Vector3.zero;

            return o;
        }

        public void Clear()
        {
            foreach (RectTransform child in target.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public override bool CanBind()
        {
            return false;
        }

        public override void OnCreateResolver()
        {

        }

    }
}