using Blaze.Property;
// using Sirenix.OdinInspector;
using UnityEngine;

public class TestProperty : MonoBehaviour
{
    public StringProperty playerName;
    public IntProperty playHp;
    public FloatProperty attack;
    public FloatProperty attack2;

    public SpriteProperty character;
    // public ImageProperty icon;

    public PrefabListProperty levelList;

    // [DrawWithUnity]
    // public Bummy[] haha;

    [System.Serializable]
    public class Bummy
    {
        public IntProperty playHp;
        public BoolProperty nice;
    }

    private void Start()
    {
        playerName.Value = "setting new value";
        playerName.valueChanged.AddListener((value) =>
        {
            //value changed
        });
        //use as string directly
        string name = playerName;
        attack.Value = 20;
    }
}