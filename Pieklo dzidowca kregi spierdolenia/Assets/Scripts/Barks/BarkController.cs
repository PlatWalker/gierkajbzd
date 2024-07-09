using System.Collections;
using System.Collections.Generic;
using jbzd.Common;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jbzd.Barks
{
    [RequireComponent(typeof(TMP_Text))]
    public class BarkController: MonoBehaviour
    {
        public int timeBetweenBarks;
        public int timeOfDisplayingBarks;
        
        [SerializeField]
        [JbzdReadOnly]
        private bool isBarking;
        
        public List<string> barks;

        private TMP_Text _barksText;

        public void Awake()
        {
            _barksText = GetComponent<TMP_Text>();
            _barksText.text = "";
        }

        public void StartBarks()
        {
            if(isBarking) return;
            
            isBarking = true;
            StartCoroutine(Barking());
        }

        public void StopBarks()
        {
            if(isBarking is false) return;
            
            isBarking = false;
        }

        private IEnumerator Barking()
        {
            for (;;)
            {
                if (!isBarking)
                {
                    _barksText.text = "";
                    break;
                }

                _barksText.text = barks[Random.Range(0, barks.Count)];
                
                yield return new WaitForSeconds(timeOfDisplayingBarks);
                _barksText.text = "";
                yield return new WaitForSeconds(timeBetweenBarks);
            }
        }
    }
}