using System.IO;
using jbzdy.Items;
using UnityEngine;
using jbzdy.Inventory.Database;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace jbzdy.Inventory.SaveLoad
{
    public class InventoryData
    {
        public readonly string[] itemNames;
        public readonly int[] stackSize;
        public readonly Vector2[] itemGridPos;

        public InventoryData(string[] itemNames, int[] stackSize, Vector2[] itemGridPos)
        {
            this.itemNames = itemNames;
            this.stackSize = stackSize;
            this.itemGridPos = itemGridPos;
        }
    }

    public class LevelData
    {
        public Vector3[] itemPos;
        public Quaternion[] itemRot;
        public string[] itemName;
        public int[] itemStackSize;
    }

    public class LootBoxData
    {
        public string[] itemNames;
        public string[] stackSize;
    }

    public class SaveData : MonoBehaviour
    {
        public ItemDatabase assetsDatabase;

        public KeyCode saveKeyCode = KeyCode.F5;
        public KeyCode loadKeyCode = KeyCode.F9;

        public static bool loadDataTrigger = false;

        public static GameObject instance;
        
        private void Update()
        {
            if (loadDataTrigger)
            {
                print("Attempt to load player save");
                Load();
                loadDataTrigger = false;
            }

            if (Input.GetKeyDown(saveKeyCode))
            {
                Save();
            }

            if (Input.GetKeyDown(loadKeyCode))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                loadDataTrigger = true;
            }
        }

        /// <summary>
        /// Method to save level state for transition persistance
        /// </summary>
        public void SaveLevelPeristence()
        {
            //Save scene items

            var allSceneItems = FindObjectsOfType<SharItem>();

            List<SharItem> enabledItems = new List<SharItem>();

            foreach (var item in allSceneItems)
            {
                if (item.isActiveAndEnabled)
                    enabledItems.Add(item);
            }

            LevelData itemsLevelData = new LevelData();

            itemsLevelData.itemName = new string[enabledItems.ToArray().Length];
            itemsLevelData.itemPos = new Vector3[enabledItems.ToArray().Length];
            itemsLevelData.itemRot = new Quaternion[enabledItems.ToArray().Length];
            itemsLevelData.itemStackSize = new int[enabledItems.ToArray().Length];

            for (int i = 0; i < enabledItems.ToArray().Length; i++)
            {
                itemsLevelData.itemName[i] = enabledItems.ToArray()[i].itemName;
                itemsLevelData.itemPos[i] = enabledItems.ToArray()[i].transform.position;
                itemsLevelData.itemRot[i] = enabledItems.ToArray()[i].transform.rotation;
                itemsLevelData.itemStackSize[i] = enabledItems.ToArray()[i].itemStackSize;
            }

            string _itemsLevelData = JsonUtility.ToJson(itemsLevelData);
            File.WriteAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_persistenceItems", _itemsLevelData);
        }

        public void Save()
        {
            //Save inventory items

            var sceneItems = FindObjectsOfType<InventoryItem>();
            List<string> items = new List<string>();
            List<int> stacksize = new List<int>();
            List<Vector2> itemGridPos = new List<Vector2>();
            List<Vector2> itemRectPos = new List<Vector2>();

            foreach (var i_item in sceneItems)
            {
                items.Add(i_item.item.itemName);
                stacksize.Add(i_item.item.itemStackSize);
                itemGridPos.Add(new Vector2(i_item.x, i_item.y));
                itemRectPos.Add(i_item.GetComponent<RectTransform>().anchoredPosition);
            }

            var _i = items.ToArray();
            var _s = stacksize.ToArray();
            var _p = itemGridPos.ToArray();

            InventoryData inventoryData = new InventoryData(_i, _s, _p);
            string _inventoryData = JsonUtility.ToJson(inventoryData);
            File.WriteAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_inventoryData", _inventoryData);

            //Save scene items

            var allSceneItems = FindObjectsOfType<SharItem>();

            List<SharItem> enabledItems = new List<SharItem>();

            foreach (var item in allSceneItems)
            {
                if (item.isActiveAndEnabled)
                    enabledItems.Add(item);
            }

            LevelData itemsLevelData = new LevelData();

            itemsLevelData.itemName = new string[enabledItems.ToArray().Length];
            itemsLevelData.itemPos = new Vector3[enabledItems.ToArray().Length];
            itemsLevelData.itemRot = new Quaternion[enabledItems.ToArray().Length];
            itemsLevelData.itemStackSize = new int[enabledItems.ToArray().Length];

            for (int i = 0; i < enabledItems.ToArray().Length; i++)
            {
                itemsLevelData.itemName[i] = enabledItems.ToArray()[i].itemName;
                itemsLevelData.itemPos[i] = enabledItems.ToArray()[i].transform.position;
                itemsLevelData.itemRot[i] = enabledItems.ToArray()[i].transform.rotation;
                itemsLevelData.itemStackSize[i] = enabledItems.ToArray()[i].itemStackSize;
            }

            string _itemsLevelData = JsonUtility.ToJson(itemsLevelData);
            //print(_itemsLevelData);
            File.WriteAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_itemsLevelData", _itemsLevelData);
        }

        public void LoadLevelPersistence()
        {
            if (instance == null || loadDataTrigger)
                return;
            
            if (File.Exists(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_persistenceItems"))
            {
                SharItem[] existingItems = FindObjectsOfType<SharItem>();

                if (existingItems != null)
                {
                    foreach (SharItem item in existingItems)
                    {
                        Destroy(item.gameObject);
                    }
                }

                LevelData itemsLevelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_persistenceItems"));

                for (int i = 0; i < itemsLevelData.itemName.Length; i++)
                {
                    if (itemsLevelData.itemName[i] != null)
                    {
                        try
                        {
                            var item = Instantiate(assetsDatabase.FindItem(itemsLevelData.itemName[i]));
                            item.transform.position = itemsLevelData.itemPos[i];
                            item.transform.rotation = itemsLevelData.itemRot[i];
                            item.itemStackSize = itemsLevelData.itemStackSize[i];
                        }
                        catch
                        {
                            Debug.LogAssertion("Item you try to restore from save: " + itemsLevelData.itemName[i] + " is null or not exist in database");
                        }
                    }
                }
            }
        }

        public void ClearScenePersistence()
        {
            string sceneName = string.Empty;
            
            string[] sceneNamesInBuild = new string[SceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
                string _sceneName = Path.GetFileNameWithoutExtension(pathToScene);

                sceneNamesInBuild[i] = _sceneName;

            }

            foreach (var _sceneName in sceneNamesInBuild)
            {
                sceneName = _sceneName;

                string itemsLevelData = Application.persistentDataPath + "/" + sceneName + "_persistenceItems";
                string lootBoxData = Application.persistentDataPath + "/" + sceneName + "_persistenceLoot";

                try
                {
                    File.Delete(itemsLevelData);
                }
                catch
                {
                    Debug.Log("Attemp to clear persistence for scene " + sceneName + " is failed. Probably, scene persistent data not exist");
                }
                try
                {
                    File.Delete(lootBoxData);
                }
                catch
                {
                    Debug.Log("Attemp to clear persistence for scene " + sceneName + " is failed. Probably, scene persistent data not exist");
                }
            }

            print("Persistence for all levels in build was removed");
        }

        public void Load()
        {
            print("Load started");

            var itemsToDestroy = FindObjectsOfType<SharItem>();

            
            foreach (var item in itemsToDestroy)
            {
                Destroy(item.gameObject);
            }
            
            //Inventory
            InventoryClass inventory = FindObjectOfType<InventoryClass>();

            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(File.ReadAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_inventoryData"));

            var inventoryItems = inventoryData.itemNames;
            var stackSize = inventoryData.stackSize;
            var itemPos = inventoryData.itemGridPos;

            bool isAutoEquipEnabled = inventory.autoEquipItems;

            inventory.autoEquipItems = false;

            if (inventoryItems != null)
            {
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    var findItem = assetsDatabase.FindItem(inventoryItems[i]);

                    if (findItem != null)
                    {
                        var item = Instantiate(findItem);

                        item.itemStackSize = stackSize[i];

                        inventory.AddItem(item, (int)itemPos[i].x, (int)itemPos[i].y);
                    }
                    else
                    {
                        Debug.LogAssertion("Missing item. Check if it exists in the ItemsDatabase inspector");
                    }
                }
            }

            inventory.autoEquipItems = isAutoEquipEnabled;

            print("Looking for data for scene " + SceneManager.GetActiveScene().name);

            LevelData itemsLevelData = JsonUtility.FromJson<LevelData>(File.ReadAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + "_itemsLevelData"));

            for (int i = 0; i < itemsLevelData.itemName.Length; i++)
            {
                if (itemsLevelData.itemName[i] != null)
                {
                    try
                    {
                        var item = Instantiate(assetsDatabase.FindItem(itemsLevelData.itemName[i]));
                        item.transform.position = itemsLevelData.itemPos[i];
                        item.transform.rotation = itemsLevelData.itemRot[i];
                        item.itemStackSize = itemsLevelData.itemStackSize[i];
                    }
                    catch
                    {
                        Debug.LogAssertion("Item you try to restore from save: " + itemsLevelData.itemName[i] + " is null or not exist in database");
                    }
                }
            }
        }
    }
}