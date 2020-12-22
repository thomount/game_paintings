using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int usage = 0;
    public int mode = 0;
    public int attP = 0;
    public int attM = 0;
    public float crt = 0;
    public Hero owner;
    public Vector2 offset;
    public Vector2 force;
    public int control_time = 0;
    public AudioSource sounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void set_owner(GameObject _owner) {
        owner = _owner.GetComponent<Hero>();
    }

    public virtual Vector2Int next_Attack(Vector2Int last) {
        if (last.x == 0)
            return new Vector2Int(1,1);
        if (last.y > 0) 
            return new Vector2Int(last.x, last.y-1);
        return new Vector2Int(0,0);
    }
    
    // 攻击判定区域对象
    public void get_hit(int mask) {
        var obj = gameObject.GetComponent<Collider2D>();
        obj.offset = offset;
        //Debug.Log(obj.offset.ToString()+owner.facing.ToString()+(obj.bounds.center - obj.transform.position).ToString());
        var filter = new ContactFilter2D();
        filter.SetLayerMask(mask);
        //Debug.Log(mask);
        //Debug.Log(GameObject.Find("enemy").layer);
        //Debug.Log(GameObject.Find("Cube").layer);
        var cds = new List<Collider2D>();
        obj.OverlapCollider(filter, cds);
        obj.isTrigger = true;
        //Debug.Log(obj.transform.position);
        /*
        if (cds.Count == 0)
        {
            Debug.Log("Miss" + obj.bounds.center.ToString());
            
            var cube = Instantiate(GameObject.Find("Cube"));
            cube.name = "hitting place";
            cube.GetComponent<Collider2D>().isTrigger = true;
            cube.transform.position = obj.bounds.center;
            cube.transform.localScale = 2 * obj.bounds.extents;
            cube.layer = LayerMask.NameToLayer("Player");
            
        }
        */
        if (cds.Count > 0) get_effect();
        foreach (var cd in cds) {
            get_damage(cd.gameObject);
        }
        obj.offset = new Vector2(0, 0);
    }

    void get_damage(GameObject enemy) {
        var ret = new List<int>(2);
        ret.Add(attP);
        ret.Add(attM);
        //Debug.Log("attacking " +enemy.name);
        var act_force = force;
        act_force.x *= owner.facing;
        enemy.GetComponent<Hero>().receive_force(act_force, control_time);
    }

    public GameObject get_effect() {
        sounds.Play();
        return new GameObject();
    }
}
