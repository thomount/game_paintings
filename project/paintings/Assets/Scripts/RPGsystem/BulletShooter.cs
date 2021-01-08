using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class BulletShooter
{
    // Start is called before the first frame update
    static public GameObject gen_bullet(string name, Vector2 pos, Vector2 vel, Vector2 acc, float life_time, string sprite_path) {
        Debug.Log(pos.ToString() + vel.ToString() + acc.ToString());
        GameObject ret = new GameObject(name);
        var controller = ret.AddComponent<MoveController>();
        controller.init(pos, vel, acc, life_time);
        var renderer = ret.AddComponent<SpriteRenderer>();

        renderer.sprite = Resources.Load<Sprite>(sprite_path);
        renderer.spriteSortPoint = SpriteSortPoint.Pivot;
        renderer.sortingOrder = 3;
        return ret;
    }
}
