using System.Collections;
using System.Collections.Generic;
using jbzd.Common;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jbzd.MinorSystems.Barks
{
    public class BarkController: MonoBehaviour
    {
        [SerializeField] private TMP_Text barksText;
        [SerializeField] private GameObject textCloudImage;
        [SerializeField] [JbzdReadOnly] private bool isBarking;
        [SerializeField] private int timeBetweenBarks;
        [SerializeField] private int timeOfDisplayingBarks;
        [SerializeField] [TextArea(1,5)] private List<string> barks;
        
        private Camera _camera;
        private Collider _parentCollider;
        private RectTransform _textCloudImageRectTransform;
        private bool _shouldFollow;
        
        private void Awake()
        {
            _camera = Camera.main;
            _parentCollider = transform.parent.GetComponent<Collider>();
            _textCloudImageRectTransform = textCloudImage.GetComponent<RectTransform>();
            Debug.Assert(_camera, "Missing main camera");
            Debug.Assert(_parentCollider, "Missing collider on parent object");
            Debug.Assert(textCloudImage, "Missing game object for cloud image");
        }

        private void Update()
        {
            if (!_shouldFollow) return;

            AlignTextCloud();
        }

        private void AlignTextCloud()
        {
            const float offsetTextCloudStartPointToReferencePointXaxis = 50f;
                
            var centerPosition = _parentCollider.bounds.center;
            var halfHeight = _parentCollider.bounds.extents.y;
            var topPoint = new Vector3(centerPosition.x, centerPosition.y + halfHeight, centerPosition.z);
                
            var result = _camera.WorldToScreenPoint(topPoint);
            var sizeDelta = _textCloudImageRectTransform.sizeDelta;
            _textCloudImageRectTransform.position = result - new Vector3(sizeDelta.x * 0.5f - offsetTextCloudStartPointToReferencePointXaxis, -sizeDelta.y * 0.5f, 0);
        }
        
        private IEnumerator Barking()
        {
            for (;;)
            {
                if (!isBarking)
                {
                    HideNpcBark();
                    break;
                }

                ShowNpcBark(barks[Random.Range(0, barks.Count)]);
                
                yield return new WaitForSeconds(timeOfDisplayingBarks);
                HideNpcBark();
                yield return new WaitForSeconds(timeBetweenBarks);
            }
        }

        public void ShowNpcBark(string textToShow)
        {
            _shouldFollow = true;
            AlignTextCloud();
            textCloudImage.SetActive(true);
            barksText.text = textToShow;
        }

        private void HideNpcBark()
        {
            _shouldFollow = false;
            textCloudImage.SetActive(false);
            barksText.text = "";
        }

        /// <summary>
        /// Starts showing bark above parent of this game object. It takes random bark from <see cref="barks"/> and display drawn bark
        /// for <see cref="timeOfDisplayingBarks"/> time period, then wait for <see cref="timeBetweenBarks"/> until next bark is drawn.
        /// </summary>
        public void StartRandomBarks()
        {
            if(isBarking) return;
            
            isBarking = true;
            StartCoroutine(Barking());
        }
        
        /// <summary>
        /// Stops showing random barks, started by <see cref="StartRandomBarks"/>.
        /// </summary>
        public void StopRandomBarks()
        {
            if(isBarking is false) return;
            
            isBarking = false;
        }
    }
}