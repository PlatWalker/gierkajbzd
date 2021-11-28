using UnityEditor;
using UnityEngine;

[System.Serializable]
public class QuestGoal : ScriptableObject 
{

    public string Description;
    public bool Completed { get; set; }
    public int requiredAmount = 1;
    public int currentAmount { get; set; }

    public virtual void Init()
    {
        Completed = false;
        currentAmount = 0;
    }

    protected bool IsReached()
    {

        if (currentAmount >= requiredAmount)
        {
            Complete();
            return true;
        }

        return false;
    }

    private void Complete()
    {
        Completed = true;
        Debug.Log("Koniec podzadania");
    }

    public override string ToString()
    {
        return Description + ": <i>" + currentAmount + "/" + requiredAmount + "</i>\n";
    }


    //metoda która w inspektorze wyświetla pola związane z goalem
    public virtual void GoalCustomEditor()
    {
        EditorGUILayout.LabelField("Opis");
        Description = EditorGUILayout.TextArea(Description as string, GUILayout.Height(20));
        requiredAmount = EditorGUILayout.IntField("Potrzebna ilość", requiredAmount);
    }
}

