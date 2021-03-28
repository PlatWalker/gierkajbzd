using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using jbzdy.Items.Enums;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using jbzdy.Inventory;
using System;

public class InventoryEditor1 : EditorWindow
{
    #region Properties

    //Inventory
    private int sizeX, sizeY;
    private int cellRectSize;
    private int cellSpacing;

    private Sprite cellImage;
    private Sprite inventoryBackground;

    [SerializeField] private Color defaultColor = Color.white, hoveredColor = Color.grey, blockedColor = Color.red, inventoryBackgroundColor = Color.white;
    
    private Color stackColor = Color.gray;
    private Font stackTextFont;

    //Loot
    private Vector2 lootRectSize;
    private int lootSizeX, lootSizeY;

    //Equipment
    private int equipmentPanelsCount;
    private int currentEquipmentPanelsCount;
    private int[] equipmentPanelX;
    private int[] equipmentPanelY;
    private EquipmentSlot equipmentPanelType;

    #endregion


    public int tabIndex = 0;
    public string[] tabHeaders = new string[] { "Inventory", "Loot", "Equipment" };

    public Transform inventoryTransform;
    public InventoryClass1 inventory;


    [MenuItem("Inventory/New Inventory")]
    static void Init()
    {
        InventoryEditor1 editor = (InventoryEditor1)GetWindow(typeof(InventoryEditor1));

        editor.Show();
    }

    private void OnGUI()
    {
        tabIndex = GUILayout.Toolbar(tabIndex, tabHeaders);

        if(tabIndex == 0)
        {
            EditorGUILayout.LabelField("Inventory & Cell size", EditorStyles.centeredGreyMiniLabel);

            sizeX = EditorGUILayout.IntSlider("Inventory X size", sizeX, 1, 100);
            sizeY = EditorGUILayout.IntSlider("Inventory Y size", sizeY, 1, 100);

            defaultColor = EditorGUILayout.ColorField("Default cell color", defaultColor);
            hoveredColor = EditorGUILayout.ColorField("Hovered cell color", hoveredColor);
            blockedColor = EditorGUILayout.ColorField("Blocked cell color", blockedColor);

            stackColor = EditorGUILayout.ColorField("Stacked cell color", stackColor);
            stackTextFont = (Font)EditorGUILayout.ObjectField("Stack text font", stackTextFont, typeof(Font), false);

            EditorGUILayout.LabelField("");

            cellRectSize = EditorGUILayout.IntSlider("Cell size", cellRectSize, 1, 200);
            cellSpacing = EditorGUILayout.IntSlider("Cell spacing", cellSpacing, 1, 50);

            cellImage = (Sprite)EditorGUILayout.ObjectField("Cell image", cellImage, typeof(Sprite), false);

            if(GUILayout.Button("Generate Inventory"))
            {
                GameObject eventSystem = Instantiate(new GameObject());
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                if (cellImage == null)
                {
                    EditorUtility.DisplayDialog("Generation not completed", "Zapomniałeś kurwo dać obrazka", "KURWAAAAAAA");
                    return;
                }

                GameObject canvasObj = Instantiate(new GameObject());
                canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                eventSystem.gameObject.transform.parent = canvasObj.transform;

                GameObject cellHolder = Instantiate(new GameObject());
                cellHolder.transform.SetParent(canvasObj.transform);

                cellHolder.name = "Inventory";
                inventoryTransform = canvasObj.transform;

                var inventoryCellImage = cellHolder.AddComponent<Image>();
                inventoryCellImage.rectTransform.sizeDelta = new Vector2((cellRectSize + cellSpacing) * sizeX, (cellRectSize + cellSpacing) * sizeY);
                inventoryCellImage.rectTransform.anchoredPosition = Vector2.zero;
                inventoryCellImage.color = Color.green;

                lootRectSize = inventoryCellImage.rectTransform.sizeDelta;

                var inventory = cellHolder.AddComponent<InventoryClass1>();
                inventory.cellSize = cellRectSize;
                inventory.padding = cellSpacing;
                inventory.column = sizeX;
                inventory.row = sizeY;

                inventory.defaultColor = defaultColor;
                inventory.blockedColor = blockedColor;
                inventory.hoveredColor = hoveredColor;

                var imageObj = Instantiate(new GameObject());
                imageObj.AddComponent<Image>().sprite = cellImage;

                RectTransform imageRect = imageObj.GetComponent<RectTransform>();
                imageRect.sizeDelta = new Vector2(cellRectSize, cellRectSize);
                imageRect.anchorMin = new Vector2(0, 1);
                imageRect.anchorMax = new Vector2(0, 1);
                imageRect.pivot = new Vector2(0, 1);
                imageObj.transform.SetParent(canvasObj.transform);

                imageObj.GetComponent<Image>().type = Image.Type.Sliced;
                imageObj.GetComponent<Image>().color = Color.white;
                imageObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 3000);
                imageObj.AddComponent<GridSlot1>();

                var stackTextObj = Instantiate(new GameObject());
                stackTextObj.transform.SetParent(imageObj.transform);

                var stackTextComponent = stackTextObj.AddComponent<Text>();
                stackTextComponent.alignment = TextAnchor.LowerLeft;
                stackTextComponent.raycastTarget = false;
                stackTextComponent.rectTransform.anchorMin = new Vector2(0, 0);
                stackTextComponent.rectTransform.anchorMax = new Vector2(0, 0);
                stackTextComponent.rectTransform.anchoredPosition = new Vector2(53, 53);
                stackTextComponent.color = stackColor;

                if(stackTextFont != null)
                {
                    stackTextComponent.font = stackTextFont;
                }

                //Drawing inventory 

                inventory.cellImage = imageObj.GetComponent<Image>();
                inventory.DrawPreview();

                CleanUp(); CleanUp(); CleanUp(); CleanUp();
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        if(tabIndex == 1)
        {
            inventory = InventoryClass1.Instance;

            if(inventory == null)
            {
                EditorGUILayout.HelpBox("Stworz inventory pierw na scenie", MessageType.Error);
            }
            else
            {
                GUILayout.BeginVertical("GroupBox");

                lootSizeX = EditorGUILayout.IntField("Loot Horizontal size", lootSizeX);
                lootSizeY = EditorGUILayout.IntField("Loot Horizontal size", lootSizeY);

                if(GUILayout.Button("Generate Loot Window"))
                {
                    var cellHolder = Instantiate(new GameObject());
                    cellHolder.transform.SetParent(inventoryTransform);
                    cellHolder.name = "Loot window";

                    var cellHolderImage = cellHolder.AddComponent<Image>();
                    cellHolderImage.rectTransform.sizeDelta = new Vector2((cellRectSize + cellSpacing) * lootSizeX, (cellRectSize + cellSpacing) * lootSizeY);
                    cellHolderImage.rectTransform.anchoredPosition = Vector2.zero;
                    cellHolderImage.color = inventoryBackgroundColor;

                    cellHolderImage.rectTransform.anchoredPosition = new Vector2(lootRectSize.x, 0);

                    inventory.lootPanel = cellHolder.GetComponent<RectTransform>();
                    inventory.lootRow = lootSizeX;
                    inventory.lootColumn = lootSizeY;

                    inventory.DrawPreview();
                    CleanUp();
                }
            }
        }
        if(tabIndex == 2)
        {
            if(inventory == null)
            {
                EditorGUILayout.HelpBox("Stworz inventory pierw na scenie", MessageType.Error);
            }
            else
            {
                equipmentPanelsCount = EditorGUILayout.IntSlider("Equipment panels count", equipmentPanelsCount, 1, 20);

                if(currentEquipmentPanelsCount != equipmentPanelsCount)
                {
                    equipmentPanelX = new int[equipmentPanelsCount];
                    equipmentPanelY = new int[equipmentPanelsCount];

                    currentEquipmentPanelsCount = equipmentPanelsCount;
                }

                for (int i = 0; i < equipmentPanelsCount; i++)
                {
                    GUILayout.BeginVertical("HelpBox");

                    GUILayout.Label("");

                    equipmentPanelX[i] = EditorGUILayout.IntField("Equiment Horizontal size", equipmentPanelX[i]);

                    if(equipmentPanelX[i] == 0)
                    {
                        equipmentPanelX[i] = 1;
                    }

                    equipmentPanelY[i] = EditorGUILayout.IntField("Equiment Vertical size", equipmentPanelY[i]);

                    if (equipmentPanelY[i] == 0)
                    {
                        equipmentPanelY[i] = 1;
                    }

                    GUILayout.EndVertical();
                }

            }
        }
    }

    private void CleanUp()
    {
        var garbage = GameObject.Find("New Game Object");

        DestroyImmediate(garbage);
    }
}
