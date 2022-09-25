using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.UI.PlayerMenu
{
    public class PlayerMenuUIController : UserInterfaceController
    {
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnPlayerMenuOpened += () => gameObject.SetActive(!gameObject.activeSelf);
            input.OnEscapeClick += () => gameObject.SetActive(false);
        }

        public override bool InitialActivationState() => false;
    }
}
