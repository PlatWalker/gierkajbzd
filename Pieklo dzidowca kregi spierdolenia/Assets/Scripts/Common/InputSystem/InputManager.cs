using System;
using System.Collections.Generic;
using jbzd.Common.InputSystem.Inputs;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace jbzd.Common.InputSystem
{
    [UsedImplicitly]
    public class InputManager : ITickable , IInitializable
    {
        private readonly List<JbzdInput> _inputs = new ();

        public InputManager()
        {
            _inputs.Add(new UserInterfaceInput());
            _inputs.Add(new PlayerInput());
        }
        
        public void Initialize()
        {
            foreach (var input in _inputs)
            {
                input.Start();
            }
        }
        
        public void Tick()
        {
            foreach (var input in _inputs)
            {
                input.UpdateInputs();
            }
        }

        public T GetInput<T>() where T : JbzdInput
        {
            var input = (T) _inputs.Find(input => input.GetType() == typeof(T));
            
            if (input != null) return input;

            Debug.LogWarning("There is no such input class!");
            return default;
        }
    }
}