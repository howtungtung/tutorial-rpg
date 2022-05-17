using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite itemSprite;
    public string description;
    public GameObject prefab;

    public virtual bool UsedBy(CharacterData user)
    {
        return false;
    }
}
