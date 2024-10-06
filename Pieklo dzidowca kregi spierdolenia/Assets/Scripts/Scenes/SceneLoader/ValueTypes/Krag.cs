using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct Krag
    {
        public readonly string Value;
        
        public Krag(string kragName)
        {
            if (kragName is not 
                (Kregi.Krag1 or Kregi.Krag2 or Kregi.Krag3 
                or Kregi.Krag4 or Kregi.Krag5 or Kregi.Krag6
                or Kregi.Krag7 or Kregi.Krag8 or Kregi.Krag9 or Kregi.None))
            {
                Debug.LogError($" value type:{nameof(Krag)} has been constructed with wrong constructor parameter: {kragName}");
                Value = string.Empty;
                return;
            }
            Value = kragName;
        }

        public static implicit operator string(Krag krag)
        {
            return krag.Value;
        }
        
        public static bool operator ==(Krag left, Krag right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Krag left, Krag right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(Krag left, string right)
        {
            return left.Value == right;
        }
        
        public static bool operator !=(Krag left, string right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(string left, Krag right)
        {
            return right.Value == left;
        }

        public static bool operator !=(string left, Krag right)
        {
            return left != right.Value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is string str)
            {
                return Value == str;
            }

            return obj is Krag other && Value == other.Value;
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