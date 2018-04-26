using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Retry_Menu : MonoBehaviour
{
    public Button retryButton;
    private Text retryText;

    private void Start()
    {
        retryText = retryButton.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if(GameManager.instance.ballInHole)
        {
            retryText.text = "Next";
            
            if (Application.CanStreamedLevelBeLoaded(GetNextScene()))
            {
                retryButton.onClick.RemoveAllListeners();
                retryButton.onClick.AddListener(NextLevel);
            }
            else
            {
                retryButton.interactable = false;
            }
        }
        else
        {
            retryText.text = "Retry";

            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(ResetLevel);
        }
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ResetLevel()
    {
        GameManager.instance.ResetLevel();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(GetNextScene());
    }

    private string GetNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int currentLevelNumber = int.Parse(currentScene.ToCharArray()[currentScene.Length - 1].ToString());

        string nextScene = "Level_" + (currentLevelNumber + 1).ToString();

        return nextScene;
    }
}
