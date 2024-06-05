using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public UIInventory inventory;

    public EquippableItemData item;
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
        equipText.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        _outline.enabled = equipped;
        equipText.enabled = equipped;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        equipText.gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        inventory.SelectedItem(index);
    }
}
