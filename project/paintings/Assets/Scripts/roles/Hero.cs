using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Role
{
    // Start is called before the first frame update
    //public GameObject wObj_L, wObj_R;
    public int weapon_choice = 0;
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

        // add default weapon
        //Debug.Log(transform.GetChild(0).gameObject.name);
        wObj_L = transform.GetChild(0).Find("left arm").GetChild(0).GetChild(0).gameObject;
        wObj_R = transform.GetChild(0).Find("right arm").GetChild(0).GetChild(0).gameObject;
        wObj_L.layer = LayerMask.NameToLayer("Weapon");
        var fist_l = wObj_L.AddComponent<Sword>();
        fist_l.set_owner(gameObject);
        weapon[0] = fist_l;
        //wObj_L.transform.parent = gameObject.transform;
        //wObj_L.transform.position = transform.position;
        weapon[0].onEquip(0);

        wObj_R.layer = LayerMask.NameToLayer("Weapon");
        var fist_r = wObj_R.AddComponent<Fist>();
        fist_r.set_owner(gameObject);
        weapon[1] = fist_r;
        //wObj_R.transform.parent = gameObject.transform;
        //wObj_R.transform.position = transform.position;
        weapon[1].onEquip(1);


        gameObject.AddComponent<InputHandler>();

        stat.hp_raw = 100;
        stat.mp_raw = 100;
        stat.hpp_raw = 1;
        stat.mpp_raw = 1;
        stat.atk_raw = 10;
        stat.mtk_raw = 10;
        stat.Init();

        skill[0] = gameObject.AddComponent<Heal>();
        skill[0].init(this, stat, 0, 0, null);
        skill[1] = gameObject.AddComponent<Fireball>();
        skill[1].init(this, stat, 0, 1, null);


    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        background_update();
    }

    public void change_weapon() {
        weapon_choice = 1 - weapon_choice;
        Destroy(wObj_L.GetComponent<Weapon>());
        Weapon weapon_l = null;
        if (weapon_choice == 0) weapon_l = wObj_L.AddComponent<Sword>(); else weapon_l = wObj_L.AddComponent<HugeSword>();
        weapon_l.set_owner(gameObject);
        weapon[0] = weapon_l;
        //wObj_L.transform.parent = gameObject.transform;
        //wObj_L.transform.position = transform.position;
        weapon[0].onEquip(0);
    }
    public override void dead()
    {
        base.dead();

    }
}
