using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_fist : Attack
{
    public int value;
    public override void init(Role user, Status _stat, int level, int id, List<int> param)
    {
        base.init(user, _stat, level, id, param);
        value = level * 5 + 10;

        sound.clip = Resources.Load<AudioClip>("music/weapons/fist_a");
        sound.volume = 0.2f;
        cost = 0;
        p1 = 0.15f;
        p2 = 0;
        p3 = 0.45f;
        cold = 0.2f;
        type = 1+(id-4);
        icon = null;
    }

    public override void effect()
    {
        base.effect();
        role.weapon[param[0]].get_hit(role.attack_layer);
    }
    public override bool use()
    {
        bool ret = base.use();
        if (ret)
            role.anim.SetTrigger("attack");
        return ret;
    }
}
