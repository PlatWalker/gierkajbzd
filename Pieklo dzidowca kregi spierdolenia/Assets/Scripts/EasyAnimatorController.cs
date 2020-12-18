///<summary>
///Created By Kumdzio
///</summary>


using UnityEngine;

public class EasyAnimatorController
{
    private Animator animator;
    private string[] booleansNames;
    private string currentTrueBoolean;

    public EasyAnimatorController(Animator _animator,string[] ignoredBooleansArray)
    {
        this.animator = _animator;

        int boolParametersCount=0;

        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                bool shouldIncreaseCounter = true;
                foreach (string ignoredBoolean in ignoredBooleansArray)
                {
                    if(ignoredBoolean == parameter.name)
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
        currentTrueBoolean = booleansNames[0];
    }

    public bool setBooleanTrue(string booleanName)
    {
        foreach (string name in booleansNames)
        {
            if (name == booleanName)
            {
                animator.SetBool(currentTrueBoolean, false);
                currentTrueBoolean = name;
                animator.SetBool(currentTrueBoolean, true);
                return true;
            }
        }
        Debug.Log("Can't find boolean name: " + booleanName + " in AnimatorController: " + animator.name);
        return false;
    }

    public void ResetAllBooleans()
    {
        foreach(string name in booleansNames)
        {
            animator.SetBool(name, false);
        }
    }

    public string GetCurrentTrueBoolean()
    {
        return currentTrueBoolean;
    }
}
