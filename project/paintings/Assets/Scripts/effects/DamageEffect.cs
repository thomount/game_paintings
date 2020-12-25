using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageEffect
{
    private static Sprite[] nums = null;
    private static Sprite[] backs = null;
    private static void Init() {
        Object[] icons = Resources.LoadAll("effects/damages");
        nums = new Sprite[10];
        backs = new Sprite[5];
        for (int i = 0; i < 10; i++) nums[i] = (Sprite)icons[i + 1];
        for (int i = 0; i < 5; i++) backs[i] = (Sprite)icons[i + 11];
    }
    public static void create(int Type, int value, int crt, Vector3 pos) {
        if (nums == null) Init();

        var ef = GameObject.Instantiate(Resources.Load<GameObject>("effects/Damage"));
        var back_obj = ef.transform.GetChild(1).gameObject;
        var back_render = back_obj.AddComponent<SpriteRenderer>();
        var front_obj = ef.transform.GetChild(0).gameObject;
        var ret = new List<SpriteRenderer>();
        Sprite back = null;
        if (Type == 0)
        {
            back = backs[(crt == 1) ? 1 : 0];
        }
        else if (Type == 1)
        {
            back = backs[(crt == 1) ? 3 : 2];
        }
        else if (Type == 2) { 
            back = backs[4];
        }


        back_render.sortingOrder = 5;
        back_render.sprite = back;
        back_render.spriteSortPoint = SpriteSortPoint.Pivot;
        ret.Add(back_render);

        List<int> front = new List<int>();
        if (value == 0) front.Add(0); else {
            while (value > 0) { front.Insert(0, value % 10); value /= 10; }
        }
        int len = front.Count;
        back_obj.transform.localScale = new Vector3((len + 1) / 3, 1.2f, 1);
        GameObject pat = front_obj.transform.GetChild(0).gameObject;
        for (int i = 0; i < len; i++) {
            var _pat = GameObject.Instantiate(pat);
            var pat_render = _pat.AddComponent<SpriteRenderer>();
            pat_render.sortingOrder = 6;
            pat_render.sprite = nums[front[i]];
            pat_render.spriteSortPoint = SpriteSortPoint.Pivot;
            ret.Add(pat_render);
            _pat.transform.position = new Vector3(i - len / 2.0f+0.5f, 0, 0);
            _pat.transform.parent = front_obj.transform;
        }

        ef.transform.localScale = new Vector3(0.5f, 0.5f, 1);



        var eff = ef.AddComponent<Effect>();
        eff.init(pos, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.4f, 1.0f), 0), 1, ret, 1);

    }

}
