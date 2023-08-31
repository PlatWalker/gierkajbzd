using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.Playables;

namespace jbzd.QuestCreationScripts
{
    public class CutsceneTriggerOnTask : MonoBehaviour
    {
        [field: SerializeField] private Quest _quest;
        [field: SerializeField] private TaskSO _task;

        public PlayableDirector playableDirector;

        private void OnTriggerEnter(Collider other)
        {

            if (_quest is null || _task is null)
            {
                Debug.Log($"There is a trigger without quest or task on {name}. You need to place a quest or delete component");
                return;
            }

            if (_quest.ActiveTask == _task)
            {
                playableDirector.Play();
            }
        }
    }
}
