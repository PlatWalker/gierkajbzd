using UnityEngine;

namespace jbzd.Common
{
    public static class ErrorTemplate
    {
        public static string NotAssignedVariable(string variableName, GameObject gameObjectWithMissingVariable) => 
            $"There is not assigned value to variable {variableName} on game object {gameObjectWithMissingVariable.name}";
    }
}