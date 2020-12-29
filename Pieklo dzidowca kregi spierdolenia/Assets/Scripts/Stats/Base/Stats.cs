namespace jbzdy.CharacterStats
{
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300
    }

    public class Stats
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int ReadOrder;
        public readonly object Source;

        public Stats(float value, StatModType type, int readOrder, object source)
        {
            Value = value;
            Type = type;
            ReadOrder = readOrder;
            Source = source;
        }

        public Stats(float value, StatModType type) : this(value, type, (int)type, null)
        {

        }

        public Stats(float value, StatModType type, int readOrder) : this(value, type, readOrder, null)
        {

        }
        public Stats(float value, StatModType type, object source) : this(value, type, (int)type, source)
        {

        }
    }
}