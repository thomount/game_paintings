using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Role
{
    // Start is called before the first frame update
    Transform filled = null;

    protected override void Start()
    {
        base.Start();
        type = "enemy";
        jumpV1 = 8;
        jumpV2 = 6;
        jumpLimit = 1;
        move_v = 3;
        //gameObject.AddComponent<Animator>();
        //anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Assets/Sprites/stickman/Hero.controller");
        attack_layer = LayerMask.GetMask("Player");
        ob_layer = LayerMask.GetMask("Player", "Wall");
        set_layers("Enemy");
        sound_run.clip = Resources.Load<AudioClip>("music/stickman/run");
        sound_run.volume = 0.6f;
        sound_run.loop = true;

        // add default weapon
        //Debug.Log(transform.GetChild(0).gameObject.name);
        wObj_L = transform.GetChild(0).Find("left arm").GetChild(0).GetChild(0).gameObject;
        wObj_R = transform.GetChild(0).Find("right arm").GetChild(0).GetChild(0).gameObject;
        wObj_L.layer = LayerMask.NameToLayer("Weapon");
        Fist fist_l = wObj_L.AddComponent<Fist>();
        fist_l.set_owner(gameObject);
        weapon[0] = fist_l;
        //wObj_L.transform.parent = gameObject.transform;
        //wObj_L.transform.position = transform.position;
        weapon[0].onEquip(0);

        wObj_R.layer = LayerMask.NameToLayer("Weapon");
        Fist fist_r = wObj_R.AddComponent<Fist>();
        fist_r.set_owner(gameObject);
        weapon[1] = fist_r;
        //wObj_R.transform.parent = gameObject.transform;
        //wObj_R.transform.position = transform.position;
        weapon[1].onEquip(1);


        gameObject.AddComponent<AI_Random>();

        stat.hp_raw = 100;
        stat.mp_raw = 100;
        stat.hpp_raw = 1;
        stat.mpp_raw = 1;
        stat.atk_raw = 10;
        stat.mtk_raw = 10;
        stat.Init();

        var _bar = Instantiate(Resources.Load<GameObject>("guis/BloodBar"));
        var offset = new Vector3(-0.8f, 1.5f, 0);
        _bar.transform.localScale = new Vector3(1.6f/8, 0.2f/8, 1);
        filled = _bar.transform.GetChild(0).gameObject.transform;
        _bar.transform.position = transform.position + offset;
        _bar.transform.parent = transform;
        bar = _bar.transform;

    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (filled == null) return;
        filled.localScale = new Vector3(stat.hp/stat.hp_max, 1, 1);
    }
}
