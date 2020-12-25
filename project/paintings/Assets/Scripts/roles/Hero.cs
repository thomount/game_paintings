using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Role
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Debug.Log("new Hero");
        type = "hero";
        jumpV1 = 8;
        jumpV2 = 6;
        jumpLimit = 1;
        move_v = 5;
        //gameObject.AddComponent<Animator>();
        //anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Assets/Sprites/stickman/Hero.controller");
        attack_layer = LayerMask.GetMask("Enemy");
        ob_layer = LayerMask.GetMask("Enemy", "Wall");
        set_layers("Player");
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

        gameObject.AddComponent<InputHandler>();

        stat.hp_raw = 100;
        stat.mp_raw = 100;
        stat.hpp_raw = 1;
        stat.mpp_raw = 1;
        stat.atk_raw = 10;
        stat.mtk_raw = 10;
        stat.Init();

        skill[0] = gameObject.AddComponent<Heal>();
        skill[0].init(this, stat, 0);
        skill[1] = gameObject.AddComponent<Fireball>();
        skill[1].init(this, stat, 0);

    }   

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        background_update();
    }
}
