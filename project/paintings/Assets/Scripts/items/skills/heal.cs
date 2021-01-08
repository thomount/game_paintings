using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    public int value;
    public override void init(Role user, Status _stat, int level, int id, List<int> param)
    {
        base.init(user, _stat, level, id, param);
        value = level * 5 + 10;

        sound.clip = Resources.Load<AudioClip>("music/skills/heal_skill");
        cost = 20;
        p1 = 0.5f;
        p2 = 0;
        p3 = 0.5f;
        cold = 10;
        type = 1001;
        icon = Resources.Load<Texture2D>("itemsIcon/heal");
    }

    public override void effect()
    {
        base.effect();
        // 特效
        stat.AddBuff(new HealBuff(value, role));
        //Debug.Log("mp = " + stat.mp.ToString()+"/"+stat.mp_max.ToString());
    }
    public override void start_effect()
    {
        // TODO 起手特效
        base.start_effect();
    }
}
