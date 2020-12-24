using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    /*
    public float std_v = 3;
    public float std_jump_force1 = 8;
    public float std_jump_force2 = 6;

    public int jump_limit = 2;
    */
    // Start is called before the first frame update
    Role role = null;
    void Start()
    {
        role = GetComponent<Role>();
        //Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (role == null) return;
        /*
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("Left");
            GameObject.Find("Main Camera").transform.Translate(new Vector3(-0.1f, 0, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Right");
            GameObject.Find("Main Camera").transform.Translate(new Vector3(+0.1f, 0, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            GameObject.Find("Main Camera").transform.Translate(new Vector3(0, +0.1f, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Down");
            GameObject.Find("Main Camera").transform.Translate(new Vector3(0, -0.1f, 0));
        }
        Debug.Log(GameObject.Find("Main Camera").transform.position);
        */
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            //Debug.Log("Left");
            role.Move(-1);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //Debug.Log("Right");
            role.Move(1);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("Up");
            role.Jump(true);
        } else
        {
            role.Jump(false);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Debug.Log("Down");
        }


        if (Input.GetKey(KeyCode.J)) {
            role.Attack(0);
        }
        if (Input.GetKey(KeyCode.K)) {
            role.Attack(1);
        }
    }
}
