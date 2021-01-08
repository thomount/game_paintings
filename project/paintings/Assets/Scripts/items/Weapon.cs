using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CollectiveItem
{
    public int usage = 0;
    public int mode = 0;
    public int attP = 0;
    public int attM = 0;
    public float crt = 0;
    public Role owner;
    public Vector2 offset;
    public Vector2 force;
    public int control_time = 0;
    public AudioSource sounds;
    public SpriteRenderer render;
    public float PhK, MgK, DmgType;     //伤害分配和呈现
    public int side = -1;
    public string icon_path = null;
    // Start is called before the first frame update
    protected override void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
       
    }

    public void set_owner(GameObject _owner) {
        owner = _owner.GetComponent<Role>();
    }

    public virtual Vector2Int next_Attack(Vector2Int last) {
        if (last.x == 0)
            return new Vector2Int(1,1);
        if (last.y > 0) 
            return new Vector2Int(last.x, last.y-1);
        return new Vector2Int(0,0);
    }
    
    // 攻击判定区域对象
    public virtual void get_hit(int mask) {
        var obj = gameObject.GetComponent<Collider2D>();
        obj.enabled = true;
        //obj.offset = offset;
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
            
            var cube = Instantiate(Resources.Load<GameObject>("map/Cube"));
            cube.name = "hitting place";
            cube.GetComponent<Collider2D>().isTrigger = true;
            cube.transform.position = obj.bounds.center;
            cube.transform.localScale = 2 * obj.bounds.extents;
            cube.layer = LayerMask.NameToLayer("Player");
            Destroy(cube, 5);
        }
        */
        if (cds.Count > 0) get_effect();
        foreach (var cd in cds) {
            get_damage(cd.gameObject);
        }
        //obj.offset = new Vector2(0, 0);
        obj.enabled = false;
    }

    void get_damage(GameObject enemy) {
        // calculate the damage
        int dmg = Mathf.FloorToInt((attP + owner.stat.atk) * PhK + (attM + owner.stat.mtk) * MgK);
        int isCrt = 0;
        if (Random.Range(0, 1f) < owner.stat.crt) {
            dmg = Mathf.FloorToInt(dmg*(1 + owner.stat.crt_dmg));
            isCrt = 1;
        }
        //Debug.Log("attacking " +enemy.name);
        var act_force = force;
        act_force.x *= owner.facing;
        Role role = enemy.GetComponent<Role>();
        role.receive_force(act_force, control_time);
        Debug.Log(owner.name + " hit " + enemy.name);
        if (DmgType == 1) role.stat.AddBuff(new PhysicsDamage(dmg, isCrt, role));
        if (DmgType == 2) role.stat.AddBuff(new MagicDamage(dmg, isCrt, role));
    }

    public void get_effect() {
        sounds.Play();
        //return new GameObject();
        // TODO 生成特效
    }

    // 更新status
    public virtual void passive_effect() { 
        
    }
    public virtual void onEquip(int _side) {
        side = _side;
        if (owner.skill[4 + side] != null)
            Destroy(owner.skill[4 + side]);
    }
    public virtual void unEquip() { 
    }

}
