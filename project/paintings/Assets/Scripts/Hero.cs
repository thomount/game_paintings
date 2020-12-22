﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;


public class Hero : MonoBehaviour
{
    // camera state
    public float scene_k = 0.9f;


    // body state
    private Rigidbody2D body;
    private Collider2D cld;
    public int facing = 1;

    // Jump const
    private int jumpCount = 0;
    private float jumpV1;
    private float jumpV2;
    private float move_v;
    private int jumpLimit;

    // jump state
    private int jumpState = 0;
    private bool lastjumpState = false;

    // Move data
    private int moved = 0;
    private float inertia;
    private float move_signal = 0;

    // animate state
    private Animator anim;
    private int move = 0;
    private int ground = 1;
    private int control_flag = 0;
    private float control_end_time = 0;
    private int attacked = 0;       // 攻击开始信号
    private int attack_mode = 0;    // 攻击模式
    private int skill = 0;          // 技能时间
    private int hard = 0;           // 霸体时间

    // weapon state
    private Weapon[] weapon = new Weapon[2];

    // attack state
    public int Attack_Signal_Last_Time = 20;
    private Vector2Int attack_state;
    private int attack_signal = 0;
    private int combo_last_time = 0;
    private int combos = 0;
    private int active_weapon = 0;
    private int hitted = 0;

    private LayerMask attack_layer = 0;
    private LayerMask ob_layer = 0;

    private List<Vector2> force_list;
    private float resist = 1;
    // Start is called before the first frame update


    // basci state
    private string type;
    public bool inited = false;
    // TODO Bag system
    //private Bag bag = null;


    // sound state
    public AudioSource sound_attacked;
    public AudioSource sound_attack;
    public AudioSource sound_run;
    public AudioSource sound_jump;
    public AudioSource sound_skill;

    void Start()
    {
        //gameObject.AddComponent<AI_Random>();


        body = GetComponent<Rigidbody2D>();
        inertia = 0;
        jumpState = 0;
        anim = null;
        cld = GetComponent<Collider2D>();
        force_list = new List<Vector2>();
        control_flag = 0;
        anim = gameObject.GetComponent<Animator>();

        attack_state = new Vector2Int(0, 0);


        // add default weapon
        var obj = Instantiate(GameObject.Find("Weapon"));
        obj.layer = LayerMask.NameToLayer("Weapon");
        var fist = obj.AddComponent<Fist>();
        fist.set_owner(gameObject);
        weapon[0] = fist;
        weapon[1] = weapon[0];
        obj.transform.parent = gameObject.transform;
        obj.transform.position = obj.transform.parent.position;

        for (int i = 0; i < 5; i++)
            gameObject.AddComponent<AudioSource>();
        var sounds = gameObject.GetComponents<AudioSource>();
        sound_attack = sounds[0];
        sound_run = sounds[1];
        sound_jump = sounds[2];
        sound_skill = sounds[3];
        sound_attacked = sounds[4];
        Debug.Log(sound_attack);
        inited = true;
    }
    IEnumerator wait_for_start(string s) {
        yield return new WaitUntil(() => inited == true);
        Debug.Log(inited);
        type = s;
        if (s == "enemy")
        {
            gameObject.AddComponent<AI_Random>();
            set_layers("Enemy");
        }
        if (s == "hero")
        {
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
            

        }
        if (s == "enemy_test")
        {
            jumpV1 = 8;
            jumpV2 = 6;
            jumpLimit = 1;
            move_v = 3;
            attack_layer = LayerMask.GetMask("Player");
            ob_layer = LayerMask.GetMask("Player", "Wall");
            set_layers("Enemy");
        }

    }

    public void set_type(string s) {
        StartCoroutine(wait_for_start(s));
    }

    void set_layers(string s) {
        int target = LayerMask.NameToLayer(s);
        foreach (Transform tran in GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = target;//更改物体的Layer层
        }

    }


    // Update is called once per frame
    void Update()
    {
        control_update();

        attack_update();

        move_update();

        ground_update();

        if (type == "hero") background_update();

        animate_update();

        sound_update();
    }

    void sound_update() {
        if (ground == 1 && move == 1 && attacked == 0) {
            if (sound_run.isPlaying == false)
            {
                sound_run.Play();
            }
        }
        if ((move == 0 || ground == 0 || control_flag > 0 || attacked > 0) && sound_run.isPlaying == true) {
            sound_run.Stop();
        }
        /*
        if (hitted == 1) {
            sounds.clip = attacked_clip;
            sounds.loop = false;
            sounds.Play();
        }
        */
        //if (attacked > 0)

    }

    void control_update() {
        if (control_flag == 1) {
            if (Time.time > control_end_time) control_flag = 0;
        }
        if (hard > 0) hard--;
    }

    public void receive_force(Vector3 vec, int control_time) {
        if (hard == 0) {
            force_list.Add(vec);
            hitted = 1;
            //Debug.Log("receive force " + vec.ToString());
            if (control_flag != -1) {
                if (control_flag == 0) {
                    control_end_time = Time.time;
                    control_flag = 1;
                }
                control_end_time = Mathf.Max(control_end_time, Time.time + (control_time / 60.0f));
            }
        }
    }

    void dead() {
        control_flag = -1;
    }
    void background_update() {
        // camera and scenery follow
        var pos = this.transform.position;
        var st_pos = GameObject.Find("Main Camera").transform.position;
        var dpos = pos - st_pos;
        dpos.z = 0;
        GameObject.Find("Main Camera").transform.Translate(dpos);
        GameObject.Find("scenery").transform.Translate(dpos * scene_k);
        GameObject.Find("HeadLight").transform.position = this.transform.position + new Vector3(0, 0, -5);
 
    }

    void ground_update() {

        // ground check
        var hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 10f, ob_layer);
            //Debug.Log(hit.collider);
            //Debug.Log(hit.distance);
        if (hit.collider != null && hit.distance < (transform.position - (cld.bounds.center - cld.bounds.extents)).y+0.05)
        {
            //Debug.Log("Ground");
            jumpCount = 0;
            ground = 1;
        }
        else {
            //Debug.Log("Float");
            jumpCount = Mathf.Max(jumpCount, 1);
            ground = 0;
        }

    }

    void animate_update() {
        // animator update
        anim.SetInteger("ground", ground);
        anim.SetInteger("move", move);
        anim.SetInteger("control", control_flag);
        anim.SetInteger("attack", Mathf.Min(1, attacked));
        anim.SetInteger("attack_mode",attack_mode);
        //Debug.Log("attack = " + attacked.ToString() + " mode = " + attack_mode.ToString());
        //Debug.Log("ground = " + ground.ToString() + " move = " + move.ToString());
    }

    void move_update() {
        float x = move_signal;
        if (control_flag == 0) {
            if (Mathf.Abs(x) > 1e-3f || Mathf.Abs(body.velocity.x) > 1e-3f) {move = 1; moved = 5;} else move = 0;
            float act_v = (inertia * 0.3f + x * 0.7f);

            // 贴墙时不能移动
            var filter = new ContactFilter2D();
            filter.SetLayerMask(ob_layer);
            var cds = new List<Collider2D>();
            body.OverlapCollider(filter, cds);
            bool hit_left = false, hit_right = false;
            foreach (var cd in cds) {
                //Debug.Log(cd.transform.position.x - this.transform.position.x);
                float lim = cld.bounds.extents.x + cd.GetComponent<Collider2D>().bounds.extents.x - 0.05f;
                if (cd.transform.position.x - this.transform.position.x > lim)
                {
                    hit_right = true;
                    //Debug.Log(cd.gameObject.name);
                }
                if (cd.transform.position.x - this.transform.position.x < -lim)
                {
                    hit_left = true;
                    //Debug.Log(cd.gameObject.name);
                }
            }
            if (hit_left) act_v = Mathf.Max(0, act_v);
            if (hit_right) act_v = Mathf.Min(0, act_v);
            //Debug.Log(act_v);
            body.velocity = new Vector2(act_v, body.velocity.y);
            inertia = act_v;

        } else {
            foreach (var force in force_list) {
                body.velocity += force;
                //Debug.Log("add force" + force.ToString());
            }
        }
        force_list.Clear();

        // facing update
        if (body.velocity.x * facing < 0) {
            facing = -facing;
            flip();
        }

        move_signal = body.velocity.x*0.6f;
        if (moved > 0) moved --; else move_signal = 0;
    }

    // actions
    public void Move(int flag) {
        move_signal = move_v * flag;
    }

    public void Jump(bool state) {
        //Debug.Log("jump");
        int todo = 2;   // 0 for start jump 1 for keep jump 2 for no jump
        if (state == true)
        {
            todo = 1;
            if (jumpState == 0 && lastjumpState == false) todo = (jumpCount < jumpLimit) ? 0 : 1;
        }
        else todo = 2;
        lastjumpState = state;
        if (todo == 0)
        {
            //start jump
            jumpState = 1;
            body.velocity = new Vector2(body.velocity.x, (jumpCount == 1) ? jumpV2 : jumpV1);
            //body.AddForce(new Vector3(0, jumpV));
            jumpCount++;
        }
        else if (todo == 1)
        {

        }
        else if (todo == 2) 
        {
            jumpState = 0;
            body.velocity = new Vector2(body.velocity.x, Mathf.Min(0, body.velocity.y));
        }

    }

    public void flip() {
        Vector3 t = transform.localScale;
        t.x *= -1;
        transform.localScale = t;
    }

    public void Attack(int side) {
        attack_signal = side + 1;
    }

    void attack_update() {
        // 0 : 无攻击 1: 前摇(受击打断)  2：攻击判定 3：攻击后摇(受击打断/技能打断) 4：combo等待期
        if (attacked > 0) attacked--;
        //Debug.Log(Time.time.ToString() + attack_state);
        if (attack_signal > 0 && active_weapon == 0 && weapon[attack_signal - 1] != null)
        {
            active_weapon = attack_signal;
            attacked = Attack_Signal_Last_Time;
            attack_mode = weapon[active_weapon - 1].mode;
            sound_attack.Play();
        }
        if (active_weapon != 0)
        {
            if (attack_state.x == 2) {
                //Debug.Log("hiting");
                weapon[active_weapon - 1].get_hit(attack_layer);
            }
            attack_state = weapon[active_weapon-1].next_Attack(attack_state);
            // TODO finish attack logic

        }

        if (attack_state.x == 0) {
            active_weapon = 0;
            attack_mode = 0;
        }

        //Debug.Log(active_weapon.ToString() + " " + attack_state.ToString());
        // 


        attack_signal = 0;
    }

    public void equip_weapon(GameObject _weapon, int pos) {
        /*
        if (pos == 0){
            if (_weapon.GetComponent<Weapon>().usage == 1) {
                unequip_weapon(0);
                weapon[0] = _weapon;
                _weapon.GetComponent<Weapon>().set_owner(this.gameObject);
            }
            if (_weapon.GetComponent<Weapon>().usage == 3) {
                unequip_weapon(0);
                unequip_weapon(1);
                weapon[0] = _weapon;
                weapon[1] = _weapon;
                _weapon.GetComponent<Weapon>().set_owner(this.gameObject);
            }

        } else {
            if (_weapon.GetComponent<Weapon>().usage == 2) {
                unequip_weapon(1);
                weapon[1] = _weapon;
                _weapon.GetComponent<Weapon>().set_owner(this.gameObject);
            }
            if (_weapon.GetComponent<Weapon>().usage == 3) {
                unequip_weapon(0);
                unequip_weapon(1);
                weapon[0] = _weapon;
                weapon[1] = _weapon;
                _weapon.GetComponent<Weapon>().set_owner(this.gameObject);
            }

        }
        */
    }
    public void unequip_weapon(int pos) {
        /*
        if (weapon[pos] == null) return;
        if (weapon[pos].GetComponent<Weapon>().usage == 3) {
            weapon[1-pos].GetComponent<Weapon>().set_owner(null);
            weapon[1-pos] = null;
        }
        weapon[pos].GetComponent<Weapon>().set_owner(null);
        weapon[pos] = null;
        */
    }
}
