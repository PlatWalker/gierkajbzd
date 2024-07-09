using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using jbzd.QuestSystem.QuestStructureElements;
[CustomEditor(typeof(Quest))]
[CanEditMultipleObjects]
public class QuestEditor : Editor
{
    private Quest _quest;
    private SerializedProperty _questDataProperty;
    private SerializedProperty _actorsProperty;
    private SerializedProperty _isCompletedProperty;
    private SerializedProperty _activeTaskProperty;


    private List<bool> _foldoutList = new();
    private bool _foldoutTask;

    private void OnEnable()
    {   
        _quest = (Quest)target;   

        _questDataProperty = serializedObject.FindProperty("QuestData");
        _actorsProperty = serializedObject.FindProperty("Actors");
        _isCompletedProperty = serializedObject.FindProperty("isCompleted");
        _activeTaskProperty = serializedObject.FindProperty("activeTask");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        

        EditorGUILayout.PropertyField(_questDataProperty);
        EditorGUILayout.PropertyField(_actorsProperty);
        EditorGUILayout.PropertyField(_isCompletedProperty);
        EditorGUILayout.PropertyField(_activeTaskProperty);
        
        if(_quest.QuestData != null){
            while(_foldoutList.Count < _quest.QuestData.Tasks.Count) _foldoutList.Add(false);
            while(_foldoutList.Count > _quest.QuestData.Tasks.Count) _foldoutList.RemoveAt(_foldoutList.Count - 1);
        }
        
        
        

        _foldoutTask = EditorGUILayout.Foldout(_foldoutTask, "Tasks");
        
        EditorGUI.indentLevel++;
        if (_foldoutTask)
        {
            if(_quest.QuestData == null){
                EditorGUILayout.LabelField("No Quest Data", GUILayout.Width(300));
            }
            else{
                for (int i = 0; i < _quest.QuestData.Tasks.Count; i++)
                {   
                    GUILayout.BeginHorizontal();
                    _foldoutList[i] = EditorGUILayout.Foldout(_foldoutList[i], "Task");
                    GUILayout.Label("Order:", GUILayout.Width(40));
                    EditorGUILayout.IntField(_quest.QuestData.Tasks[i].Order, GUILayout.Width(50));
                    GUILayout.Label("", GUILayout.Width(10));
                    GUILayout.Label("isDone:", GUILayout.Width(50));

                    if(_quest.FinishedTasks.Contains(_quest.QuestData.Tasks[i])){
                        EditorGUILayout.ToggleLeft("",true, GUILayout.Width(50));
                    }
                    else{
                        EditorGUILayout.ToggleLeft("",false, GUILayout.Width(50));
                    }
                    GUILayout.Label("", GUILayout.Width(200));


                    GUILayout.EndHorizontal();
                    if (_foldoutList[i])
                    {
                        foreach(var goal in _quest.QuestData.Tasks[i].Goals)
                        {
                            EditorGUILayout.BeginHorizontal();
                            if(goal is jbzd.QuestSystem.Goals.KillEnemiesGoal){
                                // If game is turned off, _quest.GoalName is empty, so we need to check for that
                                if(_quest.GoalName.Count() == 0){
                                    EditorGUILayout.LabelField("Game start required for kill count.", GUILayout.Width(300));
                                }
                                else{
                                    EditorGUILayout.LabelField(goal.name, GUILayout.Width(130));
                                    
                                    if(_quest.FinishedGoals.Contains(goal)){
                                        EditorGUILayout.ToggleLeft("",true, GUILayout.Width(50));
                                    }
                                    else{
                                        EditorGUILayout.ToggleLeft("",false, GUILayout.Width(50));
                                    }

                                    var index = _quest.GoalName.IndexOf(goal.name);
                                    EditorGUILayout.LabelField(_quest.GoalItemCollected[index]+"/"+_quest.GoalCompletionItemCount[index], GUILayout.Width(130));

                                }


                            }
                            else{
                                EditorGUILayout.LabelField(goal.name, GUILayout.Width(130));
                                if(_quest.FinishedGoals.Contains(goal)){
                                    EditorGUILayout.ToggleLeft("",true, GUILayout.Width(50));
                                }
                                else{
                                    EditorGUILayout.ToggleLeft("",false, GUILayout.Width(50));
                                }
                                

                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
            
        }

        serializedObject.ApplyModifiedProperties();
    }
}
