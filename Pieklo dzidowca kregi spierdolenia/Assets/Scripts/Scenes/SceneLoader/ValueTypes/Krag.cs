using UnityEngine;

namespace jbzd.Scenes.SceneLoader.ValueTypes
{
    public readonly struct Krag
    {
        private readonly string _krag;
        
        public Krag(string kragName)
        {
            if (kragName is not 
                (Kregi.Krag1 or Kregi.Krag2 or Kregi.Krag3 
                or Kregi.Krag4 or Kregi.Krag5 or Kregi.Krag6
                or Kregi.Krag7 or Kregi.Krag8 or Kregi.Krag9 or Kregi.None))
            {
                Debug.LogError($" value type:{nameof(Krag)} has been constructed with wrong constructor parameter: {kragName}");
                _krag = string.Empty;
                return;
            }
            _krag = kragName;
        }

        public override string ToString()
        {
            return _krag;
        }
    }
}