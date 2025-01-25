using Unity.Behavior;
using UnityEngine;

namespace jbzd.Common.Extensions
{
    public static class BlackboardVariableExtension
    {
        public static Node.Status CheckIfNotNull(this BlackboardVariable variable, string nameOfVariable, string nameOfNode)
        {
            if (variable == null)
            {
                Debug.LogError($"Missing variable {nameOfVariable} in node {nameOfNode}");
                return Node.Status.Failure;
            }

            return Node.Status.Success;
        }
    }
}