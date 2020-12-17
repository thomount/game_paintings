using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    private Rigidbody body;
    private float inertia;
    private int jumpCount = 0;
    public float jumpV = 300;
    public int jumpLimit = 2;
    private int moved = 0;
    private int jumpState = 0;
    private bool lastjumpState = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        inertia = 0;
        jumpState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moved == 0) Move(0); else moved -= 1;
        // TODO if on ground set jumpCount to 0
        bool hit = Physics.Raycast(transform.position, new Vector3(0, -1, 0), 1.01f, LayerMask.GetMask("Wall"));
        //Debug.Log(hit);
        if (hit)
        {
            Debug.Log("Ground");
            jumpCount = 0;
        }
        else {
            Debug.Log("Float");
            jumpCount = Mathf.Max(jumpCount, 1);
        }


        var pos = this.transform.position;
        var st_pos = GameObject.Find("Main Camera").transform.position;
        var dpos = pos - st_pos;
        dpos.z = 0;
        GameObject.Find("Main Camera").transform.Translate(dpos);
    }

    // actions
    public void Move(float x) {
        float act_v = (inertia * 0.3f + x * 0.7f);

        //Debug.Log(act_v);
        body.velocity = new Vector3(act_v, body.velocity.y, body.velocity.z);
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
            body.velocity = new Vector3(body.velocity.x, (jumpCount == 1) ? 5 : 8, body.velocity.z);
            //body.AddForce(new Vector3(0, jumpV));
            jumpCount++;
        }
        else if (todo == 1)
        {

        }
        else if (todo == 2) 
        {
            jumpState = 0;
            body.velocity = new Vector3(body.velocity.x, Mathf.Min(0, body.velocity.y), body.velocity.z);
        }

    }



}
