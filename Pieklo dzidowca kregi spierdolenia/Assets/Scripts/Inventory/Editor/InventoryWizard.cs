using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using jbzdy.Items.Enums;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace jbzdy.Inventory.Editors
{
    public class InventoryWizard : EditorWindow
    {
        #region variables

        //Inventory
        private int SizeX, SizeY;
        private int CellRectSize;
        private int CellSpacing;

        private Sprite CellImage;
        private Sprite InventoryBackground;

        private Color normalCellColor = Color.white, hoveredCellColor = Color.grey, blockedCellCover = Color.red;
        private Color inventoryBackgroundColor = Color.white;

        private Color stackTextColor = Color.gray;
        private Font stackTextFont;

        //Loot window
        private Vector2 inventoryRectSize;
        private int LootSizeX, LootSizeY;

        //Equipment panels
        private int equipmentPanelsCount;
        private int currentEquipmentPanelsCount;
        
        private int[] equipmentPanelX;
        private int[] equipmentPanelY;
        
        private ItemTypes[] equipmentPanelType;
        private ArmorTypes[] armorPanelType;
        private WeaponTypes[] weaponPanelType;

        #endregion

        private InventoryClass inventory;

        public int tabIndex = 0;
        public string[] tabHeaders = new string[] { "Inventory", "Loot window", "Equipment panels" };
        public Transform inventoryTransform;

        [MenuItem("Sharashino Inventory/New Inventory")]
        static void Init()
        {
            InventoryWizard _editor = (InventoryWizard)GetWindow(typeof(InventoryWizard));

            _editor.Show();
        }

        void OnGUI()
        {
            tabIndex = GUILayout.Toolbar(tabIndex, tabHeaders);

            if (tabIndex == 0)
            {
                EditorGUILayout.LabelField("Inventory & Cell size", EditorStyles.centeredGreyMiniLabel);

                GUILayout.BeginVertical("HelpBox"); GUILayout.BeginVertical("GroupBox");

                SizeX = EditorGUILayout.IntSlider("Inventory Width size", SizeX, 1, 100);
                SizeY = EditorGUILayout.IntSlider("Inventory Height size", SizeY, 1, 100);

                normalCellColor = EditorGUILayout.ColorField("Normal cell color", normalCellColor);
                hoveredCellColor = EditorGUILayout.ColorField("Hovered cell color", hoveredCellColor);
                blockedCellCover = EditorGUILayout.ColorField("Blocked cell color", blockedCellCover);

                stackTextFont = (Font)EditorGUILayout.ObjectField("Stack text font", stackTextFont, typeof(Font), false);
                stackTextColor = EditorGUILayout.ColorField("Stack text color", stackTextColor);

                EditorGUILayout.LabelField("");

                CellRectSize = EditorGUILayout.IntSlider("Cell size", CellRectSize, 1, 200);
                CellImage = (Sprite)EditorGUILayout.ObjectField("Cell image", CellImage, typeof(Sprite), false);

                inventoryBackgroundColor = EditorGUILayout.ColorField("Background color", inventoryBackgroundColor);
                InventoryBackground = (Sprite)EditorGUILayout.ObjectField("Inventory background", InventoryBackground, typeof(Sprite), false);

                if (GUILayout.Button("Build inventory"))
                {
                    var eventSystem = Instantiate(new GameObject());
                    eventSystem.AddComponent<EventSystem>();
                    eventSystem.AddComponent<StandaloneInputModule>();
                    eventSystem.AddComponent<BaseInput>();
                    eventSystem.name = "Event System";

                    CleanUp();

                    if (CellImage == null)
                    {
                        EditorUtility.DisplayDialog("Can't spawn that", "Please add a cell image!", "Okay...");
                        return;
                    }

                    var obj = Instantiate(new GameObject());
                    obj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    obj.AddComponent<CanvasScaler>();
                    obj.AddComponent<GraphicRaycaster>();
                    obj.AddComponent<InventoryManager>();
                    eventSystem.gameObject.transform.parent = obj.transform;

                    obj.name = "Inventory Canvas";


                    var cellHolder = Instantiate(new GameObject());
                    cellHolder.transform.SetParent(obj.transform);
                    cellHolder.name = "Inventory";

                    inventoryTransform = obj.transform;

                    var cellHolderImage = cellHolder.AddComponent<Image>();
                    cellHolderImage.rectTransform.sizeDelta = new Vector2((CellRectSize + CellSpacing) * SizeX, (CellRectSize + CellSpacing) * SizeY);
                    cellHolderImage.rectTransform.anchoredPosition = Vector2.zero;
                    cellHolderImage.color = inventoryBackgroundColor;

                    inventoryRectSize = cellHolderImage.rectTransform.sizeDelta;

                    var inventory = cellHolder.AddComponent<InventoryClass>();
                    inventory.cellSize = CellRectSize;
                    inventory.padding = CellSpacing;
                    inventory.column = SizeX;
                    inventory.row = SizeY;

                    inventory.normalCellColor = normalCellColor;
                    inventory.blockedCellColor = blockedCellCover;
                    inventory.hoveredCellColor = hoveredCellColor;


                    var imgObj = Instantiate(new GameObject());
                    imgObj.AddComponent<Image>().sprite = CellImage;
                    imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(CellRectSize, CellRectSize);
                    imgObj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                    imgObj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                    imgObj.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                    imgObj.transform.SetParent(obj.transform);

                    imgObj.GetComponent<Image>().type = Image.Type.Sliced;
                    imgObj.GetComponent<Image>().color = Color.white;

                    imgObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 3000);
                    imgObj.AddComponent<GridSlot>();

                    imgObj.name = "Utility object";

                    var stackText = Instantiate(new GameObject());
                    stackText.transform.SetParent(imgObj.transform);

                    var stackTextComponent = stackText.AddComponent<Text>();
                    stackTextComponent.alignment = TextAnchor.LowerLeft;
                    stackTextComponent.raycastTarget = false;
                    stackTextComponent.rectTransform.anchorMin = new Vector2(0, 0);
                    stackTextComponent.rectTransform.anchorMax = new Vector2(0, 0);
                    stackTextComponent.rectTransform.anchoredPosition = new Vector2(53, 53);
                    stackTextComponent.color = stackTextColor;
                    if (stackTextFont != null)
                        stackTextComponent.font = stackTextFont;

                    inventory.cell = imgObj.GetComponent<Image>();
                    inventory.DrawPreview();

                    CleanUp(); CleanUp(); CleanUp(); CleanUp();
                }

                GUILayout.EndVertical();
                GUILayout.EndVertical();
            }
            if (tabIndex == 1)
            {
                inventory = FindObjectOfType<InventoryClass>();

                if(inventory == null)
                {
                    EditorGUILayout.HelpBox("No inventory in scene. Create an inventory first!", MessageType.Error);
                }else
                {
                    GUILayout.BeginVertical("GroupBox");

                    LootSizeX = EditorGUILayout.IntField("Loot Width size", LootSizeX);
                    LootSizeY = EditorGUILayout.IntField("Loot Height size", LootSizeY);

                    if (GUILayout.Button("Build Loot Window"))
                    {
                        var cellHolder = Instantiate(new GameObject());
                        cellHolder.transform.SetParent(inventoryTransform);
                        cellHolder.name = "Loot window";

                        var cellHolderImage = cellHolder.AddComponent<Image>();
                        cellHolderImage.rectTransform.sizeDelta = new Vector2((CellRectSize + CellSpacing) * LootSizeX, (CellRectSize + CellSpacing) * LootSizeY);
                        cellHolderImage.rectTransform.anchoredPosition = Vector2.zero;
                        cellHolderImage.color = inventoryBackgroundColor;

                        cellHolderImage.rectTransform.anchoredPosition = new Vector2(inventoryRectSize.x, 0);

                        inventory.lootPanel = cellHolder.GetComponent<RectTransform>();
                        inventory.lootRow = LootSizeX;
                        inventory.lootColumn = LootSizeY;

                        inventory.DrawPreview();
                        CleanUp();
                    }
                }
            }

            if (tabIndex == 2)
            {
                if (inventory == null)
                {
                    EditorGUILayout.HelpBox("No inventory in scene. Create an inventory first!", MessageType.Error);
                }
                else
                {
                    equipmentPanelsCount = EditorGUILayout.IntSlider("Equipment panels count", equipmentPanelsCount, 1, 10);

                    if (currentEquipmentPanelsCount != equipmentPanelsCount)
                    {
                        equipmentPanelX = new int[equipmentPanelsCount];
                        equipmentPanelY = new int[equipmentPanelsCount];
                        equipmentPanelType = new ItemTypes[equipmentPanelsCount];
                        armorPanelType = new ArmorTypes[equipmentPanelsCount];
                        weaponPanelType = new WeaponTypes[equipmentPanelsCount];

                        currentEquipmentPanelsCount = equipmentPanelsCount;
                    }

                    for (int i = 0; i < equipmentPanelsCount; i++)
                    {
                        GUILayout.BeginVertical("HelpBox");

                        equipmentPanelType[i] = (ItemTypes)EditorGUILayout.EnumPopup("Slot allowed item: ", equipmentPanelType[i]);

                        switch (equipmentPanelType[i])
                        {
                            case ItemTypes.Weapon:
                                {
                                    weaponPanelType[i] = (WeaponTypes)EditorGUILayout.EnumPopup("Slot allowed weapon: ", weaponPanelType[i]);
                                    DrawPanelSizeFields(i);
                                    break;
                                }
                            case ItemTypes.Armor:
                                {
                                    armorPanelType[i] = (ArmorTypes)EditorGUILayout.EnumPopup("Slot allowed weapon: ", armorPanelType[i]);
                                    DrawPanelSizeFields(i);
                                    break;
                                }
                            case ItemTypes.Trinket:
                                {
                                    DrawPanelSizeFields(i);
                                    break;
                                }
                            case ItemTypes.Consumable:
                                {
                                    DrawPanelSizeFields(i);
                                    break;
                                }
                            case ItemTypes.None:
                                break;
                            default:
                                break;
                        }

                        GUILayout.EndVertical();
                    }

                    if (GUILayout.Button("Build panels"))
                    {
                        inventory.equipmentPanels = new List<EquipmentPanel>();

                        for (int i = 0; i < equipmentPanelsCount; i++)
                        {
                            var cellHolder = Instantiate(new GameObject());
                            cellHolder.transform.SetParent(inventoryTransform);

                            switch (equipmentPanelType[i])
                            {
                                case ItemTypes.Weapon:
                                    {
                                        WeaponEquipmentPanel weaponPanel = cellHolder.AddComponent<WeaponEquipmentPanel>();

                                        weaponPanel.allowedWeaponType = weaponPanelType[i];
                                        weaponPanel.allowedItemType = equipmentPanelType[i];
                                        weaponPanel.width = equipmentPanelX[i];
                                        weaponPanel.height = equipmentPanelY[i];
                                        
                                        cellHolder.name = "Equipment panel: " + equipmentPanelType[i] + weaponPanelType[i];
                                        inventory.equipmentPanels.Add(weaponPanel);
                                        break;
                                    }
                                case ItemTypes.Armor:
                                    {
                                        ArmorEquipmentPanel armorPanel = cellHolder.AddComponent<ArmorEquipmentPanel>();
                                        
                                        armorPanel.allowedArmorType = armorPanelType[i];
                                        armorPanel.allowedItemType = equipmentPanelType[i];
                                        armorPanel.width = equipmentPanelX[i];
                                        armorPanel.height = equipmentPanelY[i];

                                        cellHolder.name = "Equipment panel: " + equipmentPanelType[i] + armorPanelType[i];
                                        inventory.equipmentPanels.Add(armorPanel);
                                        break;
                                    }
                                case ItemTypes.Trinket:
                                    {
                                        TrinkedEquipmentPanel trinketPanel = cellHolder.AddComponent<TrinkedEquipmentPanel>();

                                        trinketPanel.allowedItemType = equipmentPanelType[i];
                                        trinketPanel.allowedItemType = equipmentPanelType[i];
                                        trinketPanel.width = equipmentPanelX[i];
                                        trinketPanel.height = equipmentPanelY[i];

                                        cellHolder.name = "Equipment panel: " + equipmentPanelType[i];
                                        inventory.equipmentPanels.Add(trinketPanel);
                                        break;
                                    }
                                default:
                                    {
                                        EquipmentPanel equipmentPanel = cellHolder.AddComponent<ArmorEquipmentPanel>();

                                        equipmentPanel.allowedItemType = equipmentPanelType[i];
                                        equipmentPanel.allowedItemType = equipmentPanelType[i];
                                        equipmentPanel.width = equipmentPanelX[i];
                                        equipmentPanel.height = equipmentPanelY[i];

                                        cellHolder.name = "Equipment panel: " + equipmentPanelType[i];
                                        inventory.equipmentPanels.Add(equipmentPanel);
                                        break;
                                    }
                            }

                            var cellHolderImage = cellHolder.AddComponent<Image>();
                            cellHolderImage.rectTransform.sizeDelta = new Vector2((CellRectSize + CellSpacing) * equipmentPanelX[i], (CellRectSize + CellSpacing) * equipmentPanelY[i]);
                            cellHolderImage.rectTransform.anchoredPosition = Vector2.zero;
                            cellHolderImage.color = inventoryBackgroundColor;

                            CleanUp();
                        }

                        inventory.DrawPreview();
                    }
                }
            }
        }

        private void DrawPanelSizeFields(int i)
        {
            equipmentPanelX[i] = EditorGUILayout.IntField("Equipment Width size", equipmentPanelX[i]);

            if (equipmentPanelX[i] == 0)
                equipmentPanelX[i] = 1;

            equipmentPanelY[i] = EditorGUILayout.IntField("Equipment Height size", equipmentPanelY[i]);

            if (equipmentPanelY[i] == 0)
                equipmentPanelY[i] = 1;
        }

        public void CleanUp()
        {
            var garbage = GameObject.Find("New Game Object");

            DestroyImmediate(garbage);
        }

    }
}