using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // raw status
    public float hp_raw = 0;
    public float hpp_raw = 0;
    public float mp_raw = 0;
    public float mpp_raw = 0;
    public float atk_raw = 0;
    public float mtk_raw = 0;
    public float spd_raw = 1;
    public float aspd_raw = 1;
    public float resist_raw = 1;
    public float ph_im_raw = 1;
    public float mg_im_raw = 1;
    public int init_finish = 0; //初始化完成
    public int alive = 1;
    // ability
    public int art = 0;
    public int buster = 0;
    public int quick = 0;

    // ability const
    public float buster_to_hp = 10;
    public float buster_to_atk = 2;
    public float buster_to_hpp = 0.3f;
    public float buster_to_resist = 0.97f;
    public float buster_to_phim = 0.98f;

    public float quick_to_atk = 1;
    public float quick_to_crt = 0.98f;
    public float quick_to_crtdmg = 0.05f;
    public float quick_to_spd = 1.02f;
    public float quick_to_aspd = 1.02f;

    public float art_to_mp = 12;
    public float art_to_mtk = 3;
    public float art_to_mgim = 0.98f;
    public float art_to_mpp = 0.5f;

    //actual status

    public float hp_max = 0;  //最大生命
    public float hp = 0;      //生命
    public float hpp = 0;     //回蓝
    public float mp_max = 0;  //最大魔法
    public float mp = 0;      //魔法
    public float mpp = 0;     //回法
    public float atk = 0;     //物理攻击
    public float mtk = 0;     //魔法攻击

    public float spd = 1;   //速度加成
    public float aspd = 1;  //攻击速度
    public float resist = 1;    //韧性
    public float ph_im = 1;     //物理免伤
    public float mg_im = 1;     //法术免伤
    public float im = 1;        //免伤
    public float crt = 0.05f;       //暴击
    public float crt_dmg = 1;   //爆伤
    public float shield = 0;      //护盾
    public int hard = 0;        //霸体
    public float absorb = 0;    //吸血

    protected GameObject owner = null;
    protected List<Buff> buff_list = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        owner = gameObject;
        buff_list = new List<Buff>();

        hp_max = hp_raw;
        hp = hp_max;

        mp_max = mp_raw;
        mp = mp_max;

        shield = 0;
        alive = 1;

        init_finish = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (owner == null || init_finish == 0 || alive == 0) return;  //未初始化完成/死了
        // raw (ability)

        hp_max = (hp_raw + buster * buster_to_hp);
        mp_max = (mp_raw + art * art_to_mp);
        hpp = (hpp_raw + buster * buster_to_hpp);
        mpp = (mpp_raw + art * art_to_mpp);

        if (hp > 0) hp = Mathf.Min(hp + hpp*Time.deltaTime, hp_max); else hp = 0;
        mp = Mathf.Min(Mathf.Max(mp, 0) + mpp * Time.deltaTime, mp_max);

        atk = (atk_raw + buster * buster_to_atk + quick * quick_to_atk);
        mtk = (mtk_raw + art * art_to_mtk);

        spd = spd_raw * Mathf.Pow(quick_to_spd, quick);
        aspd = aspd_raw * Mathf.Pow(quick_to_aspd, quick);
        resist = resist_raw * Mathf.Pow(buster_to_resist, buster);
        ph_im = ph_im_raw * Mathf.Pow(buster_to_phim, buster);
        mg_im = mg_im_raw * Mathf.Pow(art_to_mgim, art);
        im = 1;
        crt = 1 - Mathf.Pow(quick_to_crt, quick);
        crt_dmg = 1 + quick_to_crtdmg * quick;

        hard = 0;
        absorb = 0;
        shield = Mathf.Max(0, shield);

        //Debug.Log(owner.name + " : hp = " + hp.ToString() + " mp = " + mp.ToString());
        // count buffs
        
        for (int i = 0; i < buff_list.Count; i++) {
            buff_list[i].manual_update();
            //Debug.Log(buff_list[i].isOver());
            if (buff_list[i].isOver()) { buff_list.RemoveAt(i); }
            if (hp <= 0) owner.GetComponent<Role>().dead();
        }

        // count weapons effect
        for (int i = 0; i < 2; i++) {
            if (owner.GetComponent<Role>().weapon[i] != null) {
                owner.GetComponent<Role>().weapon[i].passive_effect();
                if (hp <= 0) owner.GetComponent<Role>().dead();
            }
        }

    
    }

    public void AddBuff(Buff buff) {
        buff_list.Add(buff);
    }
}
