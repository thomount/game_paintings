using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeSword : Weapon
{
    float last_record = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sounds = gameObject.AddComponent<AudioSource>();
        //Debug.Log("Fist init");
        usage = 1;
        attP = 10;
        crt = 0;
        mode = 1;   //徒手武器
        var obj = gameObject.AddComponent<BoxCollider2D>();
        obj.isTrigger = true;
        obj.size = new Vector2(2.0f, 2.4f);
        obj.offset = new Vector2(0, -2);
        obj.enabled = false;
        force = new Vector2(5, 4);
        control_time = 30;
        // TODO change sound
        sounds.clip = Resources.Load<AudioClip>("music/weapons/hugesword_h");
        sounds.volume = 1f;
        //sounds.loop = false;

        sounds.loop = false;
        render.sprite = Resources.Load<Sprite>("equips/wp_golden_pen");
        icon_path = "equips/wp_golden_pen";
        render.transform.localScale = new Vector3(1f, 1f, 1);
        render.spriteSortPoint = SpriteSortPoint.Pivot;
        render.sortingOrder = 4;


        PhK = 1.5f;
        MgK = 0;

        DmgType = 1;

    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        //Debug.Log(transform.position);
        base.FixedUpdate();
    }
    /*
    public override Vector2Int next_Attack(Vector2Int last)
    {
        //Debug.Log("Fist attack");
        if (last.x == 0)
        {
            last_record = Time.time;
            return new Vector2Int(1, 15);
        }
        if ((Time.time - last_record)*60 < last.y)
            return last;
        last_record = Time.time;
        if (last.x == 1) { 
            return new Vector2Int(2, 0);
        }
        if (last.x == 2) { 
            return new Vector2Int(3, 30);
        }
        if (last.x == 3)
        {
            return new Vector2Int(0, 0);
        }
        return new Vector2Int(0, 0);
    }
    */
    public override void passive_effect()
    {
        //owner.stat.crt_dmg *= 1.5f;
        owner.stat.crt += 0.3f;
    }

    public override void onEquip(int _side)
    {
        base.onEquip(_side);
        owner.skill[4 + side] = owner.gameObject.AddComponent<a_hugesword>();
        List<int> param = new List<int>();
        param.Add(side);
        owner.skill[4 + side].init(owner, owner.stat, 0, 4 + side, param);
    }
    public override void unEquip()
    {
        Destroy(owner.skill[4 + side]);
    }
}