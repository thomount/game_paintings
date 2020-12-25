using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDamage : Buff
{
    private float value;
    private int crt = 0;
    public PhysicsDamage(int _v, int _c, Role _r) : base(_r, -1)
    {
        value = _v;
        crt = _c;
    }

    protected override void effect()
    {
        // act to owner
        float dmg = Mathf.FloorToInt(value * owner.im * owner.ph_im);
        float stop = Mathf.Min(owner.shield, dmg);
        dmg -= stop;
        owner.shield -= stop;
        owner.hp -= dmg;
        Debug.Log(owner.hp.ToString() + "Physics");

        DamageEffect.create(0, Mathf.FloorToInt(dmg), crt, owner.gameObject.transform.position);
    }
}
