using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzd.Scenes.Scenes_Changing__Obsolete_
{
    public class ScenePartLoader : MonoBehaviour
    {

        [SerializeField]
        private bool enter;
        private GameObject targetObject;
        [SerializeField]
        private string mainSceneName;   // nazwa głównej sceny
        [SerializeField]
        private string mainSceneObjectName;  // tu należy podać nazwę GameObject'u pod którym są wszystkie GameObjecty dla danej sceny
        [SerializeField]
        private GameObject mainSceneTriggerName; // trigger którym wchodzimy do sceny głównej danego poziomu

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.CompareTag("Player")) return;
        

            Scene targetScene = SceneManager.GetSceneByName(mainSceneName);
            if (targetScene.isLoaded)
            {
                GameObject[] rootObjects = targetScene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                {
                    if (rootObject.name == mainSceneObjectName)
                    {
                        targetObject = rootObject;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("Scene " + mainSceneName + " is not loaded.");
            }
        
            if (enter)
            {
                if(gameObject.name == mainSceneTriggerName.name)
                {
                    targetObject.SetActive(true);
                }
                else
                {
                    SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
                }

                enter = false;
            }
            else
            {
                if(gameObject.name == mainSceneTriggerName.name)
                {
                    targetObject.SetActive(false);
                }
                else
                {
                    SceneManager.UnloadSceneAsync(gameObject.name);
                }

                enter = true;
            }

        }

    }
}
