using jbzdy.DialogueSystem.SO;
using UnityEngine;

namespace jbzdy.DialogueSystem.Events
{
    [System.Serializable]
    [CreateAssetMenu(menuName ="Dialogue/New OpenShop Event", fileName = "OpenShop Event")]
    public class EventOpenShop : DialogueEventSO
    {
        public override void RunEvent()
        {
            base.RunEvent();
            OpenShop();
        }

        private void OpenShop()
        {
            Debug.Log("We coś kup byczq");
        }
    }
}

