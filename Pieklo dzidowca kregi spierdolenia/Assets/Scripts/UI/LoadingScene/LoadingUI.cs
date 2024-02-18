using jbzd.Common;
using UnityEngine;

namespace jbzd
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField] private bool _fromGameplay;

        public void OnSceneLoaded()
        {
            gameObject.SetActive(true);
            FreezeTime.Freeze();
        }

        public void OnSceneUnloaded()
        {
            if (_fromGameplay)
                gameObject.SetActive(false);
            FreezeTime.Unfreeze();
        }

        public void OnAsyncSceneLoadEnd(AsyncOperation asyncOperation)
        {
            OnSceneUnloaded();
            asyncOperation.completed -= OnAsyncSceneLoadEnd;
        }

    }
}