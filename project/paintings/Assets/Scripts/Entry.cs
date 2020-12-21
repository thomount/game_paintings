using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hero = Instantiate(GameObject.Find("Hero"));
        GameObject.Find("Hero").SetActive(false);
        hero.AddComponent<Hero>();
        hero.GetComponent<Hero>().set_type("hero");
        hero.name = "MainHero";      
        
        gameObject.AddComponent<MapLoader>();
        //Debug.Log(LayerMask.GetMask("wall"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
