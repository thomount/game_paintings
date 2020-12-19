using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cube_num = 40;
        for (int i = 0; i < cube_num; i++) {
            GameObject cube_clone = Instantiate(GameObject.Find("Cube"));
            cube_clone.name = "Cube_"+i.ToString();
            cube_clone.transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
