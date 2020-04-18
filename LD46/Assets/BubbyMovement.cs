using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbyMovement : MonoBehaviour
{
    // Sprite renderer for animation
    private SpriteRenderer sprite;

    // Editor paramaters
    public float movement_speed;
    public float animation_speed;
    public Sprite[] walk_cycle;

    // Bubby's state variables
    private float animation_time = 0;
    private int current_sprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update animation timer
        animation_time += Time.deltaTime;

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
        sprite.sprite = walk_cycle[current_sprite];
    }
}
