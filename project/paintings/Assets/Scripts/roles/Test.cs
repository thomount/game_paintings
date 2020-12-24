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

        sound_attack.clip = Resources.Load<AudioClip>("music/stickman/attack");
        sound_attack.volume = 0.4f;
        sound_attack.loop = false;

        // add default weapon
        var obj = Instantiate(GameObject.Find("Weapon"));
        obj.layer = LayerMask.NameToLayer("Weapon");
        var fist = obj.AddComponent<Fist>();
        fist.set_owner(gameObject);
        weapon[0] = fist;
        weapon[1] = weapon[0];
        obj.transform.parent = gameObject.transform;
        obj.transform.position = obj.transform.parent.position;

        gameObject.AddComponent<AI_Random>();

        stat.hp_raw = 100;
        stat.mp_raw = 100;
        stat.hpp_raw = 0;
        stat.mpp_raw = 0;
        stat.atk_raw = 10;
        stat.mtk_raw = 10;
        stat.Init();

        var _bar = Instantiate<GameObject>(Resources.Load<GameObject>("others/BloodBar"));
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
