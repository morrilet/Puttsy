using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    private void Start()
    {
        if(!AudioManager.instance.musicSource.isPlaying)
        {
            AudioManager.instance.PlayMusic("Background", 0.5f);
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0.5f;
        }
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
