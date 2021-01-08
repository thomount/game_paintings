using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fireball : Bullet
{
    // Start is called before the first frame update
    public override void init(Role _owner, int level)
    {
        base.init(_owner, level);
        var temp = gameObject.AddComponent<CircleCollider2D>();
        //temp.transform.parent = gameObject.transform;
        temp.radius = 0.5f;
        temp.isTrigger = true;
        //Debug.Log(temp.bounds);
        collider = temp;
    }

    public override void frame_effect() { }
    public override void touch_effects(List<Collider2D> cds) {
        //Debug.Log("Touch objs");
        //Debug.Log(collider.transform.position.ToString()+collider.bounds.ToString());
        base.touch_effects(cds);
        // TODO explode effects
        Destroy(gameObject);
    }
    public override void touch_effect(GameObject obj) {
        //Debug.Log(obj.name);
        if (obj.layer == LayerMask.NameToLayer("Enemy")) { 
            Role role = obj.GetComponent<Role>();
            float Mdmg = ((1 + level) * 20 + 1.5f * owner.stat.mtk);
            int crt_flag = 0;
            if (Random.Range(0, 1.0f) < owner.stat.crt) { crt_flag = 1; Mdmg *= (1 + owner.stat.crt_dmg); }

            obj.GetComponent<Role>().stat.AddBuff(new MagicDamage(Mathf.FloorToInt(Mdmg), crt_flag, role));
        }
    }
}
