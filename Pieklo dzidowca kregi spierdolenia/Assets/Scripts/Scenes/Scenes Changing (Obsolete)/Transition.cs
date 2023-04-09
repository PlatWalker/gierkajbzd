using System.Collections;
using UnityEngine;

namespace jbzd.Scenes.Scenes_Changing__Obsolete_
{
    public class Transition : MonoBehaviour
    {
        [SerializeField]
        private float transitionDelayTime = 1.0f;
        [SerializeField]
        private GameObject blackScreen;

        private void Start()
        {
            OnLoadLevel();
        }

        private void OnLoadLevel()
        {

            foreach (Transform child in blackScreen.transform)
                StartCoroutine(FadeInAndOut(child.gameObject, transitionDelayTime));

        }

        private IEnumerator FadeInAndOut(GameObject objectToFade, float duration)
        {

            MeshRenderer tempMeshRenderer = objectToFade.GetComponent<MeshRenderer>();

            if (tempMeshRenderer == null)
            {

                yield break;

            }

            Color currentColor = tempMeshRenderer.material.color;

            float a = 0, b = 1;
            float counter = 0f;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                float alpha = Mathf.Lerp(a, b, counter / duration);
                tempMeshRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

                yield return null;
            }

        }
    }
}
