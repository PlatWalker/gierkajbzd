using UnityEngine;

namespace jbzd.Common.Extensions
{
    public static class MonoBehaviourExtension
    {
        public static T SafeGetComponent<T>(this MonoBehaviour obj) where T : Component
        {
            var component = obj.GetComponent<T>();

            if (component is null) Debug.LogError($"Brakuje komponentu {typeof(T)} w {obj.name}");
            
            return component;
        }
        
        public static T SafeGetComponentInParent<T>(this MonoBehaviour obj) where T : Component
        {
            var component = obj.GetComponentInParent<T>();

            if (component is null) Debug.LogError($"Brakuje komponentu {typeof(T)} w {obj.name}");
            
            return component;
        }
        
        public static T SafeGetComponentInChildren<T>(this MonoBehaviour obj) where T : Component
        {
            var component = obj.GetComponentInChildren<T>();

            if (component is null) Debug.LogError($"Brakuje komponentu {typeof(T)} w {obj.name}");
            
            return component;
        }
    }
}