using System;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.UI.PlayerMenu
{
    public class PlayerMenuUIController : UserInterfaceController
    {
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnPlayerMenuOpened += OnPlayerMenuOpened;
            input.OnEscapeClick += OnEscapeClick;
        }

        private void OnEscapeClick()
        {
            gameObject.SetActive(false);
        }

        private void OnPlayerMenuOpened()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public override bool InitialActivationState() => false;
    }
}
