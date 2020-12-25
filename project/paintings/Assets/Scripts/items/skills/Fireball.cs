using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public int value;
    public override void init(Role user, Status _stat, int level)
    {
        base.init(user, _stat, level);
        value = level * 5 + 10;

        sound.clip = Resources.Load<AudioClip>("music/skills/heal_skill");
        cost = 20;
        p1 = 0.5f;
        p2 = 0;
        p3 = 0.5f;
        cold = 10;
        type = 3;
        icon = Resources.Load<Texture2D>("itemsIcon/heal");
    }

    public override void effect()
    {
        base.effect();
        stat.AddBuff(new HealBuff(value, role));
        Debug.Log("mp = " + stat.mp.ToString() + "/" + stat.mp_max.ToString());
    }
}
