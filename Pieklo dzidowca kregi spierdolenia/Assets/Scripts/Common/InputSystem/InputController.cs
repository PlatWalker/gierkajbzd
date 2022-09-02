using System;
using System.Collections.Generic;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;

namespace jbzd.Common.InputSystem
{
    public class InputController : MonoBehaviour
    {
        private readonly List<IInput> _inputs = new ();

        public void Awake()
        {
            _inputs.Add(new UserInterfaceInput());
            _inputs.Add(new PlayerInput());
        }

        private void Update()
        {
            foreach (var input in _inputs)
            {
                input.UpdateInputs();
            }
        }

        public T GetInput<T>() where T : IInput
        {
            var input = (T) _inputs.Find(input => input.GetType() == typeof(T));
            
            if (input != null) return input;

            Debug.LogWarning("There is no such input class!");
            return default;
        }
    }
}