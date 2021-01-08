using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Skill
{
    public SpriteRenderer render = null;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (current_state == 0) {
            Color color = render.material.color;
            color.a = 0;
            render.material.color = color;
        }
    }
    public override void init(Role user, Status _stat, int _level, int _id, List<int> param)
    {
        base.init(user, _stat, _level, _id, param);
        render = role.weapon[_id - 4].gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log(render);
    }

    public override void start_effect() {
        base.start_effect();
        Color color = render.material.color;
        color.a = 1;
        render.material.color = color;
    }
}
