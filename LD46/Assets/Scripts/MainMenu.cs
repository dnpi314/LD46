using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioSource audio_source;
    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        audio_source.Play();
        SceneManager.LoadScene(1);
    }

    public void LevelSelect(int level)
    {
        audio_source.Play();
        SceneManager.LoadScene(level);
    }
}
