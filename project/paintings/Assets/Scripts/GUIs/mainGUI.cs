using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainGUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Status stat = null;
    public Role role = null;
    protected Texture2D blood, bar, dark_magic, light_magic, null_skill, shadow;
    int inited = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init() {
        blood = Resources.Load<Texture2D>("guis/green_blood");
        bar = Resources.Load<Texture2D>("guis/red_blood");
        dark_magic = Resources.Load<Texture2D>("guis/dark_magic");
        light_magic = Resources.Load<Texture2D>("guis/light_magic");
        null_skill = Resources.Load<Texture2D>("itemsIcon/none");
        shadow = Resources.Load<Texture2D>("itemsIcon/shadow");

        //Debug.Log("blood = " + blood.ToString());
        inited = 1;
    }
    private void OnGUI()
    {
        if (stat == null) {
            //Debug.Log(GameObject.Find("MainHero").GetComponent<Role>());
            if (GameObject.Find("MainHero").GetComponent<Role>().stat.init_finish == 1) {
                stat = GameObject.Find("MainHero").GetComponent<Role>().stat;
                role = GameObject.Find("MainHero").GetComponent<Role>();
                
            } 
        }
        if (stat == null || inited == 0) return;

        // hp
        GUI.DrawTexture(new Rect(30, 30, stat.hp_max * 2, 20), bar);
        GUI.DrawTexture(new Rect(30, 30, stat.hp * 2, 20), blood);


        // mp
        GUI.DrawTexture(new Rect(30, 60, stat.mp_max * 1.5f, 15), dark_magic);
        GUI.DrawTexture(new Rect(30, 60, stat.mp * 1.5f, 15), light_magic);

        // skill
        for (int i = 0; i < 4; i++) {
            Texture2D icon = (role.skill[i] == null) ? null_skill : role.skill[i].icon;
            GUI.DrawTexture(new Rect(30 + 30 * i, 80, 25, 25), icon);
            if (role.skill[i] == null) continue;
            float k = (role.skill[i].cold_t - Time.time) / (role.skill[i].cold);
            GUI.DrawTexture(new Rect(30 + 30 * i, 80, 25, 25 * Mathf.Max(0, Mathf.Min(1, k))), shadow);
            
        }
    }
}
