using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatLabel;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private PlayerInputController _controller;
    private EquippableItemData _selectedItem;

    private int _selectedItemIdx = 0;
    private int _curEquipIdx;

    private void Awake()
    {
        _controller = GameObject.FindWithTag(Define.PlayerTag).GetComponent<PlayerInputController>();
    }

    void Start()
    {
        _controller.OnTabEvent += Toggle;

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

    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatLabel.gameObject.SetActive(false);
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

    public void AddItem(EquippableItemData data)
    {
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            UpdateUI();
            return;
        }

        ThrowItem(data);
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void SelectedItem(int idx)
    {
        if (slots[idx].item == null)
        {
            return;
        }

        _selectedItem = slots[idx].item;
        _selectedItemIdx = idx;

        selectedItemName.text = _selectedItem.itemName;
        selectedItemDescription.text = _selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        selectedStatLabel.gameObject.SetActive(true);
        selectedStatName.text += _selectedItem.type.ToString();
        selectedStatValue.text += _selectedItem.increaseValue.ToString();

        equipButton.SetActive(!slots[idx].equipped);
        unEquipButton.SetActive(slots[idx].equipped);
        dropButton.SetActive(true);
    }

    // TODO: 플레이어 스텟 변경 추가
    public void OnEquipButton()
    {
        if (slots[_curEquipIdx].equipped)
        {
            UnEquip(_curEquipIdx);
        }

        slots[_selectedItemIdx].equipped = true;
        _curEquipIdx = _selectedItemIdx;
        UpdateUI();

        SelectedItem(_selectedItemIdx);
    }

    public void OnUnEquipButton()
    {
        UnEquip(_selectedItemIdx);
    }

    // TODO: 플레이어 스텟 변경 추가
    private void UnEquip(int idx)
    {
        slots[idx].equipped = false;
        UpdateUI();

        if (_selectedItemIdx == idx)
        {
            SelectedItem(_selectedItemIdx);
        }
    }

    public void OnDropButton()
    {
        ThrowItem(_selectedItem);
        RemoveSelectedItem();
    }

    private void ThrowItem(EquippableItemData data)
    {
        Instantiate(data.itemPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    private void RemoveSelectedItem()
    {
        _selectedItem = null;
        slots[_selectedItemIdx].item = null;
        _selectedItemIdx = -1;

        ClearSelectedItemWindow();
        UpdateUI();
    }
}
