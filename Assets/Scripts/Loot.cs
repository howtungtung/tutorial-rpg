using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : InteractableObject
{
    public int itemID;

    private void Start()
    {
        Item item = ItemCollection.instance.GetItem(itemID);
        Instantiate(item.prefab, transform);
    }

    public void Pickup(CharacterData target)
    {
        target.inventory.AddItem(itemID);
        InventoryUI.instance.Load(target);
        Destroy(gameObject);
    }
}
