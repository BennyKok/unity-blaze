using System;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Blaze.Property
{
    //Binding from GameObject target to Property
    public class DataBindResult
    {
        public Dictionary<Property, GameObject> properties = new Dictionary<Property, GameObject>();

        public void UnBind()
        {
            foreach (KeyValuePair<Property, GameObject> entry in properties)
            {
                Debug.Log("UnBinding from (" + entry.Value.name + ")");
                entry.Key.UnBindListener(entry.Value);
            }
        }
    }

    public static class PropertyDataBind
    {
        public static Dictionary<Type, FieldInfo[]> reflectionCache = new Dictionary<Type, FieldInfo[]>();

        public static void DataBindChildAuto(this GameObject target, object data, params string[] specificFields)
        {
            var auto = target.AddComponent<PropertyBindTarget>();
            auto.Bind(data, true, null, specificFields);
        }

        public static void DataBindSelfAuto(this GameObject target, object data, string targetField)
        {
            var auto = target.AddComponent<PropertyBindTarget>();
            auto.Bind(data, false, targetField, null);
        }

        public static DataBindResult DataBindChild(this GameObject target, object data, params string[] specificFields)
        {
            return DataBind(target, true, data, specificFields);
        }

        public static DataBindResult DataBindSelf(this GameObject target, object data, string targetField)
        {
            if (string.IsNullOrWhiteSpace(targetField))
            {
                return null;
            }
            return DataBind(target, false, data, targetField);
        }

        private static DataBindResult DataBind(GameObject target, bool bindChild, object data, params string[] specificFields)
        {
            FieldInfo[] allFields;
            if (!reflectionCache.TryGetValue(data.GetType(), out allFields))
            {
                UnityEngine.Debug.Log("Reflecting for " + data.GetType());
                // Stopwatch stopwatch = Stopwatch.StartNew();
                allFields = data.GetType().GetFields();
                // UnityEngine.Debug.Log("Reflecting time " + stopwatch.ElapsedMilliseconds);
                reflectionCache.Add(data.GetType(), allFields);
            }

            var result = new DataBindResult();

            // Debug.Log("Total Field " + allFields.Length);
            foreach (var field in allFields)
            {
                if (specificFields.Length > 0)
                {
                    var has = false;
                    foreach (var checkField in specificFields)
                    {
                        if (field.Name == checkField)
                        {
                            has = true;
                            break;
                        }
                    }

                    if (!has)
                        continue;
                }
                // Debug.Log("Field " + field.Name);
                // Debug.Log("Field " + ToProperCase(field.Name));
                // Debug.Log("Field " + field.FieldType);
                // Debug.Log("Field " + field.FieldType.BaseType);
                // var ob = field.GetValue(data);
                // Debug.Log("Field " + ob);

                // Stopwatch stopwatch = Stopwatch.StartNew();
                var reflect = field.GetValue(data) as Property;
                // UnityEngine.Debug.Log("Get value time " + stopwatch.ElapsedMilliseconds);

                if (reflect != null && reflect.CanBind())
                {
                    if (bindChild)
                    {
                        var name = field.Name; // ToProperCase(field.Name);
                        var attempt = target.transform.FindDeepChild(name);

                        if (attempt != null)
                        {
                            if (reflect.BindListener(attempt.gameObject))
                            {
                                UnityEngine.Debug.Log("DataBind bind target (" + name + ") in " + target.name);
                                result.properties.Add(reflect, attempt.gameObject);
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("DataBind can't find target (" + name + ") in " + target.name);
                        }
                    }
                    else
                    {
                        // Debug.Log("Test");
                        if (reflect.BindListener(target))
                        {
                            UnityEngine.Debug.Log("DataBind bind listener target field (" + field.Name + ") in " + target.name);
                            result.properties.Add(reflect, target);
                        }

                        //Since we don't bind all the child, we break once we got one
                        break;
                    }
                }
            }

            //Nothing bind
            if (result.properties.Count == 0)
            {
                return null;
            }

            return result;
        }

        //Breadth-first search
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }

        public static string ToProperCase(this string the_string)
        {
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            string result = the_string.Substring(0, 1).ToUpper();

            for (int i = 1; i < the_string.Length; i++)
            {
                if (char.IsUpper(the_string[i])) result += " ";
                result += the_string[i];
            }

            return result;
        }

        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToMeaningfulName(this string value)
        {
            return Regex.Replace(value, "(?!^)([A-Z])", " $1");
        }
    }
}