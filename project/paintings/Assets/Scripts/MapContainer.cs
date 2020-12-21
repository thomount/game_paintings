using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainer : MonoBehaviour
{
    private GameObject hero;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void init(string s, Vector2 pos) {
        
        hero = GameObject.Find("MainHero");
        //Debug.Log(transform.position);
        Vector2 hero_offset = new Vector2(0,0);
        if (s == "random") {
            var cube_num = 200;
            int bound_left = -40, bound_right = 40, bound_down = -20, bound_up = 20;
            for (int i = 0; i < cube_num; i++) {
                GameObject cube_clone = Instantiate(GameObject.Find("Cube"));
                cube_clone.name = "Cube_"+i.ToString();
                cube_clone.transform.position = new Vector2(Random.Range(bound_left, bound_right), Random.Range(bound_down, bound_up));
                cube_clone.transform.parent = transform;
            }  
            
        }

        if (s == "main_menu") {
            var plat_len = 5;
            hero_offset = new Vector2(0, 3);
            List<Vector2> Plat_off = new List<Vector2>();
            Plat_off.Add(new Vector2(-2, 0));
            Plat_off.Add(new Vector2(-2, -6));
            Plat_off.Add(new Vector2(-9, 3));
            Plat_off.Add(new Vector2(-9, -3));
            Plat_off.Add(new Vector2(5, 3));
            Plat_off.Add(new Vector2(5, -3));
            for (int i = 0; i < plat_len; i++) {
                foreach (Vector2 off in Plat_off) {
                    GameObject cube_clone = Instantiate(GameObject.Find("Cube"));
                    cube_clone.transform.position = new Vector2(i, 0) + off;
                    cube_clone.transform.parent = transform;
                }
                
            }
            var enemy = Instantiate(GameObject.Find("Cube"));
            enemy.transform.position = new Vector2(3, 2);
            enemy.transform.parent = transform;
            enemy.layer = LayerMask.NameToLayer("Enemy");
            enemy.name = "enemy";
        }
        hero.transform.position = pos + hero_offset;
        transform.position = pos;
    }

    void del() {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
