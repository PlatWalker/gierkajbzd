using System;
using UnityEngine;

namespace jbzd.Common
{
    public class LookAtMainCamera : MonoBehaviour
    {
        private Transform _mainCameraTransform;

        public void Awake()
        {
            if (Camera.main is null)
            {
                Debug.LogError("Missing main camera");
                return;
            }
            
            _mainCameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            transform.rotation = _mainCameraTransform.rotation;
        }
    }
}
