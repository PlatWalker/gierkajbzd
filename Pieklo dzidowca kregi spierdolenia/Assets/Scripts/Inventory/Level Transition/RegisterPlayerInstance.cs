using UnityEngine;

namespace jbzdy.Inventory.SaveLoad
{
    
    public class RegisterPlayerInstance : MonoBehaviour
    {
        private void Start()
        {
            if (SaveData.instance == null)
            {
                SaveData.instance = gameObject;
            }
            else
            {
                print("Destroy player duplicate. Player instance already exists");
                Destroy(gameObject);
            }
    }
    }
}
