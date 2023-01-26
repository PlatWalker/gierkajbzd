using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.UI
{
    [Serializable]
    public abstract class UserInterfaceController : MonoBehaviour
    {
        /// <summary>
        /// Method that let you add handlers to user inputs. For example you can subscribe in this method
        /// to event that is invoked when player press escape.  
        /// </summary>
        /// <param name="input">Controller with button inputs that are responsible for UI</param>
        public abstract void ConnectInputToHandler(UserInterfaceInput input);
        /// <summary>
        /// Method that indicate if UI should be displayed on game start.
        /// </summary>
        /// <returns>Value that indicate if UI should be displayed on game start</returns>
        public abstract bool InitialActivationState();
    }
}