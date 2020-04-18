# unity-blaze-property
Smart properties with data binding, events, save/load functions for unity

## Preview
![inspector](https://i.imgur.com/EeLOmLo.gif)

Decalare like normal variable in MonoBehaviour
<br />
![inspector](https://i.imgur.com/ZYBIFus.png)

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
