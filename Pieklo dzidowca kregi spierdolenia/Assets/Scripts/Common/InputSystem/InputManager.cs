using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.InputSystem.Inputs;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace jbzd.Common.InputSystem
{
    [UsedImplicitly] //Initiated in installer
    public class InputManager : ITickable , IInitializable
    {
        private readonly List<JbzdInput> _inputs = new ();

        public InputManager()
        {
            var fieldValues = GetType()
                .Assembly.GetTypes()
                .Where(type => type.BaseType == typeof(JbzdInput) 
                               && !type.IsAbstract
                               && type.IsClass);

            foreach (var fieldValue in fieldValues)
            {
                _inputs.Add(Activator.CreateInstance(fieldValue) as JbzdInput);
            }
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