using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbyMovement : MonoBehaviour
{
    // required components
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body;
    private ParticleSystem particle_system;

    // Editor paramaters
    public float movement_speed;
    public float animation_speed;
    public Sprite[] walk_cycle;
    public Sprite dead_sprite;
    public LevelManager level_manager;

    // Bubby's state variables
    private float animation_time = 0;
    private int current_sprite = 0;
    private bool is_alive = true;
    private int walking_direction = 1;
    private bool on_ground = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body = GetComponent<Rigidbody2D>();
        particle_system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only do this if Bubby is alive and kicking and on the ground
        if (is_alive && on_ground)
        {
            // Update animation timer
            animation_time += Time.deltaTime;
        }

        // Check if its time to change animation sprite
        if (animation_time >= animation_speed)
        {
            animation_time = 0;
            ChangeSprite();
        }
    }

    // Movement with rigid body so do this here
    private void FixedUpdate()
    {
        // Only do this if Bubby is alive and kicking and on the ground
        if (is_alive && on_ground)
        {
            // Move Bubby
            rigid_body.velocity = new Vector2((float)walking_direction * movement_speed, rigid_body.velocity.y);
        }
        // Check if falling
        if (rigid_body.velocity.y < -0.5f)
        {
            on_ground = false;
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
        // Check relative position
        float y_diff = transform.position.y - collision.transform.position.y;
        float x_diff = transform.position.x - collision.transform.position.x;
        // Check if Bubby is touching the ground
        if (collision.collider.tag == "Ground")
        {
            // Check if it's the feet touching
            
            if (y_diff > 0)
            {
                on_ground = true;
            }
            // otherwise its side (probably)
            else
            {
                // Change direction
                walking_direction *= -1;
                sprite_renderer.flipX = !sprite_renderer.flipX;
            }
        }
        // Check if Bubby is touching a box
        if (collision.collider.tag == "Box")
        {
            // check if its top touching
            if (y_diff > 0 && Mathf.Abs(x_diff) < 0.225f)
            {
                on_ground = true;
            }
            // Check if it's the bottom touching
            else if (y_diff < 0 && Mathf.Abs(x_diff) < 0.225f)
            {
                Die(); 
            }
            // Otherwise its the side
            else
            {
                // Change direction
                walking_direction *= -1;
                sprite_renderer.flipX = !sprite_renderer.flipX;
            }
        }
        // check if bubby is touching a solid object
        if (collision.collider.tag == "Solid")
        {
            // Change direction
            walking_direction *= -1;
            sprite_renderer.flipX = !sprite_renderer.flipX;
        }
    }

    // Called when Bubby walks into spikes or the goal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Deadly")
        {
            Die();
        }
        else if (collision.tag == "Goal")
        {
            level_manager.ChangeState(0);
        }
    }

    // Called when Bubby dies
    void Die()
    {
        is_alive = false; // He's dead now duh
        sprite_renderer.sprite = dead_sprite; // Make him look dead
        rigid_body.simulated = false; // Stop moving
        particle_system.Play(); // Bloooooooood
        level_manager.ChangeState(1);
    }
}
