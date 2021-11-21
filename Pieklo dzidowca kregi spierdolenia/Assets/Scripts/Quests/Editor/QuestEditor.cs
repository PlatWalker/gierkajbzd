using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    private Quest quest;
    private short infoBox = 0;
    private int popUpOption = 0;

    private void OnEnable()
    {
        quest = (Quest)target;
    }

    public override void OnInspectorGUI()
    {
        SerializedObject serializedObject = new SerializedObject(quest);

        SerializedProperty serializedList = serializedObject.FindProperty("tasks");

        serializedObject.Update();

        quest.title = EditorGUILayout.TextField("Tytuł", quest.title);
        EditorGUILayout.LabelField("Opis");
        quest.description = EditorGUILayout.TextArea(quest.description, GUILayout.Height(40));
        quest.expReward = EditorGUILayout.IntField("Ilość expa", quest.expReward);
        quest.goldReward = EditorGUILayout.IntField("Ilość złota", quest.goldReward);

        EditorGUILayout.Space(20);

        DrawTasks(serializedList);

        EditorGUILayout.Space(50);
        AddTask();
        SaveQuest();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTasks(SerializedProperty tasks)
    {

        for(int i=0; i < tasks.arraySize; i++)
        {

            tasks.GetArrayElementAtIndex(i).FindPropertyRelative("showMore").boolValue = EditorGUILayout.Foldout(tasks.GetArrayElementAtIndex(i).FindPropertyRelative("showMore").boolValue, "zadanie " + i);

            if (tasks.GetArrayElementAtIndex(i).FindPropertyRelative("showMore").boolValue)
            {
                EditorGUILayout.BeginVertical("box");

                if(tasks.arraySize > 1)
                {
                    quest.tasks[i].order = EditorGUILayout.IntSlider("Kolejność", tasks.GetArrayElementAtIndex(i).FindPropertyRelative("order").intValue, 0, tasks.arraySize - 1);
                    EditorGUILayout.Space(10);
                }
                else
                {
                    quest.tasks[i].order = 0;
                }

                ShowElements(tasks.GetArrayElementAtIndex(i).FindPropertyRelative("goals"), i);

                DrawButtons(i);
                EditorGUILayout.Space(10);
                DeleteTask(i);
                EditorGUILayout.Space(10);

                EditorGUILayout.EndVertical();
            }
            
        }

    }

    private void AddTask()
    {
        if (GUILayout.Button("Dodaj zadanie"))
        {
            quest.tasks.Add(new Quest.Task());
        }
    }

    private void DeleteTask(int index)
    {
        if (GUILayout.Button("Usuń zadanie"))
        {
            foreach(QuestGoal goal in quest.tasks[index].goals)
            {
                Object.DestroyImmediate(goal, true);
                AssetDatabase.SaveAssets();
            }
            quest.tasks.RemoveAt(index);
        }
    }

    private void ShowElements(SerializedProperty goalList, int index)
    {

        for (int i = 0; i < goalList.arraySize; i++)
        {

            quest.tasks[index].goals[i].GoalCustomEditor();

            if (GUILayout.Button("Usuń", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
            {
                Object.DestroyImmediate(goalList.GetArrayElementAtIndex(i).objectReferenceValue, true);
                quest.tasks[index].goals.RemoveAt(i);
                AssetDatabase.SaveAssets();

            }

            EditorGUILayout.Separator();

        }

    }

    private void DrawButtons(int i)
    {
        string[] options = new string[] { "Zabij", "Przeprowadź interakcje", "Zbierz przedmioty", "Dotrzyj do miejsca", "Zaprowadź npc" };
        

        EditorGUILayout.BeginHorizontal();

        popUpOption = EditorGUILayout.Popup(popUpOption, options);
        if (GUILayout.Button("Dodaj", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
        {
            switch (popUpOption)
            {
                case 0:
                    KillGoal killGoal = ScriptableObject.CreateInstance<KillGoal>();
                    killGoal.name = "Nowy kill goal";
                    quest.tasks[i].goals.Add(killGoal);
                    AssetDatabase.AddObjectToAsset(killGoal, quest);
                    AssetDatabase.SaveAssets();
                    break;
                case 1:
                    TalkGoal talkGoal = ScriptableObject.CreateInstance<TalkGoal>();
                    talkGoal.name = "Nowy talk goal";
                    quest.tasks[i].goals.Add(talkGoal);
                    AssetDatabase.AddObjectToAsset(talkGoal, quest);
                    AssetDatabase.SaveAssets();
                    break;
                case 2:
                    ItemGoal itemGoal = ScriptableObject.CreateInstance<ItemGoal>();
                    itemGoal.name = "Nowy item goal";
                    quest.tasks[i].goals.Add(itemGoal);
                    AssetDatabase.AddObjectToAsset(itemGoal, quest);
                    AssetDatabase.SaveAssets();
                    break;
                case 3:
                    PlaceGoal placeGoal = ScriptableObject.CreateInstance<PlaceGoal>();
                    placeGoal.name = "Nowy place goal";
                    quest.tasks[i].goals.Add(placeGoal);
                    AssetDatabase.AddObjectToAsset(placeGoal, quest);
                    AssetDatabase.SaveAssets();
                    break;
                case 4:
                    EscortGoal escortGoal = ScriptableObject.CreateInstance<EscortGoal>();
                    escortGoal.name = "Nowy escort goal";
                    quest.tasks[i].goals.Add(escortGoal);
                    AssetDatabase.AddObjectToAsset(escortGoal, quest);
                    AssetDatabase.SaveAssets();
                    break;
                default:
                    break;
            }
            
        }

        EditorGUILayout.EndHorizontal();
    }

    private bool ValidateQuest()
    {
        if (quest.tasks.Count == 0)
            return false;

        bool[] order = new bool[quest.tasks.Count];

        foreach(Quest.Task task in quest.tasks)
        {
            if (task.goals.Count == 0)
                return false;

            if (order[task.order])
                return false;
            else
                order[task.order] = true;
        }

        return true;
    }

    private void SaveQuest()
    {
        

        if (GUILayout.Button("Zatwierdź"))
        {
            
            if (ValidateQuest())
            {
                List<Quest.Task> tasks = new List<Quest.Task>(quest.tasks);

                foreach (Quest.Task task in tasks)
                {
                    quest.tasks.RemoveAt(task.order);
                    quest.tasks.Insert(task.order, task);
                }

                infoBox = 1;
            }
            else
                infoBox = 2;
        }

        switch (infoBox)
        {
            case 1:
                EditorGUILayout.HelpBox("Quest zapisany", MessageType.Info, true);
                break;
            case 2:
                EditorGUILayout.HelpBox("Sprawdź czy każde zadanie ma swój cel i czy kolejność się nie powtarza!", MessageType.Error, true);
                break;
            default:
                break;
        }
            
    }

}


