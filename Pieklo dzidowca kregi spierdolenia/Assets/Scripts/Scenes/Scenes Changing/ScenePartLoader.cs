using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{

    [SerializeField]
    private bool enter;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        
        
        if (enter)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            enter = false;
        }
        else
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            enter = true;
        }

    }

}
