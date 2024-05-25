using System.Collections;
using UnityEngine;

namespace jbzd.Scenes.PlayerBehindObstaclesView.V2
{
    public class ObjectFader : MonoBehaviour
    {
        public float fadeTime = 0.3f;
        public float fadeAmount = 0.5f;
        private float originalOpacity = 1f;
        public Renderer[] renderers; // Assuming you want to fade materials of these renderers
        private Coroutine currentFadeCoroutine;

        void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        public void StartFadeOut()
        {
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);
            
            currentFadeCoroutine = StartCoroutine(FadeOutCoroutine());
        }

        public void StartFadeIn()
        {
            if (currentFadeCoroutine != null)
                StopCoroutine(currentFadeCoroutine);
            
            currentFadeCoroutine = StartCoroutine(FadeInCoroutine());
        }

        IEnumerator FadeOutCoroutine()
        {
            float startOpacity = GetCurrentOpacity();
            float targetOpacity = fadeAmount;

            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeTime);
                SetOpacity(newOpacity);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator FadeInCoroutine()
        {
            float startOpacity = GetCurrentOpacity();
            float targetOpacity = originalOpacity;

            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / fadeTime);
                SetOpacity(newOpacity);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        void SetOpacity(float opacity)
        {
            foreach (Renderer renderer in renderers)
            {
                Color color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, opacity);
            }
        }

        float GetCurrentOpacity()
        {
            if (renderers.Length > 0 && renderers[0].material != null)
            {
                return renderers[0].material.color.a;
            }
            return 0f;
        }
    }
}
