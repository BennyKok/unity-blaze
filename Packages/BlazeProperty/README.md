# unity-blaze-property
Smart properties with data binding, events, save/load functions for unity

## Preview
![inspector](https://i.imgur.com/EeLOmLo.gif)

Decalare like normal variable in MonoBehaviour
![inspector](https://i.imgur.com/ZYBIFus.png)

## Properties
| Property | Databind Support Target | Value Type | Description |
| --- | --- | --- | --- |
| `BoolProperty` | `TextMeshProUGUI` | `bool` | |
| `FloatProperty` | `TextMeshProUGUI` | `float` | |
| `IntProperty` | `TextMeshProUGUI` | `int` | |
| `StringProperty` | `TextMeshProUGUI` | `string` | |
| `SpriteProperty` | `SpriteRenderer`, `Image` | `Sprite` | |
| `PrefabListProperty` |  | `GameObject(Prefab)` | Auto instantiate a list of data and databind to the indivudal prefab instance |
| `ButtonProperty` | `Button` | `UnityAction` | |
