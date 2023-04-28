using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;
using Random = System.Random;

namespace jbzd.Dialogues.Barks
{
    [Serializable]
    public struct BarksDictionary {
        public BarksSO barksSet;
        public BarksTrigger barksTrigger;
        public BarksTrigger barksEndTrigger;
    }

    
    public class Barks : MonoBehaviour
    {

        [field: SerializeField] private TMP_Text text;

        private List<string> _activeBarks = new();

        [field: SerializeField] private float barkDuration = 3f;
        private bool _barking = false;

        [field: SerializeField] public BarksDictionary[] barksSets;

        private Coroutine _lastCoroutine = null;
        private RunnerFactory _factory;
        
        [Inject]
        public void Constructor(RunnerFactory runnerFactory)
        {
            _factory = runnerFactory;
        }
        
        void Start()
        {

            if (text is null)
            {
                Debug.LogWarning("Bark nie posiada miejsca na którym może sie wyświetlić");
            }

            foreach (var barks in barksSets)
            {
                if (barks.barksSet.startConditions.Count == 0)
                {
                    StartBarks(barks.barksSet);
                }
            }

        }
 
        public void CheckBarks(BarksTrigger incomingTrigger, bool isStarting = true)
        {

            foreach (var barksSet in barksSets)
            {
                var trigger = isStarting ? barksSet.barksTrigger : barksSet.barksEndTrigger;
                
                if (trigger == incomingTrigger && barksSet.barksSet.IsReady(_factory, isStarting))
                {
                    if(isStarting)
                        StartBarks(barksSet.barksSet);
                    else
                        StopBarks();
                    
                    return;
                }
            }
        }
        
        private void StartBarks(BarksSO barks)
        {
            StopBarks();
            
            _activeBarks = barks.barks;
            _lastCoroutine = StartCoroutine(Barking(barks.showOnce));
        }

        private void StopBarks()
        {
            if (_lastCoroutine == null) return;
                
            StopCoroutine(_lastCoroutine);
            _activeBarks = null;
            _barking = false;
            text.text = "";
        }

        private IEnumerator Barking(bool once = false)
        {

            do
            {
                if (!_barking)
                {
                    var random = new Random();
                    text.text = _activeBarks[random.Next(_activeBarks.Count)];
                }
                else
                {
                    text.text = "";
                }

                _barking = !_barking;
                yield return new WaitForSeconds(barkDuration);
            } while (!once);
            
            text.text = "";
        }
    }
}
