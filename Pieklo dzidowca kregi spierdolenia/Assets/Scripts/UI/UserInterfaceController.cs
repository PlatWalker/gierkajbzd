using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.UI
{
    [Serializable]
    public abstract class UserInterfaceController : MonoBehaviour
    {
        public abstract void ConnectInputToHandler(UserInterfaceInput input);
        public abstract bool InitialActivationState();
    }
}