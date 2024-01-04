using UnityEngine;
using jbzd.Cutscenes;
using Zenject;

namespace jbzd.Enemies
{
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

        public void Dissapear(){
            gameObject.SetActive(false);
        }

        public void Reappear(){
            gameObject.SetActive(true);
        }

    }
}