using System.Collections;
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
    private int attacked = 0;       // 攻击开始信号
    private int attack_mode = 0;    // 攻击模式
    private int skill = 0;          // 技能时间
    private int hard = 0;           // 霸体时间

    // weapon state
    private Weapon[] weapon = new Weapon[2];

    // attack state
    private Vector2Int attack_state;
    private int attack_signal = 0;
    private int combo_last_time = 0;
    private int combos = 0;
    private int active_weapon = 0;

    private LayerMask attack_layer = 0;
    private LayerMask ob_layer = 0;

    private List<Vector3> force_list;
    // Start is called before the first frame update

    // TODO Bag system
    //private Bag bag = null;
    
    void Start()
    {
        //gameObject.AddComponent<AI_Random>();


        body = GetComponent<Rigidbody2D>();
        inertia = 0;
        jumpState = 0;
        anim = null;
        cld = GetComponent<Collider2D>();
        force_list = new List<Vector3>();
        control_flag = 0;
        anim = gameObject.GetComponent<Animator>();

        attack_state = new Vector2Int(0, 0);

        var obj = GameObject.Find("Weapon");
        obj.layer = LayerMask.NameToLayer("Weapon");
        var fist = obj.AddComponent<Fist>();
        fist.set_owner(gameObject);
        weapon[0] = fist;
        weapon[1] = weapon[0];
        obj.transform.parent = gameObject.transform;
        obj.transform.position = obj.transform.parent.position;
    }

    public void set_type(string s) {
        if (s == "enemy") {
            gameObject.AddComponent<AI_Random>();
        }
        if (s == "hero") {
            jumpV1 = 8;
            jumpV2 = 6;
            jumpLimit = 1;
            move_v = 5;
            //gameObject.AddComponent<Animator>();
            //anim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Assets/Sprites/stickman/Hero.controller");
            attack_layer = LayerMask.GetMask("Enemy");
            ob_layer = LayerMask.GetMask("Enemy", "Wall");
        }
    }
    // Update is called once per frame
    void Update()
    {
        control_update();

        attack_update();

        move_update();

        ground_update();

        background_update();

        animate_update();
    }

    void control_update() {
        if (control_flag > 0) control_flag --;
        if (hard > 0) hard--;
    }

    void receive_force(Vector3 vec, int control_time) {
        if (hard == 0) {
            force_list.Add(vec);
            if (control_flag != -1) {
                control_flag = Mathf.Max(control_flag, control_time); 
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
                body.AddForce(force);
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
        // 0 : 无攻击 1: 前摇(受击打断)  2：攻击后摇(受击打断/技能打断) 3：combo等待期
        if (attacked > 0) attacked--;
        if (attack_signal > 0 && active_weapon == 0 && weapon[attack_signal - 1] != null)
        {
            active_weapon = attack_signal;
            attacked = 20;
            attack_mode = weapon[active_weapon - 1].mode;
        }
        if (active_weapon != 0)
        {
            if (attack_state.x == 1 && attack_state.y == 0) {
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

        Debug.Log(active_weapon.ToString() + " " + attack_state.ToString());
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
