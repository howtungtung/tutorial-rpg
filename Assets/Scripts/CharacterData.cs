using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : InteractableObject
{
    public string characterName;
    [System.Serializable]
    public struct Stat
    {
        public int hp;
        public int atk;
        public int def;
    }

    public Stat defaultStat;
    public Stat currentStat;

    public void Init()
    {
        currentStat = defaultStat;
    }

    public bool CanAttackTarget(CharacterData target)
    {
        if (target.currentStat.hp == 0)
            return false;
        return true;
    }

    public bool CanAttackReach(CharacterData target)
    {
        return Vector3.Distance(transform.position, target.transform.position) < 2f;
    }

    public void Attack(CharacterData target)
    {
        int damage = Mathf.Max(currentStat.atk - target.currentStat.def, 1);
        target.Damage(damage);
    }

    public void Damage(int damage)
    {
        currentStat.hp = Mathf.Max(currentStat.hp - damage, 0);
        DamageUI.instance.NewDamage(damage, transform.position);
        if (currentStat.hp == 0)
        {
            Death();
        }
    }

    public void Death()
    {

    }

}
