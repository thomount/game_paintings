using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Random : MonoBehaviour
{
    // Start is called before the first frame update
    private int status = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Hero>().Move(3);
        gameObject.GetComponent<Hero>().Jump(true);
    }
}
