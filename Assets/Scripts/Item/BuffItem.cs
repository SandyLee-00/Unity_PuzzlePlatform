using UnityEngine;

public class BuffItem : ItemObject
{
    public BuffItemData buffItemData;
    public override string GetPrompt()
    {
        return $"{buffItemData.itemName}\n{buffItemData.description}";
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //}
}
