using System.Linq;
using UnityEngine;

namespace jbzd.UI
{
    public struct BlackScreenAnimatorParameters
    {
        public static string StartFadeOutParam => "StartFadeOut";
        public static string StartFadeInParam => "StartFadeIn";
    }
    
    public class UIControllerLegacy : MonoBehaviour
    {
        #region Canvases References

        [SerializeField]
        private Canvas _blackScreenFadeCanvas;

        #endregion

        private Animator _blackScreenAnimator;

        public void ScreenFadeOut()
        {
            _blackScreenAnimator.SetTrigger(BlackScreenAnimatorParameters.StartFadeOutParam);
            _blackScreenFadeCanvas.sortingOrder = 0;
        }

        public void ScreenFadeIn()
        {
            _blackScreenAnimator.SetTrigger(BlackScreenAnimatorParameters.StartFadeInParam);
            _blackScreenFadeCanvas.sortingOrder = 2;
        }

        private void Start()
        {
            _blackScreenAnimator = _blackScreenFadeCanvas.GetComponent<Animator>();
            
            CheckAnimatorParameters();
        }

        private void CheckAnimatorParameters()
        {
            #region Black Screen parametres

            if(_blackScreenAnimator.parameters.Any(x => 
                   x.name == BlackScreenAnimatorParameters.StartFadeOutParam) == false && _blackScreenFadeCanvas.enabled) 
                Debug.Log("Blad w nazwie parametru Zanikania obrazu");
            if(_blackScreenAnimator.parameters.Any(x => 
                   x.name == BlackScreenAnimatorParameters.StartFadeInParam) == false && _blackScreenFadeCanvas.enabled) 
                Debug.Log("Blad w nazwie parametru Zanikania obrazu");

            #endregion
        }
    }
}
