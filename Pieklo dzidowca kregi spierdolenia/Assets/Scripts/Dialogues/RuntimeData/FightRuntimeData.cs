using jbzd.Common.RunnerThing;

namespace jbzd.Dialogues.RuntimeData
{
    public class FightRuntimeData : NodeRuntimeData
    {
        [RunMethod]
        public void Run(DialogueManager manager)
        {
            manager.NextNode();
        }
    }
}
