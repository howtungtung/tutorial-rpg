using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{
    public class InventoryEntry
    {
        public int count;
        public int itemID;
    }

    public InventoryEntry[] entries = new InventoryEntry[24];
    private CharacterData owner;

    public void Init(CharacterData owner)
    {
        this.owner = owner;
    }

    public void AddItem(int itemID)
    {
        bool found = false;
        int firstEmpty = -1;
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] == null)
            {
                if (firstEmpty == -1)
                    firstEmpty = i;
            }
            else if (entries[i].itemID == itemID)
            {
                entries[i].count++;
                found = true;
            }
        }
        if (!found && firstEmpty != -1)
        {
            InventoryEntry entry = new InventoryEntry();
            entry.itemID = itemID;
            entry.count = 1;
            entries[firstEmpty] = entry;
        }
    }

    public bool UseItem(InventoryEntry entry)
    {
        Item item = ItemCollection.instance.GetItem(entry.itemID);
        if (item.UsedBy(owner))
        {
            entry.count--;
            if (entry.count <= 0)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    if (entries[i] == entry)
                    {
                        entries[i] = null;
                        break;
                    }
                }
            }
            return true;
        }
        return false;
    }
}
