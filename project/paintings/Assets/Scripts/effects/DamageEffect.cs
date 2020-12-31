using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageEffect
{
    private static Sprite[] nums = null;
    private static Sprite[] backs = null;
    private static void Init() {
        Object[] icons = Resources.LoadAll("effects/damage_new");
        nums = new Sprite[60];
        for (int i = 0; i < 60; i++) nums[i] = (Sprite)icons[i + 1];
    }
    public static void create(int Type, int value, int crt, Vector3 pos) {
        if (nums == null) Init();

        var ef = GameObject.Instantiate(Resources.Load<GameObject>("effects/Damage"));
        var front_obj = ef.transform.GetChild(0).gameObject;
        var ret = new List<SpriteRenderer>();
        int offset = 0;
        if (Type == 0)
        {
            offset = (crt == 1) ? 3 : 1;
        }
        else if (Type == 1)
        {
            offset = (crt == 1) ? 4 : 2;
        }
        else if (Type == 2) {
            offset = 5;
        }

        List<int> front = new List<int>();
        if (value == 0) front.Add(0); else {
            while (value > 0) { front.Insert(0, value % 10); value /= 10; }
        }
        int len = front.Count;
        GameObject pat = front_obj.transform.GetChild(0).gameObject;
        for (int i = 0; i < len; i++) {
            var _pat = GameObject.Instantiate(pat);
            var pat_render = _pat.AddComponent<SpriteRenderer>();
            pat_render.sortingOrder = 6;
            pat_render.sprite = nums[offset*10+(front[i]+9)%10];
            pat_render.spriteSortPoint = SpriteSortPoint.Pivot;
            ret.Add(pat_render);
            _pat.transform.position = new Vector3(i - len / 2.0f+0.5f, 0, 0);
            _pat.transform.parent = front_obj.transform;
        }

        ef.transform.localScale = new Vector3(0.5f, 0.5f, 1);



        var eff = ef.AddComponent<Effect>();
        eff.init(pos, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(1.0f, 2.0f), 0), 1, ret, 1);

    }

}
