using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    // Start is called before the first frame update
    public float endTime = -1;
    public int type;               //good / bad / normal / sudden
    protected Status owner;
    private bool isover = false;

    public Buff(Role obj, float last_time) {
        owner = obj.stat;
        owner.AddBuff(this);
        endTime = Time.time + last_time;
    }
    public bool isOver() {
        isover = (Time.time > endTime);
        return isover;
    }
    // Update is called once per frame


    public void manual_update()
    {
        if (endTime < 0 || isover) return;        //未生效

        // 生效
        effect();
    }

    protected virtual void effect() { 
        // act to owner
    }
}
