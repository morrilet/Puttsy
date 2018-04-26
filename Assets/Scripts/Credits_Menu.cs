using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_Menu : MonoBehaviour
{
    private void Start()
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

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
