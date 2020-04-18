using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Required components
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body;

    // Editor paramaters
    public float movement_speed;
    public float midair_speed;
    public float jump_power;
    public float animation_speed;
    public Sprite[] walk_cycle;
    public Sprite dead_sprite;

    // Player's state variables
    private float animation_time = 0;
    private int current_sprite = 0;
    private bool is_alive = true;
    private int walking_direction = 0;
    private bool on_ground = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only do this if player is alive
        if (is_alive)
        {
            // Get inputs
            if (Input.GetButtonDown("Jump") && on_ground)
            {
                // Set upward velocity to jump power
                Vector2 velocity = rigid_body.velocity;
                velocity.y = jump_power;
                rigid_body.velocity = velocity;

                on_ground = false; // no longer on ground after jumping duh
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                sprite_renderer.flipX = false;
                walking_direction = 1;

                // Update animation timer
                animation_time += Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                sprite_renderer.flipX = true;
                walking_direction = -1;

                // Update animation timer
                animation_time += Time.deltaTime;
            }
            else
            {
                walking_direction = 0;
            }
            // Ground movement
            if (on_ground)
            {
                // Move Player
                rigid_body.velocity = new Vector2((float)walking_direction * movement_speed, rigid_body.velocity.y);
            }
            // Midair movement CHANGE THIS
            else
            {
                // Move Player
                rigid_body.velocity = new Vector2((float)walking_direction * midair_speed, rigid_body.velocity.y);
            }
        }
        // Check if its time to change animation sprite
        if (animation_time >= animation_speed)
        {
            animation_time = 0;
            ChangeSprite();
        }
    }

    // Called when its time to change the animation sprite
    void ChangeSprite()
    {
        // Change to next sprite
        current_sprite++;

        // Loop back to 0
        if (current_sprite >= walk_cycle.Length)
        {
            current_sprite = 0;
        }

        // Set sprite in sprite renderer
        sprite_renderer.sprite = walk_cycle[current_sprite];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if player is touching the ground
        if (collision.collider.tag == "Ground")
        {
            on_ground = true;
        }
    }
}
