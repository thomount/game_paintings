using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : CollectiveItem
{
    // Start is called before the first frame update
    public Role role;
    public Status stat;
    public int inited = 0;

    public AudioSource sound;
    public float p1, p2, p3, cold;
    public int cost;
    public int type;    //1 attack 2 def 3 bless
    public int level = 0;

    
    public float p1_t, p2_t, p3_t, cold_t;
    public int current_state = 0;
    public int isover = 1;
    public int id = -1;

    public List<int> param = null;

    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (current_state == 0)
            return;
        if (Time.time > p1_t) current_state = 2;
        if (Time.time > p2_t && isover == 0) {
            isover = 1;
            current_state = 3;
            cold_t = Time.time + cold;
            effect();
        }
        if (Time.time > p3_t) current_state = 0;
    }

    public virtual void init(Role user, Status _stat, int _level, int _id, List<int> param) {   //初始化
        role = user;
        stat = _stat;
        inited = 1;
        id = _id;
        sound = role.sound_skill[id];
        sound.clip = null;
        sound.loop = false;
        this.param = param;
        level = _level;
    }

    public virtual bool isUsing() {         //是否正在使用
        return current_state != 0;
    }

    public virtual bool canUse()            //是否可以使用，被控？攻击硬直？冷却？蓝？沉默？
    {
        if (inited == 0 || current_state != 0 || role.control_flag != 0 || (role.skill_state == 1 || role.skill_state == 2) || (Time.time < cold_t) || (stat.mp < cost)) {
            return false;
        }
        return true;
    }

    public override bool use() {             //使用,打断攻击
        if (!canUse()) return false;

        //打断攻击
        //role.active_weapon = attack_side;
        //role.attack_mode = attack_mode;
        if (role.skill_now != -1)
            role.skill[role.skill_now].kill();
        stat.mp = Mathf.Max(0, stat.mp - cost);
        p1_t = Time.time + p1;
        p2_t = p1_t + p2;
        p3_t = p2_t + p3;
        isover = 0;
        current_state = 1;
        start_effect();
        return true;
    }
    public virtual void start_effect() {
        sound.Play();
    }
    public virtual void kill() {            //结束使用
        current_state = 0;
    }
    public virtual int get_state() {        //获取技能使用状态，0、1、2、3
        return current_state;
    }

    public virtual void effect() {
    }

}
