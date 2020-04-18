# unity-blaze-property
Smart properties with data binding, events, save/load functions for unity

## Features
- **Smart Property for basic variable type**

- **Property has event callback accessible in the editor**

- **Custom Property Drawer for every Blaze Property**

- **Auto Databind feature with every Blaze Property using reflection**

- **Auto Reflection caching for Databind**

- **Works with ScriptableObject**

## General Properties
| Property | Databind Supported Target | Value Type | Description |
| --- | --- | --- | --- |
| `BoolProperty` | `TextMeshProUGUI` | `bool` | |
| `FloatProperty` | `TextMeshProUGUI` | `float` | |
| `IntProperty` | `TextMeshProUGUI` | `int` | |
| `StringProperty` | `TextMeshProUGUI` | `string` | |

## Databind Specific Properties
| Property | Databind Supported Target | Value Type | Description |
| --- | --- | --- | --- |
| `SpriteProperty` | `SpriteRenderer`, `Image` | `Sprite` | |
| `PrefabListProperty` | `Transform` | `GameObject(Prefab)` | Auto instantiate a list of data and auto databind the indivudal prefab instance, code initialization required. |
| `ButtonProperty` | `Button` | `UnityAction` | Auto bind the `UnityAction` to the `Button` target during databind, code initialization required. |

## Preview
![inspector](https://i.imgur.com/EeLOmLo.gif)

Decalare like normal variable in MonoBehaviour
<br />
![inspector](https://i.imgur.com/ZYBIFus.png)
