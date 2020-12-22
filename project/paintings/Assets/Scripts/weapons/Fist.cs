﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
    float last_record = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Fist init");
        usage = 1;
        attP = 10;
        crt = 0;
        mode = 1;   //徒手武器
        var obj = gameObject.AddComponent<BoxCollider2D>();
        obj.isTrigger = true;
        obj.size = new Vector2(1.0f, 1.0f);
        offset = new Vector2(1.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position);
    }
    public override Vector2Int next_Attack(Vector2Int last)
    {
        //Debug.Log("Fist attack");
        if (last.x == 0)
            return new Vector2Int(1, 20);
        if ((Time.time - last_record)*60 < last.y)
            return last;
        last_record = Time.time;
        if (last.x == 1) { 
            return new Vector2Int(2, 0);
        }
        if (last.x == 2) { 
            return new Vector2Int(3, 10);
        }
        if (last.x == 3)
        {
            return new Vector2Int(0, 0);
        }
        return new Vector2Int(0, 0);
    }
}
