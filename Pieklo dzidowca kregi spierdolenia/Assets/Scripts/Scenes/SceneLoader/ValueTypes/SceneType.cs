using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct SceneType
    {
        private readonly string _sceneType;

        public SceneType(string sceneType)
        {
            if (sceneType is not (SceneTypes.Interactive or SceneTypes.Passive or SceneTypes.SingleLoad))
            {
                Debug.LogError($" value type:{nameof(SceneType)} has been constructed with wrong constructor parameter: {sceneType}");
                _sceneType = string.Empty;
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
                _sceneType = string.Empty;
                return;
            }
            _sceneType = sceneType;
        }

        public override string ToString()
        {
            return _sceneType;
        }
    }
}