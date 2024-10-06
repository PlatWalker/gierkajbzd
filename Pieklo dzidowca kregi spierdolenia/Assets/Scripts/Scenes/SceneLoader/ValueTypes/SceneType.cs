using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct SceneType
    {
        public readonly string Value;

        public SceneType(string sceneType)
        {
            if (sceneType is not (SceneTypes.Interactive or SceneTypes.Passive or SceneTypes.SingleLoad))
            {
                Debug.LogError($" value type:{nameof(SceneType)} has been constructed with wrong constructor parameter: {sceneType}");
                Value = string.Empty;
                return;
            }
            
            var sceneTypeName =  sceneType switch
            {
                "(SingleLoad)" => "",
                "(Passive)" => "",
                "(Interactive)" => "",
                _ => "error"
            };

            if (sceneTypeName is "error")
            {
                Debug.LogError($" value type:{nameof(Krag)} has been constructed with wrong constructor parameter: {sceneType}");
                Value = string.Empty;
                return;
            }
            Value = sceneType;
        }
        
        public static implicit operator string(SceneType sceneType)
        {
            return sceneType.Value;
        }

        public static bool operator ==(SceneType left, SceneType right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(SceneType left, SceneType right)
        {
            return !(left == right);
        }

        public static bool operator ==(SceneType left, string right)
        {
            return left.Value == right;
        }
        
        public static bool operator !=(SceneType left, string right)
        {
            return !(left == right);
        }
        
        public static bool operator ==(string left, SceneType right)
        {
            return right.Value == left;
        }

        public static bool operator !=(string left, SceneType right)
        {
            return left != right.Value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is string str)
            {
                return Value == str;
            }

            return obj is SceneType other && Value == other.Value;
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