using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedBehavior : MonoBehaviour
{

    public GameObject m_fireBall = null;
    private int shoot_time = 0;
    public int hp = 10;

    public List<DialogPage> red_dialog0;
    public List<DialogPage> red_dialog1;
    public List<DialogPage> red_dialog2;
    public List<DialogPage> red_dialog3;
    public List<DialogPage> red_dialog4;
    public List<DialogPage> red_dialog5;
    public List<DialogPage> red_dialog6;
    public List<DialogPage> red_dialog7;
    public List<DialogPage> red_dialog8;
    public List<DialogPage> red_dialog9;

    public List<DialogPage>[] red_dialog;

    private void Start()
    {
        red_dialog = new List<DialogPage>[] { red_dialog0, red_dialog1, red_dialog2, red_dialog3, red_dialog4, red_dialog5, red_dialog6, red_dialog7, red_dialog8, red_dialog9 };
    }


    // Update is called once per frame
    private void Update()
    {
        if (hp == 0)
        {
            GameObject brother = GameObject.FindGameObjectWithTag("Enemy");
            Destroy(brother);
            Destroy(gameObject);
        }
        shoot_time++;
        if (shoot_time % 180 == 0)
        {
            ShootFireball();
        }
    }

    // Creates a fireball, and launches it
    private void ShootFireball()
    {
        GameObject newFireball = Instantiate(m_fireBall, this.transform) as GameObject;

        FireBehavior fireBallBehavior = newFireball.GetComponent<FireBehavior>();

        GameObject player = GameObject.Find("Player");

        Vector3 v = (player.transform.position - this.transform.position).normalized;

        Vector2 dir = v;

        if (fireBallBehavior != null)
        {
            fireBallBehavior.Launch(dir);
        }
    }
}