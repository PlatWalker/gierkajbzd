using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace jbzdy.UI
{
    public struct BlackScreenAnimatorParameters
    {
        public static string StartFadeOutParam => "StartFadeOut";
        public static string StartFadeInParam => "StartFadeIn";
    }
    
    public class UIController : MonoBehaviour
    {
        #region Canvases References

        [SerializeField]
        private Canvas _blackScreenFadeCanvas;

        #endregion

        private Animator _blackScreenAnimator;

        public void ScreenFadeOut() => _blackScreenAnimator.SetTrigger(BlackScreenAnimatorParameters.StartFadeOutParam);
        public void ScreenFadeIn() => _blackScreenAnimator.SetTrigger(BlackScreenAnimatorParameters.StartFadeInParam);
        
        private void Start()
        {
            _blackScreenAnimator = _blackScreenFadeCanvas.GetComponent<Animator>();
            
            CheckAnimatorParameters();
        }

        private void CheckAnimatorParameters()
        {
            #region Black Screen parametres

            if(_blackScreenAnimator.parameters.Any(x => 
                   x.name == BlackScreenAnimatorParameters.StartFadeOutParam) == false) 
                Debug.Log("Blad w nazwie parametru Zanikania obrazu");
            if(_blackScreenAnimator.parameters.Any(x => 
                   x.name == BlackScreenAnimatorParameters.StartFadeInParam) == false) 
                Debug.Log("Blad w nazwie parametru Zanikania obrazu");

            #endregion
        }
    }
}
