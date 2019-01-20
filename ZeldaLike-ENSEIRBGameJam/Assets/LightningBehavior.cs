/* Author : Raphaël Marczak - 2018, for ENSEIRB-MATMECA
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBehavior : MonoBehaviour
{
   Rigidbody2D m_rb2D;

   float m_launchedTime; // Absolute time (in sec.) when the fireball has been launched
   float m_fireDuration = 2f; // Time (in sec.) after which the fireball should be destroyed

   float m_speed = 100f; // Speed of the fireball

    private int rand;

    

    GameObject player;

    void Awake()
   {
       m_rb2D = gameObject.GetComponent<Rigidbody2D>();
       m_launchedTime = Time.realtimeSinceStartup;
   }

   public void Launch(Vector2 direction)
   {
       m_rb2D.AddForce(direction.normalized * m_speed, ForceMode2D.Impulse);
   }

   void Update()
   {
        // Checks if the fireball should remain on screen
        // or if the life time has been reached
        if (Time.realtimeSinceStartup > m_launchedTime + m_fireDuration)
       {
           Destroy(gameObject);
       }
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
        // Destroys the fireball when it hits something, except the player or another fireball
        // (to prevent the fireball to be destroyed as soon as it is created)
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
        }
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyBehavior>().hp--;
                if (collision.gameObject.GetComponent<EnemyBehavior>().hp == 0)
                {
                    player.GetComponent<PlayerBehavior>().WaitEnd();
                }
            }
            else
            {
                if (collision.gameObject.tag == "EnemyRed")
                {
                    collision.gameObject.GetComponent<EnemyRedBehavior>().hp--;
                    if (collision.gameObject.GetComponent<EnemyRedBehavior>().hp != 0)
                    {
                        rand = Random.Range(0, 10);
                        player.GetComponent<PlayerBehavior>().m_dialogDisplayer.SetDialog(collision.gameObject.GetComponent<EnemyRedBehavior>().red_dialog[rand]);
                    }
                    else
                    {
                        player.GetComponent<PlayerBehavior>().WaitEnd();
                    }
                }
            }
        }
       if (collision.gameObject.tag != "Fireball" && collision.gameObject.tag != "Player")
       {
           Destroy(gameObject);
       }
       else
       {
           Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
       }
   }
}
