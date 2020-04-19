using Blaze.Property;
using UnityEngine;

namespace Blaze.Dialogue
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Blaze/Dialogue/Actor", order = 4000)]
    public class Actor : ScriptableObject
    {
        public TranslatableProperty actorName;
        public SpriteProperty icon;
    }
}