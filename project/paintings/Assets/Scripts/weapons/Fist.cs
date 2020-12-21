using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon
{
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
        obj.size = new Vector2(0.3f, 0.3f);

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
        if (last.y > 0)
            return new Vector2Int(last.x, last.y - 1);
        if (last.x == 1) { 
            return new Vector2Int(2, 10);
        }
        if (last.x == 2) { 
            return new Vector2Int(0, 0);
        }
        return new Vector2Int(0, 0);
    }
}
