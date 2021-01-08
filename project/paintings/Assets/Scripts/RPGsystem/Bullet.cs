using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Collider2D collider = null;
    public ContactFilter2D filter = new ContactFilter2D();
    public Role owner = null;
    public int inited = -1;
    public int level = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (inited == -1) return;
        if (inited == 0) {
            if (gameObject.GetComponent<MoveController>().inited == 1) inited = 1; else return;
        }
        //Debug.Log(gameObject.transform.position);
        frame_effect();
        var cds = new List<Collider2D>();
        collider.OverlapCollider(filter, cds);
        if (cds.Count > 0)
        {
            touch_effects(cds);
        }
    }
    public virtual void init(Role _owner, int _level) {
        // TODO add sounds
        owner = _owner;
        collider = null;
        filter.SetLayerMask(owner.ob_layer);
        level = _level;
    }
    public void active() {
        gameObject.GetComponent<MoveController>().active();
        inited = 0;
    }

    public virtual void frame_effect() { }
    public virtual void touch_effects(List<Collider2D> cds) { 
        foreach (var cd in cds) {
            touch_effect(cd.gameObject);
        }
    }
    public virtual void touch_effect(GameObject obj) { }
}
