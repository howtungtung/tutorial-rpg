using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    public Item[] itemTable;
    public static ItemCollection instance;
    private void Awake()
    {
        instance = this;
    }

    public Item GetItem(int itemID)
    {
        for (int i = 0; i < itemTable.Length; i++)
        {
            if (itemTable[i].id == itemID)
            {
                return itemTable[i];
            }
        }
        return null;
    }
}
