using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Potion : Item
{
    public override bool UsedBy(CharacterData user)
    {
        user.currentStat.hp = (int)Mathf.Min(user.currentStat.hp + user.defaultStat.hp * 0.2f, user.defaultStat.hp);
        return true;
    }
}
