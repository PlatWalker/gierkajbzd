using System.Linq;
using UnityEngine;

namespace jbzd.Common.Extensions
{
    public static class AnimatorExtension
    {
        public static void CheckIfExists(this Animator animator, int animatorParameterHash, string parameterNameToLog)
        {
            if (animator.parameters.Any(x => x.nameHash == animatorParameterHash) == false)
                Debug.LogError($"Nie ma parametru o nazwie {parameterNameToLog} w animatorze: {animator.name}");
        }

        public static void CheckIfExists(this Animator animator, string parameterName)
        {
            if (animator.parameters.Any(x => x.name == parameterName) == false)
                Debug.LogError($"Nie ma parametru o nazwie {parameterName} w animatorze: {animator.name}");
        }
    }
}