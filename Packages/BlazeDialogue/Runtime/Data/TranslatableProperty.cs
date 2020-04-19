using Blaze.Property;

namespace Blaze.Dialogue
{
    [System.Serializable]
    public class TranslatableProperty : TextBaseProperty<Translatable>
    {
        public override Translatable Load(string key, Translatable defaultValue)
        {
            return null;
        }

        public override void Save(string key, Translatable value)
        {
            
        }
    }
}