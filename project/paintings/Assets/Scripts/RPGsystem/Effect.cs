using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public float endTime = -1;
    public Vector3 v;
    public List<SpriteRenderer> renders;
    public float alpha_v = 0;

    public void init(Vector3 start, Vector3 _v, float last_time, List<SpriteRenderer> _renders, float alpha_spd) {
        v = _v;
        transform.position = start;
        endTime = Time.time + last_time;
        renders = _renders;
        alpha_v = alpha_spd;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (endTime < 0) return;
        transform.Translate(v*Time.deltaTime);
        //Color color = render.material.color;
        //color.a -= alpha_v * Time.deltaTime;
        //render.material.color = color;
        foreach (var render in renders) {
            Color color = render.material.color;
            color.a -= alpha_v * Time.deltaTime;
            render.material.color = color;
        }
        
        if (Time.time > endTime) Destroy(gameObject);
    }
}
