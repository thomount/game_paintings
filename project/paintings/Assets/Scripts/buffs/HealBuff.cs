using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : Buff
{
    private float value;
    public HealBuff(int _v, Role _r) : base(_r, -1)
    {
        value = _v;
    }

    protected override void effect()
    {
        // act to owner
        owner.hp += value;
        DamageEffect.create(2, Mathf.FloorToInt(value), 0, owner.gameObject.transform.position);
    }
}
