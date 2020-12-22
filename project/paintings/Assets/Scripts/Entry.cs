using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Main Camera").GetComponent<Camera>().cullingMask = LayerMask.GetMask("Player", "Enemy", "Wall");
        //Destroy(GameObject.Find("Hero"));
        var hero_model = Resources.Load<GameObject>("stickman/Hero");
        //Debug.Log(hero_model);
        var hero = Instantiate(hero_model);
        //hero.layer = LayerMask.NameToLayer("Player");
        hero.AddComponent<Hero>();
        hero.GetComponent<Hero>().set_type("hero");
        hero.name = "MainHero";      
        
        
        gameObject.AddComponent<MapLoader>();
        gameObject.AddComponent<InputHandler>();
        //Debug.Log(LayerMask.GetMask("wall"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
