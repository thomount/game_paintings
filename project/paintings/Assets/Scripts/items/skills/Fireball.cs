using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill
{
    public int value;
    public override void init(Role user, Status _stat, int level, int id, List<int> param)
    {
        base.init(user, _stat, level, id, param);
        value = level * 5 + 10;

        sound.clip = null;
        cost = 10;
        p1 = 0.3f;
        p2 = 0;
        p3 = 0.2f;
        cold = 5;
        type = 1002;
        icon = Resources.Load<Texture2D>("itemsIcon/fireball");
    }

    public override void effect()
    {
        base.effect();
        var bullet = BulletShooter.gen_bullet("fireball", role.transform.position, new Vector2(role.facing * 10, 0.5f), new Vector2(0, -1), 10, "bullets/fireball");
        var script = bullet.AddComponent<bullet_fireball>();
        bullet.transform.localScale = new Vector3(role.facing, 1, 1);
        script.init(role, level);
        script.active();
    }

    public override void start_effect()
    {
        // TODO 起手特效
        base.start_effect();
    }
}
