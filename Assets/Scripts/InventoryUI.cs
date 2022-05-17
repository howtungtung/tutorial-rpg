using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void Load(CharacterData target)
    {
        foreach (var item in target.inventory.entries)
        {
            if (item != null)
                Debug.Log(item.itemID);
        }
    }
}
