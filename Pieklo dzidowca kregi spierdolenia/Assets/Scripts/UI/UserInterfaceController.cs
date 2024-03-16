using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.UI
{
    [Serializable]
    public abstract class UserInterfaceController : MonoBehaviour
    {
        /// <summary>
        /// Method that indicate if UI should be displayed on game start.
        /// </summary>
        /// <returns>Value that indicate if UI should be displayed on game start</returns>
        public abstract bool InitialActivationState();
    }
}