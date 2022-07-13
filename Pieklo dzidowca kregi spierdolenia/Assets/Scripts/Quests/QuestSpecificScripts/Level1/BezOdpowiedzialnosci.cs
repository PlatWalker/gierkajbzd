using System;
using jbzd.Common.InteractSystem;
using jbzd.NPC;
using jbzd.Quests.QuestGoals;
using UnityEngine;

namespace jbzd.Quests.QuestSpecificScripts.Level1
{
    public class BezOdpowiedzialnosci : QuestCompleteEvent
    {
        [field:SerializeField]
        public GameObject Npc1 { get; private set; }
        [field:SerializeField]
        public GameObject Npc2 { get; private set; }
        [field:SerializeField]
        public GameObject Place1 { get; private set; }
        [field:SerializeField]
        public GameObject Place2 { get; private set; }
        
        public NpcController Npc1Controller { get; set; }
        public NpcController Npc2Controller { get; set; }
        public Interaction Place1Interaction { get; set; }
        public Interaction Place2Interaction { get; set; }
        //TODO potrzebuje informacji z jakis wyzszych klas: jaki w tej chwili jest wykonywany quest?
        public int TaskNumber { get; set; }

        public void OnEnable()
        {
            Npc1Controller = Npc1.GetComponent<NpcController>();
            Npc2Controller = Npc2.GetComponent<NpcController>();
            Place1Interaction = Place1.GetComponent<Interaction>();
            Place2Interaction = Place2.GetComponent<Interaction>();
        }

        public override void OnQuestComplete(QuestGoal questGoal)
        {
            if (TaskNumber == 0)
            {
                Npc1Controller.FollowPlayer = true;
                Npc2Controller.FollowPlayer = true;
            }

            TaskNumber++;
            
            Npc1Controller.Interact();
        }
    }
}
