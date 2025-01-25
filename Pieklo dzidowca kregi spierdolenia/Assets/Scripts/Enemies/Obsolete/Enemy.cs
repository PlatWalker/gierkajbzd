using System;
using jbzd.MinorSystems.Cutscenes;
using UnityEngine;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.Enemies.Obsolete
{
    [Obsolete]
    public abstract class Enemy : MonoBehaviour
    {
        public delegate void EnemyDied();
        public abstract event EnemyDied OnDeath;

        private CutscenesManager _cutsceneManager;

        [Inject]
        public void Construct(CutscenesManager cutscenesManager)
        {  
            _cutsceneManager = cutscenesManager;
            _cutsceneManager.OnCutsceneStarted += Dissapear;
            _cutsceneManager.OnCutsceneEnded += Reappear;
        }

        public void Dissapear(TimelineAsset timelineAsset){
            gameObject.SetActive(false);
        }

        public void Reappear(TimelineAsset timelineAsset){
            gameObject.SetActive(true);
        }

        public void OnDestroy()
        {
            //TODO pokrzywa spawns childrens and not injecting. Fix it in future for now this if statement is quickfix
            if (_cutsceneManager is null) return;
            
            _cutsceneManager.OnCutsceneStarted -= Dissapear;
            _cutsceneManager.OnCutsceneEnded -= Reappear;
        }
    }
}