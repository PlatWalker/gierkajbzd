///<summary>
///Created By Kumdzio
///</summary>


using UnityEngine;
namespace jbzd.Enemies
{
    public class EasyAnimatorController
    {
        private Animator animator;
        private string[] booleansNames;
        private string currentTrueBoolean = null;

        public EasyAnimatorController(Animator _animator, string[] ignoredBooleansArray)
        {
            this.animator = _animator;

            int boolParametersCount = 0;

            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    bool shouldIncreaseCounter = true;
                    foreach (string ignoredBoolean in ignoredBooleansArray)
                    {
                        if (ignoredBoolean == parameter.name)
                        {
                            shouldIncreaseCounter = false;
                            break;
                        }
                    }
                    if (shouldIncreaseCounter)
                    {
                        boolParametersCount++;
                    }
                }
            }

            this.booleansNames = new string[boolParametersCount];

            int i = 0;
            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    bool shouldUseThisBoolean = true;
                    foreach (string ignoredBoolean in ignoredBooleansArray)
                    {
                        if (ignoredBoolean == parameter.name)
                        {
                            shouldUseThisBoolean = false;
                            break;
                        }
                    }
                    if (shouldUseThisBoolean)
                    {
                        this.booleansNames[i++] = parameter.name;
                    }
                }
            }
            currentTrueBoolean = null;
        }

        public bool SetBooleanTrue(string booleanName)
        {
            if (currentTrueBoolean == booleanName) return true;
            foreach (string name in booleansNames)
            {
                if (name == booleanName)
                {
                    animator.SetBool(name, true);
                    if (currentTrueBoolean != null) animator.SetBool(currentTrueBoolean, false);
                    currentTrueBoolean = name;
                    return true;
                }
            }
            Debug.Log("Can't find boolean name: " + booleanName + " in AnimatorController: " + animator.name);
            return false;
        }

        public void ResetAllBooleans()
        {
            foreach (string name in booleansNames)
            {
                animator.SetBool(name, false);
            }
            currentTrueBoolean = null;
        }

        public string GetCurrentTrueBoolean()
        {
            return currentTrueBoolean;
        }

        public bool GetBoolean(string name)
        {
            return animator.GetBool(name);
        }

        public void SetBooleanDirectly(string name, bool value)
        {
            animator.SetBool(name, value);
        }
    }
}