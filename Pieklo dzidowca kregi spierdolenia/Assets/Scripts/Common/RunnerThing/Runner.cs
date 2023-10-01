using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace jbzd.Common.RunnerThing
{
    /// <summary>
    /// That class only exist to wrap a method created from method with <see cref="RunMethod"/> attribute CustomRunnerFactory.
    /// </summary>
    public class Runner
    {
        private readonly MethodInfo _methodInfo;
        private readonly DiContainer _container;
        private readonly object _data;

        public Runner(MethodInfo methodInfo, DiContainer container, object data)
        {
            _methodInfo = methodInfo;
            _container = container;
            _data = data;
        }
        
        /// <summary>
        /// This method runs an extracted method. Inject only parameters from DI.
        /// </summary>
        public void Run()
        {
            List<object> args = new();
            
            foreach (var parameterType in _methodInfo.GetParameters().Select(info => info.ParameterType))
            {
                try
                {
                    args.Add(_container.Resolve(parameterType));
                }
                catch (Exception)
                {
                    Debug.LogError("There are types in method passed to Runner that are not registered in DIcontainer" 
                                   + "maybe try too use a overload of this method");
                }
            }
            _methodInfo.Invoke(_data, args.ToArray());
        }
        
        /// <summary>
        /// This method runs an extracted method. Inject parameters from DI and additional parameters.
        /// </summary>
        /// <param name="additionalParameters">Additional parameters that can be passed.
        /// Each parameter need to have unique type in list. For example if you need two ints wrap one of them in class</param>
        public void Run(List<object> additionalParameters)
        {
            var checkList = additionalParameters.Select(param => param.GetType()).ToList();
            Debug.Assert(checkList.Count() == checkList.Distinct().Count(), 
                "Each type in list must be unique, wrap it if necessary");
            
            List<object> args = new();
            
            foreach (var parameterType in _methodInfo.GetParameters().Select(info => info.ParameterType))
            {
                switch (_container.TryResolve(parameterType))
                {
                    case null or IList {Count: 0}:
                    {
                        var parameter = additionalParameters.FirstOrDefault(param => param.GetType() == parameterType);
                        if (parameter != null)
                        {
                            args.Add(parameter);
                        }

                        break;
                    }
                    default:
                        args.Add(_container.Resolve(parameterType));
                        break;
                }
            }

            _methodInfo.Invoke(_data, args.ToArray());
        }
    }
}