using Blaze.Property;
using UnityEngine;

namespace Blaze.Dialogue
{
    [CreateAssetMenu(fileName = "Actor", menuName = "BlazeDialogue/Actor", order = 0)]
    public class Actor : ScriptableObject
    {
        public Translatable actorName;
        public SpriteProperty profilePicture;
    }
}