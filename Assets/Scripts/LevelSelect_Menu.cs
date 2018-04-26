using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect_Menu : MonoBehaviour
{
    public void Start()
    {
        if (!AudioManager.instance.musicSource.isPlaying)
        {
            AudioManager.instance.PlayMusic("Background", 0.5f);
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0.5f;
        }
    }

    public void StartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
