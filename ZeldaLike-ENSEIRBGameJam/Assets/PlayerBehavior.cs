﻿/* Author : Raphaël Marczak - 2018, for ENSEIRB-MATMECA
 * 
 * This work is licensed under the CC0 License. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the cardinal directions (South, North, West, East)
public enum CardinalDirections { CARDINAL_S, CARDINAL_N, CARDINAL_W, CARDINAL_E };

public class PlayerBehavior : MonoBehaviour
{
    public float m_speed = 1f; // Speed of the player when he moves
    private CardinalDirections m_direction; // Current facing direction of the player

    public Sprite m_frontSprite = null;
    public Sprite m_leftSprite = null;
    public Sprite m_rightSprite = null;
    public Sprite m_backSprite = null;

    public GameObject enemy = null;
    public GameObject enemy_red = null;

    public GameObject m_fireBall = null; // Object the player can shoot

    public GameObject m_map = null;
    public DialogManager m_dialogDisplayer;

    private Dialog m_closestNPCDialog;

    private bool combat_incoming = false;

    private Vector3 last_position;

    private bool wait_start = false;
    private bool wait_end = false;
    private int manual_timer = 0;

    private Vector3 _offset = new Vector3(0, 1552, 0);
    private bool was_in_red = false;

    public AudioClip m_mapSound;

    Rigidbody2D m_rb2D;
    SpriteRenderer m_renderer;

    void Awake()
    {
        m_rb2D = gameObject.GetComponent<Rigidbody2D>();
        m_renderer = gameObject.GetComponent<SpriteRenderer>();

        m_closestNPCDialog = null;
    }

    // This update is called at a very precise and constant FPS, and
    // must be used for physics modification
    // (i.e. anything related with a RigidBody)
    void FixedUpdate()
    {
        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_map.activeSelf)
        {
            return;
        }

        if (combat_incoming)
        {
            combat_incoming = false;
            StartCombat();
            wait_start = true;
        }

        // Moves the player regarding the inputs
        Move();
    }

    private void Move()
    {
        float horizontalOffset = Input.GetAxis("Horizontal");
        float verticalOffset = Input.GetAxis("Vertical");

        // Translates the player to a new position, at a given speed.
        Vector2 newPos = new Vector2(transform.position.x + horizontalOffset,
                                     transform.position.y + verticalOffset)
                                     * m_speed;
        m_rb2D.MovePosition(newPos);

        // Computes the player main direction (North, Sound, East, West)
        if (Mathf.Abs(horizontalOffset) > Mathf.Abs(verticalOffset))
        {
            if (horizontalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_E;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_W;
            }
        }
        else if (Mathf.Abs(horizontalOffset) < Mathf.Abs(verticalOffset))
        {
            if (verticalOffset > 0)
            {
                m_direction = CardinalDirections.CARDINAL_N;
            }
            else
            {
                m_direction = CardinalDirections.CARDINAL_S;
            }
        }
    }


    // This update is called at the FPS which can be fluctuating
    // and should be called for any regular actions not based on
    // physics (i.e. everything not related to RigidBody)
    private void Update()
    {
        if (wait_start)
        {
            manual_timer++;
            if (manual_timer == 60)
            {
                Instantiate(enemy);
                Instantiate(enemy_red);
                manual_timer = 0;
                wait_start = false;
            }
        }

        if (wait_end)
        {
            manual_timer++;
            if (manual_timer == 60)
            {
                transform.position = GoBack();
                manual_timer = 0;
                wait_end = false;
            }
        }

        // If the player presses M, the map will be activated if not on screen
        // or desactivated if already on screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.instance.PlaySound(m_mapSound);
            m_map.SetActive(!m_map.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // If a dialog is on screen, the player should not be updated
        // If the map is displayed, the player should not be updated
        if (m_dialogDisplayer.IsOnScreen() || m_map.activeSelf)
        {
            return;
        }

        ChangeSpriteToMatchDirection();

        // If the player presses SPACE, then two solution
        // - If there is a dialog ready to be displayed (i.e. the player is closed to a NPC)
        //   then a dialog is set to the dialogManager
        // - If not, then the player will shoot a fireball
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_closestNPCDialog != null)
            {
                m_dialogDisplayer.SetDialog(m_closestNPCDialog.GetDialog());
                combat_incoming = true;
            }
            else
            {
                ShootFireball();
            }
        }

        
    }

    private Vector3 GoBack()
    {
        if (was_in_red)
        {
            was_in_red = false;
            if (gameObject.transform.position.y < -200f)
            {
                return last_position;
            }
            else
            {
                return last_position + _offset;
            }
        }
        else
        {
            if (gameObject.transform.position.y > -200f)
            {
                return last_position;
            }
            else
            {
                return last_position - _offset;
            }
        }
    }

    // Changes the player sprite regarding it position
    // (back when going North, front when going south, right when going east, left when going west)
    private void ChangeSpriteToMatchDirection()
    {
        if (m_direction == CardinalDirections.CARDINAL_N)
        {
            m_renderer.sprite = m_backSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_S)
        {
            m_renderer.sprite = m_frontSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_E)
        {
            m_renderer.sprite = m_rightSprite;
        }
        else if (m_direction == CardinalDirections.CARDINAL_W)
        {
            m_renderer.sprite = m_leftSprite;
        }
    }

    // Creates a fireball, and launches it
    private void ShootFireball()
    {
        GameObject newFireball = Instantiate(m_fireBall, this.transform) as GameObject;

        LightningBehavior fireBallBehavior = newFireball.GetComponent<LightningBehavior>();

        if (fireBallBehavior != null)
        {
            switch (m_direction)
            {
                case CardinalDirections.CARDINAL_N:
                    fireBallBehavior.Launch(new Vector2(0f, 1f));
                    break;
                case CardinalDirections.CARDINAL_S:
                    fireBallBehavior.Launch(new Vector2(0f, -1f));
                    break;
                case CardinalDirections.CARDINAL_E:
                    fireBallBehavior.Launch(new Vector2(1f, 0f));
                    break;
                case CardinalDirections.CARDINAL_W:
                    fireBallBehavior.Launch(new Vector2(-1f, 0f));
                    break;
            }
            // Lauches the fireball upward
            // (Vector2 represents a direction in x and y ;
            // so Vector2(0f, 1f) is a direction of 0 in x and 1 in y (up)
            // fireBallBehavior.Launch(new Vector2(0f, 1f));
        }
    }


    // This is automatically called by Unity when the gameObject (here the player)
    // enters a trigger zone. Here, two solutions
    // - the player is in an NPC zone, then he grabs the dialog information ready to be
    //   displayed when SPACE will be pressed
    // - the player is in an instantDialog zone, then he grabs the dialog information and
    //   displays it instantaneously
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = collision.GetComponent<Dialog>();
        }
        else if (collision.tag == "InstantDialog")
        {
            Dialog instantDialog = collision.GetComponent<Dialog>();
            if (instantDialog != null)
            {
                m_dialogDisplayer.SetDialog(instantDialog.GetDialog());
            }
        }
    }

    private void StartCombat()
    {
        last_position = gameObject.transform.position;
        if (gameObject.transform.position.y > -200f)
        {
            gameObject.transform.position = new Vector3(815f, -175f, 0f);
        }
        else
        {
            was_in_red = true;
            gameObject.transform.position = new Vector3(815f, -1710f, 0f);
        }
    }

    public void WaitEnd()
    {
        wait_end = true;
    }

    // This is automatically called by Unity when the gameObject (here the player)
    // leaves a trigger zone. Here, two solutions
    // - the player was in an NPC zone, then the dialog information is removed
    // - the player was in an instantDialog zone, then the instantDialog is destroyed
    //   (as it has been displayed, and must only be displayed once)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            m_closestNPCDialog = null;
        }
        else if (collision.tag == "InstantDialog")
        {
            Destroy(collision.gameObject);
        }
    }
}
