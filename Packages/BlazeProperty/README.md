# unity-blaze-property
![GitHub](https://img.shields.io/github/license/BennyKok/unity-blaze)
[![openupm](https://img.shields.io/npm/v/com.blaze.property?label=blaze-property&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.blaze.property/)

Smart properties with data binding, events, save/load functions for unity

## Concept
The idea is to eliminate the boilerplate code to reference and update UI component every time, with `BlazeProperty`, now also comes with persistence options with the default `PlayerPrefs` and `UnityEvent` callback options, hopefully to make coding much more fun and enjoyable in Unity.

## Features
- **Smart Property for basic variable type**

- **Property has event callback accessible in the editor**

- **Custom Property Drawer for every Blaze Property**

- **Auto Databind feature with every Blaze Property using reflection**

- **Auto Reflection caching for Databind**

- **Works with ScriptableObject**

## Preview
![inspector](https://i.imgur.com/EeLOmLo.gif)

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

## Databind use case
When dealing with a list of data, most of the time is with `ScriptableObject`, and probably you have to create a UI prefab and instance it in a for loop and inject the data into individual UI component, in this case, `PrefabListProperty` will handle it automatcally, and the DataBind logic will auto bind the GameObject with the same name as the defined variable in your data class.

- e.g. `public FloatProperty hp;` will bind to a GameObject named `hp`'s `TextMeshProUGUI` component

## How to Help
If you're interested in helping or using this package, feel free to join the discord channel
<br/>
[Discord](https://discord.gg/NhRpw4g)

## License
MIT License

## Explore
Feel free to check out some of my free assets.
<br/>
[Asset Store](https://assetstore.unity.com/publishers/28510)
<br/>
[BennyKok](https://bennykok.com)

