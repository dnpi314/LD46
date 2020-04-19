using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Editor parameters
    public Activatable[] attached_activatables;

    private AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        // play sound
        audio_source.Play();
        // switch the doors
        foreach (Activatable active in attached_activatables)
        {
            active.ToggleActivate();
        }
    }
}
