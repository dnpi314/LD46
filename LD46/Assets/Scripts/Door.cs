using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    // Required components
    SpriteRenderer door_sprite;
    SpriteRenderer light_sprite;
    BoxCollider2D box_collider;

    // Editor parameters
    public Sprite red_light;
    public Sprite green_light;
    public Sprite open_door;
    public Sprite closed_door;
    public bool open;

    // Start is called before the first frame update
    void Start()
    {
        door_sprite = GetComponent<SpriteRenderer>();
        light_sprite = GetComponentsInChildren<SpriteRenderer>()[1];
        box_collider = GetComponent<BoxCollider2D>();

        // set door to its starting state
        if (open)
        {
            door_sprite.sprite = open_door;
            light_sprite.sprite = green_light;
            box_collider.enabled = false;
        }
        else
        {
            door_sprite.sprite = closed_door;
            light_sprite.sprite = red_light;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ToggleActivate()
    {
        // switch door state
        open = !open;
        if (open)
        {
            door_sprite.sprite = open_door;
            light_sprite.sprite = green_light;
            box_collider.enabled = false;
        }
        else
        {
            door_sprite.sprite = closed_door;
            light_sprite.sprite = red_light;
            box_collider.enabled = true;
        }
    }
}
