using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    // required components
    private Image sprite_renderer;
    // editor parameters
    public float animation_speed;
    public Sprite[] walk_cycle;
    // Bubby's state variables
    private float animation_time = 0;
    private int current_sprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<Image>();
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
        sprite_renderer.sprite = walk_cycle[current_sprite];
    }
}
