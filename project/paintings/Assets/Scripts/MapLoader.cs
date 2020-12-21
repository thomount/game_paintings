using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    // Start is called before the first frame update
//    private List<GameObject> objs;
    private int bound_left, bound_right, bound_up, bound_down;
    private int current_left, current_right, current_up, current_down;
    private GameObject owner;

    public int load_width, load_height;
    private List<GameObject> loaded_map = new List<GameObject>();
    void Start()
    {
//        objs = new List<GameObject>();
        owner = GameObject.Find("MainHero");        

        bound_left = Mathf.FloorToInt(owner.transform.position.x) - 50;
        bound_right = Mathf.FloorToInt(owner.transform.position.x) + 50;
        bound_up = Mathf.FloorToInt(owner.transform.position.y) + 20;
        bound_down = Mathf.FloorToInt(owner.transform.position.y) - 20;

        
        LoadMap("main_menu", new Vector2(0,0));
        /*
        var cube_num = 40;
        for (int i = 0; i < cube_num; i++) {
            GameObject cube_clone = Instantiate(GameObject.Find("Cube"));
            cube_clone.name = "Cube_"+i.ToString();
            cube_clone.transform.position = new Vector2(Random.Range(bound_left, bound_right), Random.Range(bound_down, bound_up));
            objs.Add(cube_clone);
        }
        */

        current_up = bound_up;
        current_left = bound_left;
        current_down = bound_down;
        current_right = bound_right;
    }

    // Update is called once per frame
    void Update()
    {
        bound_left = Mathf.FloorToInt(owner.transform.position.x) - 50;
        bound_right = Mathf.FloorToInt(owner.transform.position.x) + 50;
        bound_up = Mathf.FloorToInt(owner.transform.position.y) + 20;
        bound_down = Mathf.FloorToInt(owner.transform.position.y) - 20;
        
        map_load_check();
    }

    void map_load_check() {
        
    }

    void LoadMap(string s, Vector2 pos) {
        GameObject new_map = Instantiate(GameObject.Find("MapContainer")); 
        new_map.name = s;
        new_map.GetComponent<MapContainer>().init(s, pos);
        loaded_map.Add(new_map);
    }
    void unLoadMap(string s) {
        foreach (var item in loaded_map)
        {
            if (item.name.Equals(s)) loaded_map.Remove(item);
        }
    }
}
