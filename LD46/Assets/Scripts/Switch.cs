using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Editor parameters
    public Activatable[] attached_activatables;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        foreach (Activatable active in attached_activatables)
        {
            active.ToggleActivate();
        }
    }
}
