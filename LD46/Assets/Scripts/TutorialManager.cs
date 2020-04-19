using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // Editor parameters
    public GameObject[] tutorial_panels;
    public LevelManager level_manager;

    // Tutorial states
    private int current_panel = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Activate"))
        {
            // Change panel to next if there is one
            if (current_panel < tutorial_panels.Length - 1)
            {
                tutorial_panels[current_panel].SetActive(false);
                current_panel++;
                tutorial_panels[current_panel].SetActive(true);
            }
            // Otherwise close tutorial
            else
            {
                level_manager.ChangeState(2);
            }
        }
    }
}
