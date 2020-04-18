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

    // Bubby's state variables
    private float animation_time = 0;
    private int current_sprite = 0;
    private bool is_alive = true;
    private int walking_direction = 1;

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
        // Only do this if Bubby is alive and kicking
        if (is_alive)
        {
            // Update animation timer
            animation_time += Time.deltaTime;

            // Move Bubby
            rigid_body.velocity = new Vector2((float)walking_direction * movement_speed, rigid_body.velocity.y);
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

    // Called when Bubby walks into spikes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Deadly")
        {
            is_alive = false; // He's dead now duh
            sprite_renderer.sprite = dead_sprite; // Make him look dead
            rigid_body.simulated = false; // Stop moving
            particle_system.Play(); // Bloooooooood
        }
    }
}
