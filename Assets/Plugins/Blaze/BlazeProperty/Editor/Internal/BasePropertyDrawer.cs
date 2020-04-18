using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Blaze.Property
{
    public class BasePropertyDrawer : PropertyDrawer
    {
        public static int tabBtnWidth = 25;

        public static int verticalSpace = 2;
        public static int groupHorizontalSpace = 4;

        public static float subTitleVerticalSpace;

        SerializedProperty previousProp;

        public static GUIStyle miniButton;

        public static GUISkin smallToolBarSkin;

        public class PropertyTab
        {
            public AnimBool visible;
            public GUIContent icon;
            public List<ItemContent> contents;
        }

        // public List<PropertyTab> allTabs = new List<PropertyTab>();

        //Have to do this because in array default editor, same PropertyDrawer will be used for multiple items
        public Dictionary<string, TabsState> states = new Dictionary<string, TabsState>();

        public class TabsState
        {
            public List<PropertyTab> tabs;
            public int editItem = -1;
            public int trackedEditItem;
        }

        public bool init;

        public enum ItemType
        {
            Property, Header, HeaderButton, GUI
        }

        public class ItemContent
        {
            public SerializedProperty property;
            public ItemType type;
            public string name;
            public string buttonName;
            public Action buttonCallback;
            public SerializedProperty enableIf;
            public Func<bool> enableCallback;
            public Func<float> guiHeightCallback;
            public Action<Rect> guiDrawCallback;

            public ItemContent(ItemType type)
            {
                this.type = type;
            }

            public ItemContent(SerializedProperty property, ItemType type, string name)
            {
                this.type = type;
                this.name = name;

                if (type == ItemType.Property)
                {
                    this.property = property.FindPropertyRelative(name);
                }
            }

            public ItemContent(SerializedProperty property, ItemType type, string name, string buttonName, Action buttonCallback)
            : this(property, type, name)
            {
                this.buttonName = buttonName;
                this.buttonCallback = buttonCallback;
            }
        }

        public float GetItemsHeight(List<ItemContent> contents)
        {
            return contents.Sum(
                (x) =>
                {
                    if (x.enableIf != null && !x.enableIf.boolValue)
                    {
                        return 0;
                    }
                    if (x.enableCallback != null && !x.enableCallback.Invoke())
                    {
                        return 0;
                    }
                    if (x.guiHeightCallback != null)
                    {
                        return x.guiHeightCallback.Invoke();
                    }
                    if (x.type == ItemType.Property)
                    {
                        return EditorGUI.GetPropertyHeight(x.property) + verticalSpace;
                    }
                    else
                    {
                        return subTitleVerticalSpace + verticalSpace;
                    }
                }
                ) + verticalSpace * 2;
        }

        public void Init(SerializedProperty property)
        {
            if (!init)
            {
                init = true;

                miniButton = new GUIStyle("button");
                miniButton.fontSize = 10;

                // if (smallToolBarSkin == null)
                // {

                //     var smallToolBar = new GUIStyle(EditorStyles.miniButton);
                //     smallToolBar.name = "m_smallToobar";
                //     smallToolBar.fontSize = 10;

                //     var left = new GUIStyle(EditorStyles.miniButtonLeft);
                //     left.name = "m_smallToobarLeft";
                //     left.fontSize = 10;

                //     var right = new GUIStyle(EditorStyles.miniButtonRight);
                //     right.name = "m_smallToobarRight";
                //     right.fontSize = 10;

                //     var mid = new GUIStyle(EditorStyles.miniButtonMid);
                //     mid.name = "m_smallToobarMid";
                //     mid.fontSize = 10;

                //     smallToolBarSkin = ScriptableObject.CreateInstance<GUISkin>();
                //     smallToolBarSkin.customStyles = new GUIStyle[] { smallToolBar, left, right, mid };
                // }

                previousProp = property;

                Refresh(property);
            }
            else if (property.propertyPath != previousProp.propertyPath)
            {
                Refresh(property);
            }
        }

        public List<PropertyTab> GetCurrentTab(SerializedProperty property)
        {
            return GetCurrentState(property).tabs;
        }

        public TabsState GetCurrentState(SerializedProperty property)
        {
            if (!states.TryGetValue(property.propertyPath, out var currentTabState))
            {
                currentTabState = new TabsState();
                currentTabState.tabs = new List<PropertyTab>();
                states.Add(property.propertyPath, currentTabState);
            }
            return currentTabState;
        }

        public void Refresh(SerializedProperty property)
        {
            var isNew = false;
            var currentTab = GetCurrentTab(property);
            isNew = currentTab.Count == 0;

            if (isNew)
            {
                InitTab(property, currentTab);
                foreach (var tab in currentTab)
                {
                    tab.visible = new AnimBool();
                    tab.visible.speed = BlazeDrawerUtil.AnimSpeed;
                    tab.visible.valueChanged.AddListener(() => { BlazeDrawerUtil.RepaintInspector(property.serializedObject); });
                }
            }
        }

        public virtual void InitTab(SerializedProperty property, List<PropertyTab> tabs)
        {
            tabs.Clear();
        }

        public void CloseAllTab(SerializedProperty property)
        {
            foreach (var tab in GetCurrentTab(property))
            {
                tab.visible.target = false;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);
            var currentState = GetCurrentState(property);

            var allTabs = GetCurrentTab(property);

            var valueProperty = property.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, property);

            // position.y += verticalSpace * 2;

            var groupRect = new Rect(position.x, position.y + verticalSpace * 2, position.width, position.height - verticalSpace * 2);
            GUI.BeginGroup(groupRect, EditorStyles.helpBox);

            var localX = 6;
            var localY = 4;
            var localW = position.width - 6 * 2;
            // var propertyTitleRect = new Rect(localX, localY  + EditorGUIUtility.singleLineHeight + verticalSpace + verticalSpace, localW - tabBtnWidth * allTabs.Count, EditorGUIUtility.singleLineHeight);
            // EditorGUI.LabelField(propertyTitleRect, label);

            var tabX = 0f;

            var toolBarRect = new Rect(localX + tabX, localY + EditorGUIUtility.singleLineHeight + verticalSpace + verticalSpace, localW, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginChangeCheck();
            // var tempSkin = GUI.skin;
            // GUI.skin = smallToolBarSkin;
            var tempI = GUI.Toolbar(toolBarRect, currentState.editItem, allTabs.ConvertAll(x => x.icon.text).ToArray());
            // GUI.skin = tempSkin;

            if (EditorGUI.EndChangeCheck())
            {
                if (currentState.editItem == tempI)
                {
                    currentState.editItem = -1;
                    CloseAllTab(property);
                }
                else
                {
                    currentState.trackedEditItem = tempI;
                    currentState.editItem = tempI;
                    CloseAllTab(property);
                    allTabs[tempI].visible.target = true;
                }
            }

            // for (int i = 0; i < allTabs.Count; i++)
            // {
            //     var tab = allTabs[i];
            //     //localW - (allTabs.Count - i) * tabBtnWidth
            //     var tabWidth = GUI.skin.button.CalcSize(tab.icon).x;
            //     var btnRect = new Rect(localX + tabX, localY + EditorGUIUtility.singleLineHeight + verticalSpace + verticalSpace, tabWidth, EditorGUIUtility.singleLineHeight);
            //     tabX += tabWidth;
            //     if (GUI.Button(btnRect, tab.icon))
            //     {
            //         if (GetCurrentState(property).editItem == i)
            //         {
            //             GetCurrentState(property).editItem = -1;
            //             CloseAllTab(property);
            //         }
            //         else
            //         {
            //             GetCurrentState(property).trackedEditItem = i;
            //             GetCurrentState(property).editItem = i;
            //             CloseAllTab(property);
            //             tab.visible.target = true;
            //         }
            //     }
            // }

            // - tabBtnWidth * allTabs.Count
            var propertyRect = new Rect(localX, localY, localW, EditorGUIUtility.singleLineHeight);

            // if (!allTabs.Any(x => x.visible.value))
            EditorGUI.PropertyField(propertyRect, valueProperty, label);

            GUI.EndGroup();

            subTitleVerticalSpace = EditorStyles.miniBoldLabel.CalcHeight(GUIContent.none, position.width);

            var extraRectGroup = new Rect(position.x + verticalSpace * 2, position.y + EditorGUIUtility.singleLineHeight * 2 + verticalSpace * 4 + 8, position.width - verticalSpace * 4, EditorGUIUtility.singleLineHeight);
            // extraRectGroup = EditorGUI.IndentedRect(extraRectGroup);

            var displayTab = allTabs[currentState.trackedEditItem];
            if (BlazeDrawerUtil.BeginFade(displayTab.visible))
            {
                extraRectGroup.height = GetItemsHeight(displayTab.contents) + verticalSpace;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                GUI.BeginGroup(extraRectGroup);

                DrawItemContents(displayTab.contents, extraRectGroup.width);

                GUI.EndGroup();

                EditorGUI.indentLevel = indent;

            }
            BlazeDrawerUtil.EndFade();

            EditorGUI.EndProperty();
        }

        public void DrawItemContents(List<ItemContent> contents, float width)
        {
            var extraRect = new Rect(groupHorizontalSpace, verticalSpace, width - groupHorizontalSpace * 2, EditorGUIUtility.singleLineHeight);
            foreach (var item in contents)
            {
                if (item.enableIf != null && !item.enableIf.boolValue)
                {
                    continue;
                }
                if (item.enableCallback != null && !item.enableCallback.Invoke())
                {
                    continue;
                }
                switch (item.type)
                {
                    case ItemType.Property:
                        EditorGUI.PropertyField(extraRect, item.property);
                        extraRect.y += EditorGUI.GetPropertyHeight(item.property) + verticalSpace;
                        break;
                    case ItemType.Header:
                        EditorGUI.LabelField(extraRect, item.name, EditorStyles.miniBoldLabel);
                        extraRect.y += subTitleVerticalSpace + verticalSpace;
                        break;
                    case ItemType.HeaderButton:
                        EditorGUI.LabelField(extraRect, item.name, EditorStyles.miniBoldLabel);

                        var buttonRect = new Rect(width - 50 - groupHorizontalSpace, extraRect.y, 50, 16);
                        if (GUI.Button(buttonRect, item.buttonName, miniButton))
                        {
                            item.buttonCallback.Invoke();
                        }

                        extraRect.y += subTitleVerticalSpace + verticalSpace;
                        break;
                    case ItemType.GUI:
                        item.guiDrawCallback.Invoke(extraRect);
                        extraRect.y += item.guiHeightCallback.Invoke() + verticalSpace;
                        break;
                }
            }
        }

        public bool isEditingScriptableObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject is ScriptableObject;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            var extraHeight = 0f;

            var allTabs = GetCurrentTab(property);

            var displayTab = allTabs[GetCurrentState(property).trackedEditItem];
            extraHeight += (GetItemsHeight(displayTab.contents) + 6) * displayTab.visible.faded;

            //Group Top & Bottom Margin
            extraHeight += (groupHorizontalSpace) * (1 - displayTab.visible.faded);
            extraHeight += EditorGUIUtility.singleLineHeight + groupHorizontalSpace * 2;
            extraHeight += verticalSpace * 2;

            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("value")) + extraHeight;
        }
    }
}