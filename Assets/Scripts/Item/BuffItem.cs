using UnityEngine;

public class BuffItem : ItemObject
{
    public BuffItemData buffItemData;

    public override string GetPrompt()
    {
        return $"{buffItemData.itemName}\n{buffItemData.description}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerAttributeHandler attributeHandler))
        {
            attributeHandler.OnBuff(buffItemData);
            Destroy(gameObject);
        }
    }
}
