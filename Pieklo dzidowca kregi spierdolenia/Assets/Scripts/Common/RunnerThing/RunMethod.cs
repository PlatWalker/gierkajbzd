using System;

namespace jbzd.Common.RunnerThing
{
    /// <summary>
    /// Attribute that marks method that will be used by <see cref="Runner"/>. There can be only one for one class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RunMethod : Attribute
    {
        
    }
}