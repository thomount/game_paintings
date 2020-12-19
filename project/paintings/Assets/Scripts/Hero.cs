using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hero : MonoBehaviour
{

    // body state
    private Rigidbody2D body;
    private int facing = 1;

    // Jump const
    private int jumpCount = 0;
    private float jumpV1;
    private float jumpV2;
    private int jumpLimit = 2;

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
    private Collider2D cld;

    // weapon state
    private GameObject[] weapon = new GameObject[2];

    // attack state
    private int attack_signal = 0;
    private int attack_period = 0;
    private int ap_last_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        inertia = 0;
        jumpState = 0;
        anim = GetComponent<Animator>();
        cld = GetComponent<Collider2D>();
        jumpV1 = GameObject.Find("InputHandler").GetComponent<InputHandler>().std_jump_force1;
        jumpV2 = GameObject.Find("InputHandler").GetComponent<InputHandler>().std_jump_force2;
        jumpLimit = GameObject.Find("InputHandler").GetComponent<InputHandler>().jump_limit;


    }

    // Update is called once per frame
    void Update()
    {
        // update move state
        attack_update();

        move_update();

        ground_update();

        background_update();

        animate_update();
    }

    void background_update() {
        // camera and scenery follow
        var pos = this.transform.position;
        var st_pos = GameObject.Find("Main Camera").transform.position;
        var dpos = pos - st_pos;
        dpos.z = 0;
        GameObject.Find("Main Camera").transform.Translate(dpos);
        GameObject.Find("scenery").transform.Translate(dpos * 0.5f);
        GameObject.Find("HeadLight").transform.position = this.transform.position + new Vector3(0, 0, -5);
 
    }

    void ground_update() {

        // ground check
        var hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 10f, LayerMask.GetMask("Wall"));
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
        //Debug.Log("ground = " + ground.ToString() + " move = " + move.ToString());
    }

    void move_update() {
        float x = move_signal;
        if (Mathf.Abs(x) > 1e-3f || Mathf.Abs(body.velocity.x) > 1e-3f) {move = 1; moved = 5;} else move = 0;
        // 贴墙时不能移动
        float act_v = (inertia * 0.3f + x * 0.7f);
        var filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Wall"));
        var cds = new List<Collider2D>();
        body.OverlapCollider(filter, cds);
        bool hit_left = false, hit_right = false;
        foreach (var cd in cds) {
            //Debug.Log(cd.transform.position.x - this.transform.position.x);
            float lim = cld.bounds.extents.x + cd.GetComponent<Collider2D>().bounds.extents.x - 0.05f;
            if (cd.transform.position.x - this.transform.position.x > lim)
            {
                hit_right = true;
                //Debug.Log(cld.bounds.extents.x.ToString()+" : "+(cd.transform.position.x - this.transform.position.x).ToString());
            }
            if (cd.transform.position.x - this.transform.position.x < -lim)
            {
                hit_left = true;
                //Debug.Log(cld.bounds.extents.x.ToString() + " : " + (cd.transform.position.x - this.transform.position.x).ToString());
            }
        }
        if (hit_left) act_v = Mathf.Max(0, act_v);
        if (hit_right) act_v = Mathf.Min(0, act_v);
        //Debug.Log(act_v);
        body.velocity = new Vector2(act_v, body.velocity.y);
        inertia = act_v;
        //body.AddForce()

        if (body.velocity.x * facing < 0) {
            facing = -facing;
            flip();
        }

        move_signal = body.velocity.x*0.6f;
        if (moved > 0) moved --; else move_signal = 0;
    }

    // actions
    public void Move(float x) {
        move_signal = x;
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
       
        if (attack_signal > 0 && attack_period == 0 && weapon[attack_signal-1] != null) {
            var att_state= weapon[attack_signal-1].GetComponent<Weapon>().start_Attack(this.gameObject);

        }
        // TODO finish attack logic


        attack_signal = 0;
    }

    public void equip_weapon(GameObject _weapon, int pos) {
        if (pos == 0){
            if (_weapon.GetComponent<Weapon>().usage == 1) {
                unequip_weapon(0);
                weapon[0] = _weapon;
            }
            if (_weapon.GetComponent<Weapon>().usage == 3) {
                unequip_weapon(0);
                unequip_weapon(1);
                weapon[0] = _weapon;
                weapon[1] = _weapon;
            }

        } else {
            if (_weapon.GetComponent<Weapon>().usage == 2) {
                unequip_weapon(1);
                weapon[1] = _weapon;
            }
            if (_weapon.GetComponent<Weapon>().usage == 3) {
                unequip_weapon(0);
                unequip_weapon(1);
                weapon[0] = _weapon;
                weapon[1] = _weapon;
            }

        }
    }
    public void unequip_weapon(int pos) {
        if (weapon[pos] == null) return;
        if (weapon[pos].GetComponent<Weapon>().usage == 3) weapon[1-pos] = null;
        weapon[pos] = null;

    }
}
