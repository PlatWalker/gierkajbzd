using jbzd.Common.InteractSystem;
using jbzd.NPC;
using jbzd.Quests.QuestSpecificScripts.Level1;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class EscortGoal : QuestGoal
    {
        private GameObject _npc1;
        private GameObject _npc2;
        private GameObject _place1;
        private GameObject _place2;
        
        private Interaction _interaction1;
        private Interaction _interaction2;
        private NpcController _npc1Controller;
        private NpcController _npc2Controller;
        private BezOdpowiedzialnosci _questController;
        
        public override void InGameInit()
        {
            base.InGameInit();
            _questController = GameManager.Instance.QuestController;

            if (_questController.Place1.GetComponent<Interaction>() == null)
            {
                Debug.LogWarning("Punkt docelowy quest'a z eskortą musi być obiektem z interakcją!");
                return;
            }
            
            _npc1 = _questController.Npc1;
            _npc2 = _questController.Npc2;
            _npc1Controller = _questController.Npc1Controller;
            _npc2Controller = _questController.Npc2Controller;
            _place1 = _questController.Place1;
            _place2 = _questController.Place2;
            _interaction1 = _questController.Place1Interaction;
            _interaction2 = _questController.Place2Interaction;
                
            _npc1Controller.FollowPlayer = true;
            _npc2Controller.FollowPlayer = true;
            if (_questController.TaskNumber == 0) _interaction1.OnInteractionObjectReach += ReachedPlace;
            if (_questController.TaskNumber == 1) _interaction2.OnInteractionObjectReach += ReachedPlace;
        }

        private void ReachedPlace(Collider collider)
        {
            if (!(collider.gameObject == _npc1 || collider.gameObject == _npc2)) return;
        
            CurrentAmount++;

            // TODO tutaj powinienem sprawdzić warunek czy quest został zakończony... ale nie mogę bo jak go sprawdzę
            // TODO to automatycznie się quest skończy i wywoła metodę kończącą quest. PRzez co nie moge zrobić ostatnich
            // TODO rzeczy przed skończeniem questa... 
            if (CurrentAmount >= RequiredAmount) 
            {
                if (_questController.TaskNumber == 0)
                {
                    _interaction1.OnInteractionObjectReach -= ReachedPlace;

                    _npc1Controller.MoveTo(_place1.transform.position + new Vector3(Random.Range(0,2),0,Random.Range(0,5)));
                    _npc2Controller.MoveTo(_place1.transform.position + new Vector3(Random.Range(0,2),0,Random.Range(0,5)));
                }

                if (_questController.TaskNumber == 1)
                {
                    _interaction2.OnInteractionObjectReach -= ReachedPlace;
                    _npc1Controller.MoveTo(_place2.transform.position + new Vector3(Random.Range(0,2),0,Random.Range(0,5)));
                    _npc2Controller.MoveTo(_place2.transform.position + new Vector3(Random.Range(0,2),0,Random.Range(0,5)));
                }

                _npc1Controller.FollowPlayer = false;
                _npc2Controller.FollowPlayer = false;
                IsReached();
            }
        }

    }
}
