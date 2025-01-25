using System.Collections;
using UnityEngine;

namespace jbzd.Common.Random
{
    public class DestroyThis: MonoBehaviour
    {
        [SerializeField] private int secondsToDestroy = 3;
        
        public void Start()
        {
            StartCoroutine(Destroy());
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(secondsToDestroy);
            Destroy(gameObject);
            yield return null;
        }
    }
}