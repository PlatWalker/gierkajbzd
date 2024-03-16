using UnityEngine;
using UnityEngine.UI;

namespace jbzd.UI.Dialogues
{
    public class ScrollbarButton : MonoBehaviour
    {
        public float Step = 0.1f;
        private Scrollbar _scrollbar;
        public Button buttonUp;
        public Button buttonDown;
        
        public void Awake()
        {
            _scrollbar = gameObject.GetComponent<Scrollbar>();
        }

        public void Increment()
        {
            _scrollbar.value += Step;
            buttonUp.interactable = _scrollbar.value < 1;
        }
 
        public void Decrement()
        {
            _scrollbar.value -= Step;
            buttonDown.interactable = _scrollbar.value > 0;
        }

        public void UpdateButtons()
        {
            buttonUp.interactable = _scrollbar.value < 1;
            buttonDown.interactable = _scrollbar.value > 0;
        }
    }
}
