using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private PlayerInputController _playerInputController;
    private PlayerAttributeHandler _playerAttributeHandler;

    void Start()
    {
        _playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerAttributeHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();

        //playerInputController.inventory += Toggle;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
}
