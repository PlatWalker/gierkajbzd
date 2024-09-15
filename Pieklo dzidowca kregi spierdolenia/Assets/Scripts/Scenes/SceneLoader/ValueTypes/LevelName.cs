using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct LevelName
    {
        public readonly string Value;

        public LevelName(string levelName)
        {
            if (!levelName.StartsWith("(") || !levelName.EndsWith(")"))
            {
                Debug.LogError($" value type:{nameof(LevelName)} has been constructed with wrong constructor parameter: {levelName}," +
                               $"it should have started and end with round brackets '(' ')' ");
                Value = string.Empty;
                return;
            }

            if (levelName.Contains(" "))
            {
                Debug.LogError($" value type:{nameof(LevelName)} has been constructed with wrong constructor parameter: {levelName}," +
                               $"it should not have white spaces");
                Value = string.Empty;
                return;
            }

            Value = levelName;
        }
        
        public static implicit operator string(LevelName levelName)
        {
            return levelName.Value;
        }
        
        public static bool operator ==(LevelName left, LevelName right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(LevelName left, LevelName right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(LevelName left, string right)
        {
            return left.Value == right;
        }
        
        public static bool operator !=(LevelName left, string right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(string left, LevelName right)
        {
            return right.Value == left;
        }

        public static bool operator !=(string left, LevelName right)
        {
            return left != right.Value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is string str)
            {
                return Value == str;
            }

            return obj is LevelName other && Value == other.Value;
        }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public override string ToString()
        {
            return Value;
        }
    }
}