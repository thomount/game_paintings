using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    private Rigidbody2D body;
    private float inertia;
    private int jumpCount = 0;
    public float jumpV = 300;
    public int jumpLimit = 2;
    private int moved = 0;
    private int jumpState = 0;
    private bool lastjumpState = false;
    private int facing = 1;
    private Animator anim;

    // animate state
    private int move = 0;
    private int ground = 1;
    private Collider2D cld;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        inertia = 0;
        jumpState = 0;
        anim = GetComponent<Animator>();
        cld = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moved == 0) Move(0); else moved -= 1;
        // if on ground set jumpCount to 0
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
        //Debug.Log(transform.position - (cld.bounds.center - cld.bounds.extents));


        var pos = this.transform.position;
        var st_pos = GameObject.Find("Main Camera").transform.position;
        var dpos = pos - st_pos;
        dpos.z = 0;
        GameObject.Find("Main Camera").transform.Translate(dpos);
        GameObject.Find("scenery").transform.Translate(dpos * 0.5f);
        GameObject.Find("HeadLight").transform.position = this.transform.position + new Vector3(0, 0, -5);
        //Debug.Log(body.velocity);
        if (body.velocity.x * facing < 0) {
            facing = -facing;
            flip();
        }



        // animator update
        anim.SetInteger("ground", ground);
        anim.SetInteger("move", move);
        //Debug.Log("ground = " + ground.ToString() + " move = " + move.ToString());
    }

    // actions
    public void Move(float x) {
        if (Mathf.Abs(x) > 1e-3f || Mathf.Abs(body.velocity.x) > 1e-3f) move = 1; else move = 0;
        // TODO 贴墙时不能移动
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
        moved = 5;

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
            body.velocity = new Vector2(body.velocity.x, (jumpCount == 1) ? 5 : 8);
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

}
