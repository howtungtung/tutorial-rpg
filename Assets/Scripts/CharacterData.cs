using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CharacterData : InteractableObject
{
    public string characterName;
    [System.Serializable]
    public struct Stat
    {
        public int hp;
        public int atk;
        public int def;
        public int agi;
    }

    public Stat defaultStat;
    public Stat currentStat;
    public Action OnDamage;
    private float attackCoolDown;

    public void Init()
    {
        currentStat = defaultStat;
    }

    public bool CanAttackTarget(CharacterData target)
    {
        if (target.currentStat.hp == 0)
            return false;
        if (!CanAttackReach(target))
            return false;
        if (attackCoolDown > 0)
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

    public void AttackTriggered()
    {
        attackCoolDown = 2 - (currentStat.agi * 0.001f);
    }

    public void Damage(int damage)
    {
        currentStat.hp = Mathf.Max(currentStat.hp - damage, 0);
        DamageUI.instance.NewDamage(damage, transform.position);
        OnDamage?.Invoke();
    }

    private void Update()
    {
        if(attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
    }
}
