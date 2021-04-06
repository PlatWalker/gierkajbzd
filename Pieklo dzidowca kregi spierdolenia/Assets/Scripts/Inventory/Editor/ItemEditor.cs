using UnityEngine;
using UnityEditor;
using jbzdy.Items;
using jbzdy.Items.Enums;
using jbzdy.Actions.Interaction;
using System;

namespace jbzdy.Inventory.EditorW
{
    public class ItemEditor : EditorWindow
    {
        private GameObject newItem;
        private GameObject finalItem;
        private GameObject interactionZone;

        private int index = 0;
        private bool generateInteractionZone;

        private ItemTypes itemType;

        [MenuItem("Sharashino Inventory/Create Item")]
        static void Init()
        {
            ItemEditor _editor = (ItemEditor)GetWindow(typeof(ItemEditor));
            _editor.Show();
        }
       
        private void OnGUI()
        {
            if (index == 0)
            {
                GUILayout.TextArea("New item", EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox", GUILayout.Width(300));

                newItem = (GameObject)EditorGUILayout.ObjectField("Item model", newItem, typeof(GameObject), true);
                generateInteractionZone = EditorGUILayout.Toggle("Generate Interaction Zone?", generateInteractionZone); ;

                if(generateInteractionZone)
                {
                    interactionZone = (GameObject)EditorGUILayout.ObjectField("Interaction Zone", interactionZone, typeof(GameObject), true);
                }

                GUILayout.TextArea("Item Type", EditorStyles.boldLabel);
                itemType = (ItemTypes)EditorGUILayout.EnumPopup(itemType, GUILayout.Width(100));

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
                GUILayout.BeginVertical("HelpBox", GUILayout.Width(300));
                GUILayout.TextArea("New weapon item", EditorStyles.boldLabel);

                switch (itemType)
                {
                    case ItemTypes.Weapon:
                        CreateWeapon();
                        break;
                    case ItemTypes.Armor:
                        CreateArmor();
                        break;
                    case ItemTypes.Consumable:
                        CreateConsumable();
                        break;
                    case ItemTypes.None:
                        CreateNone();
                        break;
                    default:
                        break;
                }

                if (GUILayout.Button("Next"))
                {
                    index++;
                }
            }
            else if(index == 2)
            {
                GUILayout.TextArea(newItem.name, EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox");
                

                if (finalItem != null)
                {
                    Editor editor = Editor.CreateEditor(finalItem.GetComponent<Item>());
                    editor.OnInspectorGUI();
                }
                else
                {
                    EditorGUILayout.HelpBox("Coś rozjebałeś, spróbuj od nowa. TYM RAZEM TEGO NIE SPIERDOL!!!", MessageType.Warning, true);
                }

                GUILayout.EndVertical();

                EditorGUILayout.HelpBox("Zapisz jeszcze prefab ściągając go z hierarhii do Projektu.", MessageType.Warning);
            }
        }

        

        private void CreateWeapon()
        {
            finalItem.AddComponent<WeaponItem>();
            WeaponItem newWeapon = finalItem.GetComponent<WeaponItem>();

            newWeapon.weaponDamage = EditorGUILayout.IntField("Weapon Damage", newWeapon.weaponDamage);
            newWeapon.weaponLevel = EditorGUILayout.IntField("Weapon Damage", newWeapon.weaponLevel);
        }
        
        private void CreateArmor()
        {
        }
        private void CreateConsumable()
        {
        }

        private void CreateNone()
        {
            
        }

        

        
    }
}