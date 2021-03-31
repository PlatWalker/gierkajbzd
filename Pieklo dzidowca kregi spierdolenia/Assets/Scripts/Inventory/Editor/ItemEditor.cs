using UnityEngine;
using UnityEditor;
using jbzdy.Items;
using jbzdy.Actions.Interaction;

namespace jbzdy.Inventory.EditorW
{
    public class ItemEditor : EditorWindow
    {
        [MenuItem("Sharashino Inventory/Create Item")]
        static void Init()
        {
            ItemEditor _editor = (ItemEditor)GetWindow(typeof(ItemEditor));
            _editor.Show();
        }

        GameObject newItem;
        GameObject finalItem;
        GameObject interactionZone;
        
        int index = 0;
        bool generateInteractionZone;
        private void OnGUI()
        {
            if (index == 0)
            {
                GUILayout.TextArea("New item", EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox");

                newItem = (GameObject)EditorGUILayout.ObjectField("Item model", newItem, typeof(GameObject), true);
                interactionZone = (GameObject)EditorGUILayout.ObjectField("Interaction Zone", interactionZone, typeof(GameObject), true);
                generateInteractionZone = EditorGUILayout.Toggle("Generate Interaction Zone?", generateInteractionZone);

                if (GUILayout.Button("Next"))
                {
                    finalItem = Instantiate(newItem);
                    finalItem.AddComponent<Item>();
                    finalItem.tag = "Item";

                    if(generateInteractionZone)
                    {
                        interactionZone = Instantiate(interactionZone);
                        interactionZone.transform.parent = finalItem.transform;
                        ItemPickup itemPickup = finalItem.AddComponent<ItemPickup>();
                        itemPickup.InteractionZone = interactionZone.GetComponent<InteractionZone>();
                    }
                   

                    finalItem.name = newItem.name + " - Item";

                    Selection.activeGameObject = finalItem;
                    SceneView.lastActiveSceneView.FrameSelected();

                    index++;
                }
            }
            else if(index == 1)
            {
                GUILayout.TextArea("New item", EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox");
                

                if (finalItem != null)
                {
                    Editor editor = Editor.CreateEditor(finalItem.GetComponent<Item>());
                    editor.OnInspectorGUI();
                }
                else
                {
                    EditorGUILayout.HelpBox("Can't find item. Maybe reference is missed. Try again from first step", MessageType.Warning, true);
                }

                GUILayout.EndVertical();

                EditorGUILayout.HelpBox("Don't forget to save your item settings and prefab itself! Drag prefab to the items folder.", MessageType.Warning);
            }
        }
    }
}