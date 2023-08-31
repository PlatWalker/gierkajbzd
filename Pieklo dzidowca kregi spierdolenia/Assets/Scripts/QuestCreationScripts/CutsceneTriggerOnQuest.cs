using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.Playables;

namespace jbzd.QuestCreationScripts
{
    public class CutsceneTriggerOnQuest : MonoBehaviour
    {
        [field: SerializeField] private Quest _quest;

        public PlayableDirector playableDirector;

        private void OnTriggerEnter(Collider other)
        {

            if (_quest is null)
            {
                Debug.Log($"There is a trigger without quest on {name}. You need to place a quest or delete component");
                return;
            }

            if (_quest.IsCompleted)
            {
                playableDirector.Play();
            }
        }
    }
}
