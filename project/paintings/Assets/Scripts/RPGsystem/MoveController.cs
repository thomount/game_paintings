using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public int inited = -1;
    public Vector2 pos, vel, acc;
    public float life_time, end_time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inited == -1) return;
        if (inited == 0) {
            //transform.position = pos;
            end_time = Time.time + life_time;
            inited = 1;
            //Debug.Log(transform.position);
        } else {
            transform.Translate(vel * Time.deltaTime);
            vel += acc * Time.deltaTime;
            if (Time.time > end_time)
            {
                //Debug.Log("Life over");
                Destroy(gameObject);
            }
        }
    }
    public void init(Vector2 _pos, Vector2 _vel, Vector2 _acc, float _life_time) {
        pos = _pos;
        transform.position = pos;
        vel = _vel;
        acc = _acc;
        inited = -1;
        life_time = _life_time;

    }
    public void active() { inited = 0; }
}
