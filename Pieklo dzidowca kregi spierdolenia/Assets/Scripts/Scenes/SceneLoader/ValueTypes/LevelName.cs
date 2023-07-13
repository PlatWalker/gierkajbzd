using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct LevelName
    {
        private readonly string _levelName;

        public LevelName(string levelName)
        {
            if (!levelName.StartsWith("(") || !levelName.EndsWith(")"))
            {
                Debug.LogError($" value type:{nameof(LevelName)} has been constructed with wrong constructor parameter: {levelName}," +
                               $"it should have started and end with round brackets '(' ')' ");
                _levelName = string.Empty;
                return;
            }

            if (levelName.Contains(" "))
            {
                Debug.LogError($" value type:{nameof(LevelName)} has been constructed with wrong constructor parameter: {levelName}," +
                               $"it should not have white spaces");
                _levelName = string.Empty;
                return;
            }

            _levelName = levelName;
        }

        public override string ToString()
        {
            return _levelName;
        }
    }
}