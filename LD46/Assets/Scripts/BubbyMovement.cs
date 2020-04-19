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
        // Check if Bubby is touching the ground
        if (collision.collider.tag == "Ground")
        {
            // Check if it's the feet touching
            ContactPoint2D contact_point = collision.GetContact(0);
            float diff = contact_point.point.y - collision.collider.transform.position.y;
            if (Mathf.Abs(diff) < 0.1f)
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
        // Check if Bubby is touching a box or solid object
        if (collision.collider.tag == "Box" || collision.collider.tag == "Solid")
        {
            // Check if it's the side touching
            ContactPoint2D contact_point = collision.GetContact(0);
            float diff_left = contact_point.point.x - (collision.collider.transform.position.x - 0.125f);
            float diff_right = contact_point.point.x - (collision.collider.transform.position.x + 0.125f);
            float diff_bot = contact_point.point.y - (collision.collider.transform.position.y - 0.125f);
            float diff_top = contact_point.point.y - (collision.collider.transform.position.y + 0.125f);
            // check if its top touching
            if (Mathf.Abs(diff_top) < 0.1f)
            {
                on_ground = true;
            }
            if (Mathf.Abs(diff_left) < 0.1f || Mathf.Abs(diff_right) < 0.1f)
            {
                // Change direction
                walking_direction *= -1;
                sprite_renderer.flipX = !sprite_renderer.flipX;
            }
            // Check if it's the bottom touching
            else if (Mathf.Abs(diff_bot) < 0.1f)
            {
                Die();
            }
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
