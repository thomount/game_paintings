using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Random : MonoBehaviour
{
    // Start is called before the first frame update
    protected Role role = null;
    private float decide_time = 0;
    private int movedir = 0;

    protected virtual void Start()
    {
        role = GetComponent<Role>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (role != null)
        {
            if (Time.time > decide_time) {
                decide_time = Time.time + 0.5f;
                movedir = Random.Range(-1, 2);
                if (Random.Range(0, 10) > 4) role.Attack(0);
            }
            role.Move(movedir);
        }
    }
}
