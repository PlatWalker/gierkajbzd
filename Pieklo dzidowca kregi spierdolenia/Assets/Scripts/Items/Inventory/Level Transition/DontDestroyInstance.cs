using UnityEngine;

namespace jbzdy.Inventory.SaveLoad
{
    public class DontDestroyInstance : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
