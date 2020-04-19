using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Editor paramaters
    public MonoBehaviour[] actors;
    public Canvas start_ui;
    public GameObject tutorial_ui;
    public Canvas finish_ui;
    public Canvas gameover_ui;
    public bool has_tutorial;
    public int level;

    // Manager states
    private enum UIState { start, tutorial, game, finish, gameover};
    private UIState ui_state = UIState.start;
    private bool finished_tutorial = false;
    // Start is called before the first frame update
    void Start()
    {
        if (has_tutorial)
        {
            ui_state = UIState.tutorial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Activate"))
        {
            switch (ui_state)
            {
                case UIState.start: // close start screen then either open tutorial or start level
                    if (has_tutorial)
                    {
                        if (finished_tutorial)
                        {
                            start_ui.enabled = false;
                            ui_state = UIState.game;
                            foreach (MonoBehaviour actor in actors)
                            {
                                actor.enabled = true;
                            }
                        }
                        else
                        {
                            finished_tutorial = true;
                        }
                    }
                    else
                    {
                        start_ui.enabled = false;
                        ui_state = UIState.game;
                        foreach (MonoBehaviour actor in actors)
                        {
                            actor.enabled = true;
                        }
                    }
                    break;
                case UIState.tutorial: // let tutorial script handle this
                    break;
                case UIState.game: // do nothing
                    break;
                case UIState.finish: // load next level
                    SceneManager.LoadScene(level + 1);
                    break;
                case UIState.gameover: // restart level
                    SceneManager.LoadScene(level);
                    break;
                default:
                    break;
            }
        }
    }

    public void ChangeState(int s)
    {
        foreach (MonoBehaviour actor in actors)
        {
            actor.enabled = false;
        }
        switch (s)
        {
            case 0: // Victory
                ui_state = UIState.finish;
                finish_ui.enabled = true;
                break;
            case 1: // Game over
                ui_state = UIState.gameover;
                gameover_ui.enabled = true;
                break;
            case 2:
                tutorial_ui.SetActive(false);
                ui_state = UIState.start;
                break;
            default:
                break;
        }
    }
}
