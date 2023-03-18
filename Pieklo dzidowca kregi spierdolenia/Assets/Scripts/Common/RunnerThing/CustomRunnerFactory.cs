using System;
using System.Linq;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace jbzd.Common.RunnerThing
{
    /// <summary>
    /// This factory exist to help you use a scriptable object as a holder for method run in runtime.
    /// It is doing it by making runner class from method implemented in IRunnerData.
    /// </summary>
    public class CustomRunnerFactory : IFactory<object, Runner>
    {
        private readonly DiContainer _container;
        
        public CustomRunnerFactory(DiContainer container)
        {
            _container = container;
        }

        public Runner Create(object runnerData)
        {
            if (runnerData is not ScriptableObject)
            {
                Debug.LogWarning("Object passed to runner should be scriptable object otherwise it doesnt make sense"+
                                 "but it will work nonetheless");
            }

            var listOfMethods = runnerData
                .GetType()
                .GetMethods()
                .Where(info => info.GetCustomAttributes(typeof(RunMethod), false).Length > 0)
                .ToList();
            
            Debug.Assert(listOfMethods is not null, 
                $"There is no method marked with {nameof(RunMethod)} in {nameof(runnerData.GetType)}");
            Debug.Assert(listOfMethods.Count == 1, 
                $"There can be only one method marked with {nameof(RunMethod)} in {nameof(runnerData.GetType)}" );
            Debug.Assert(listOfMethods.Count == 0, 
                $"There need to be at least one method marked with {nameof(RunMethod)} in {nameof(runnerData.GetType)}" );
            
            return new Runner(listOfMethods.First(), _container, runnerData); 
        }
    }
    
    public class RunnerFactory : PlaceholderFactory<object, Runner>
    {
    }
}