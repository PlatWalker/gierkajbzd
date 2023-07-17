using System;
using System.Linq;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace jbzd.Common.RunnerThing
{
    /// <summary>
    /// This factory exist to help you use a scriptable object as a holder for method run in runtime.
    /// It is doing it by making runner class from method that has <see cref="RunMethod"/> attribute.
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
            var listOfMethods = runnerData
                .GetType()
                .GetMethods()
                .Where(info => info.GetCustomAttributes(typeof(RunMethod), false).Length > 0)
                .ToList();
            
            Debug.Assert(listOfMethods is not null, 
                $"There is no method marked with {nameof(RunMethod)} in {nameof(runnerData.GetType)}");
            Debug.Assert(listOfMethods.Count == 1, 
                $"There can be only one method marked with {nameof(RunMethod)} in {nameof(runnerData.GetType)}" );

            return new Runner(listOfMethods.First(), _container, runnerData); 
        }
    }
    
    public class RunnerFactory : PlaceholderFactory<object, Runner>
    {
    }
}