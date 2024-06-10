using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
    [Header("Item Info UI")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatLabel;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    public Transform slotPanel;
    public Transform dropPosition;

    private ItemSlot[] _slots;
    private EquippableItemData _selectedItem;
    private int _selectedItemIdx = 0;
    private int _curEquipIdx;

    private PlayerInputController _controller;
    private PlayerAttributeHandler _attributeHandler;

    private void Awake()
    {
        _controller = GameObject.FindWithTag(Define.PlayerTag).GetComponent<PlayerInputController>();
        _attributeHandler = GameObject.FindWithTag(Define.PlayerTag).GetComponent<PlayerAttributeHandler>();
    }

    void Start()
    {
        _controller.OnTabEvent += Toggle;

        gameObject.SetActive(false);

        _slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            _slots[i].index = i;
            _slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }
    
    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
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

    public void AddItem(EquippableItemData data)
    {
        Debug.Log("아이템 추가");
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.itemData = data;
            UpdateUI();
            return;
        }

        ThrowItem(data);
    }

    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].itemData == null)
            {
                return _slots[i];
            }
        }

        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].itemData != null)
            {
                _slots[i].Set();
            }
            else
            {
                _slots[i].Clear();
            }
        }
    }

    private void ThrowItem(EquippableItemData data)
    {
        Instantiate(data.itemPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectedItem(int idx)
    {
        if (_slots[idx].itemData == null)
        {
            return;
        }

        _selectedItem = _slots[idx].itemData;
        _selectedItemIdx = idx;

        selectedItemName.text = _selectedItem.itemName;
        selectedItemDescription.text = _selectedItem.description;
        selectedStatLabel.gameObject.SetActive(true);
        selectedStatName.text = _selectedItem.type.ToString();
        selectedStatValue.text = _selectedItem.increaseValue.ToString();

        equipButton.SetActive(!_slots[idx].equipped);
        unEquipButton.SetActive(_slots[idx].equipped);
        dropButton.SetActive(!_slots[idx].equipped); // TODO: 버리기 버튼을 비활성화하지 말고, 버리면서 장비 해제하는 쪽으로 수정
    }

    public void OnEquipButton()
    {
        if (_slots[_curEquipIdx].equipped)
        {
            _attributeHandler.ApplyItemValue(_slots[_curEquipIdx].itemData, false);
            _slots[_curEquipIdx].equipped = false;
        }

        _attributeHandler.ApplyItemValue(_selectedItem, true);
        _slots[_selectedItemIdx].equipped = true;
        _curEquipIdx = _selectedItemIdx;
        UpdateUI();

        SelectedItem(_selectedItemIdx);
    }

    public void OnUnEquipButton()
    {
        if (_slots[_selectedItemIdx].equipped)
        {
            _attributeHandler.ApplyItemValue(_slots[_selectedItemIdx].itemData, false);
            _slots[_selectedItemIdx].equipped = false;
            UpdateUI();

            SelectedItem(_selectedItemIdx);
        }
    }

    public void OnDropButton()
    {
        ThrowItem(_selectedItem);
        RemoveSelectedItem();
    }

    private void RemoveSelectedItem()
    {
        _selectedItem = null;
        _slots[_selectedItemIdx].itemData = null;
        _selectedItemIdx = -1;

        ClearSelectedItemWindow();
        UpdateUI();
    }

    public List<EquippableItemData> GetItems()
    {
        List<EquippableItemData> nowEquipItems = new List<EquippableItemData>();
        for(int i=0;i< _slots.Length;i++)
        {
            nowEquipItems.Add(_slots[i].itemData);
        }
        return nowEquipItems;
    }
}
