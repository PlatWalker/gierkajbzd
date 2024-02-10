using System;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Common;
using UnityEngine;

namespace jbzd.UI.PlayerMenu
{
    public class InGameMenuUIController : UserInterfaceController
    {
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnPlayerMenuOpened += OnPlayerMenuOpened;
        }

        private void OnPlayerMenuOpened()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            if(gameObject.activeSelf)
                FreezeTime.Freeze();
            else
                FreezeTime.Unfreeze();
        }

        public override bool InitialActivationState() => false;
    }
}
