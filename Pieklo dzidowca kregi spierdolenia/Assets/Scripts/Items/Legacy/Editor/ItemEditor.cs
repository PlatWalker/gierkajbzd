using UnityEngine;
using UnityEditor;
using jbzdy.Items;
using jbzdy.Items.Enums;
using jbzdy.Actions.Interaction;

namespace jbzdy.Inventory.Editors
{
    public class ItemEditor : EditorWindow
    {
        private GameObject newItem;
        private GameObject finalItem;
        private GameObject interactionZone;

        private int index = 0;
        private bool generateInteractionZone;

        private SharItemTypes itemType;

        [MenuItem("Sharashino Tools/Create Item")]
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
                GUILayout.BeginVertical("HelpBox");

                newItem = (GameObject)EditorGUILayout.ObjectField("Item model: ", newItem, typeof(GameObject), true);
                generateInteractionZone = EditorGUILayout.Toggle("Interaction Zone: ", generateInteractionZone);

                if(generateInteractionZone)
                {
                    interactionZone = (GameObject)EditorGUILayout.ObjectField("Interaction Zone model: ", interactionZone, typeof(GameObject), true);
                }

                GUILayout.TextArea("Item Type", EditorStyles.boldLabel);
                itemType = (SharItemTypes)EditorGUILayout.EnumPopup(itemType, GUILayout.Width(100));

                if (GUILayout.Button("Next"))
                {
                    finalItem = Instantiate(newItem);

                    switch (itemType)
                    {
                        case SharItemTypes.Weapon:
                            {
                                finalItem.AddComponent<WeaponItem>();
                                break;
                            }
                        case SharItemTypes.Armor:
                            {
                                finalItem.AddComponent<ArmorItem>();
                                break;
                            }
                        case SharItemTypes.Trinket:
                            {
                                finalItem.AddComponent<TrinketItem>();
                                break;
                            }
                        case SharItemTypes.Consumable:
                            {
                                finalItem.AddComponent<ConsumableItem>();
                                break;
                            }
                        case SharItemTypes.None:
                            {
                                finalItem.AddComponent<SharItem>();
                                break;
                            }
                        default:
                            finalItem.AddComponent<SharItem>();
                            break;
                    }

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

                GUILayout.EndVertical();
            }
            else if(index == 1)
            {
                GUILayout.TextArea(newItem.name, EditorStyles.boldLabel);
                GUILayout.BeginVertical("HelpBox");

                if (finalItem != null)
                {
                    Editor newItemEditor;

                    switch (itemType)
                    {
                        case SharItemTypes.Weapon:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<WeaponItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case SharItemTypes.Armor:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<ArmorItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case SharItemTypes.Trinket:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<TrinketItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case SharItemTypes.Consumable:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<ConsumableItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        case SharItemTypes.None:
                            newItemEditor = Editor.CreateEditor(finalItem.GetComponent<SharItem>());
                            newItemEditor.OnInspectorGUI();
                            break;
                        default:
                            break;
                    }
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
            newWeapon.weaponLevel = EditorGUILayout.IntField("Weapon Level", newWeapon.weaponLevel);
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