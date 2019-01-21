using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public GameObject m_fireBall = null;
    private int shoot_time = 0;
    public int hp = 10;


    // Update is called once per frame
    private void Update()
    {
        if (hp == 0)
        {
            GameObject brother = GameObject.FindGameObjectWithTag("EnemyRed");
            Destroy(brother);
            Destroy(gameObject);
        }
        shoot_time ++;
        if (shoot_time % 30 == 0)
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
