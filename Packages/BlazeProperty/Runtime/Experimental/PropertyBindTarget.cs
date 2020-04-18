using Blaze.Property;
using UnityEngine;

public class PropertyBindTarget : MonoBehaviour
{
    public Object source;
    public Object sourceObject;

    public bool bindChild;
    public bool bindOnStart;

    public string targetProperty;

    public string[] specificFields;

    private DataBindResult dataBindResult;

    private void Awake()
    {
        if (source && !bindOnStart)
        {
            Bind(source, bindChild, targetProperty, specificFields);
        }
    }

    private void Start()
    {
        if (source && bindOnStart)
        {
            Bind(source, bindChild, targetProperty, specificFields);
        }
    }

    public void Bind(object data, bool bindChild, string targetProperty, string[] specificFields)
    {
        if (dataBindResult != null)
        {
            dataBindResult.UnBind();
        }

        if (bindChild)
        {
            dataBindResult = gameObject.DataBindChild(data, specificFields);
        }
        else
        {
            dataBindResult = gameObject.DataBindSelf(data, targetProperty);
        }

        if (dataBindResult == null)
            Debug.LogWarning("Bind result return null for (" + name + ")");
    }

    private void OnDestroy()
    {
        if (dataBindResult != null)
        {
            dataBindResult.UnBind();
        }
    }
}