using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Required components
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body;
    private ParticleSystem particle_system;

    // Editor paramaters
    public float movement_speed;
    public float midair_speed;
    public float jump_power;
    public float animation_speed;
    public Sprite[] walk_cycle;
    public Sprite dead_sprite;
    public Sprite idle_sprite;
    public Transform front_hand;
    public Transform back_hand;
    public Vector2[] hand_positions;
    public LevelManager level_manager;

    // Player's state variables
    private float animation_time = 0;
    private int current_sprite = 0;
    private bool is_alive = true;
    private int walking_direction = 0;
    private bool on_ground = true;
    private bool is_falling = false;
    private bool pushing_box = false;
    private bool can_push_switch = false;
    private Switch push_switch;

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
                if (on_ground)
                {
                    animation_time += Time.deltaTime;
                }   
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                sprite_renderer.flipX = true;
                walking_direction = -1;

                // Update animation timer
                if (on_ground)
                {
                    animation_time += Time.deltaTime;
                }
            }
            else
            {
                walking_direction = 0;
                animation_time = 0;
                sprite_renderer.sprite = idle_sprite;
            }

            // Check if its time to change animation sprite
            if (animation_time >= animation_speed)
            {
                animation_time = 0;
                ChangeSprite();
            }

            // Change hand position
            if (!pushing_box)
            {
                if (on_ground)
                {
                    switch (walking_direction)
                    {
                        case -1: // running left
                            front_hand.localPosition = new Vector3(-hand_positions[1].x, hand_positions[1].y, 0);
                            back_hand.localPosition = new Vector3(-hand_positions[2].x, hand_positions[2].y, 0);
                            break;
                        case 1: // running right
                            front_hand.localPosition = new Vector3(hand_positions[1].x, hand_positions[1].y, 0);
                            back_hand.localPosition = new Vector3(hand_positions[2].x, hand_positions[2].y, 0);
                            break;
                        case 0: // idle
                        default:
                            front_hand.localPosition = new Vector3(hand_positions[0].x, hand_positions[0].y, 0);
                            back_hand.localPosition = new Vector3(hand_positions[0].x, hand_positions[0].y, 0);
                            break;
                    }
                }
                else if (is_falling)
                {
                    switch (walking_direction)
                    {
                        case -1: // left input
                            front_hand.localPosition = new Vector3(-hand_positions[3].x, hand_positions[3].y, 0);
                            back_hand.localPosition = new Vector3(-hand_positions[4].x, hand_positions[4].y, 0);
                            break;
                        case 1: // right input
                            front_hand.localPosition = new Vector3(hand_positions[3].x, hand_positions[3].y, 0);
                            back_hand.localPosition = new Vector3(hand_positions[4].x, hand_positions[4].y, 0);
                            break;
                        case 0: // no input
                        default:
                            front_hand.localPosition = new Vector3(hand_positions[3].x, hand_positions[3].y, 0);
                            back_hand.localPosition = new Vector3(hand_positions[4].x, hand_positions[4].y, 0);
                            break;
                    }
                }
            }

            // pressing a switch
            if (can_push_switch)
            {
                if (Input.GetButtonDown("Activate"))
                {
                    push_switch.Activate();
                }
            }
        }
    }

    // Movement with rigid body so do this here
    private void FixedUpdate()
    {
        // Check if falling
        if (rigid_body.velocity.y < -0.5f)
        {
            on_ground = false;
            is_falling = true;
        }
        // Ground movement
        if (on_ground)
        {
            // Move Player
            rigid_body.velocity = new Vector2((float)walking_direction * movement_speed, rigid_body.velocity.y);
        }
        // Midair movement
        else
        {
            // Move Player
            Vector2 velocity = rigid_body.velocity;
            velocity.x += (float)walking_direction * midair_speed * Time.fixedDeltaTime;
            if (velocity.x < -movement_speed)
            {
                velocity.x = -movement_speed;
            }
            else if (velocity.x > movement_speed)
            {
                velocity.x = movement_speed;
            }
            rigid_body.velocity = velocity;
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
        // Check if player is touching the ground
        if (collision.collider.tag == "Ground")
        {
            // Check if it's the feet touching
            if (y_diff > 0)
            {
                on_ground = true;
                is_falling = false;
            }
        }
        // Check if player is touching a box
        if (collision.collider.tag == "Box")
        {
            pushing_box = true;
            // Check if it's the top touching
            if (y_diff > 0 && Mathf.Abs(x_diff) < 0.22f)
            {
                pushing_box = false;
                on_ground = true;
                is_falling = false;
            }
            // Check if it's the left side touching
            else if (x_diff < 0)
            {
                // Place hand on box to left
                front_hand.localPosition = new Vector3(hand_positions[7].x, hand_positions[7].y, 0);
                back_hand.localPosition = new Vector3(hand_positions[8].x, hand_positions[8].y, 0);
            }
            // check if its the right side touching
            else if (x_diff > 0)
            {
                // Place hand on box to right
                front_hand.localPosition = new Vector3(-hand_positions[7].x, hand_positions[7].y, 0);
                back_hand.localPosition = new Vector3(-hand_positions[8].x, hand_positions[8].y, 0);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player is touching a box
        if (collision.collider.tag == "Box")
        {
            pushing_box = false;
        }
    }

    // Called when Player walks into spikes or near a switch
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it's deadly
        if (collision.tag == "Deadly")
        {
            Die();
        }

        // Check if it's a switch
        if (collision.tag == "Switch")
        {
            can_push_switch = true;
            push_switch = collision.GetComponent<Switch>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if it's a switch
        if (collision.tag == "Switch")
        {
            can_push_switch = false;
        }
    }

    void Die()
    {
        is_alive = false; // He's dead now duh
        sprite_renderer.sprite = dead_sprite; // Make him look dead
                                              // including hands
        front_hand.localPosition = new Vector3(hand_positions[5].x, hand_positions[5].y, 0);
        back_hand.localPosition = new Vector3(hand_positions[6].x, hand_positions[6].y, 0);
        rigid_body.simulated = false; // Stop moving
        particle_system.Play(); // Bloooooooood
        level_manager.ChangeState(1);
    }
}
