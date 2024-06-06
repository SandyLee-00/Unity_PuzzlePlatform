using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public UIInventory inventory;

    public EquippableItemData itemData;
    public Image icon;
    public TextMeshProUGUI equipText;
    private Outline _outline;

    public int index;
    public bool equipped;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        _outline.enabled = equipped;
        equipText.gameObject.SetActive(equipped);
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = itemData.icon;

        _outline.enabled = equipped;
        equipText.gameObject.SetActive(equipped);
    }

    public void Clear()
    {
        itemData = null;
        icon.gameObject.SetActive(false);
        equipText.gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        inventory.SelectedItem(index);
    }
}
