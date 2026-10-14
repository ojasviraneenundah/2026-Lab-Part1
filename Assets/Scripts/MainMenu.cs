using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject highscoreText;
    public IntVariable gameScore;

    void Start()
    {
        SetHighscore();
    }
    public void StartGame()
    {
        // once done, go to next scene
        Debug.Log("Starting game...");
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Single);
    }

    void SetHighscore()
    {
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
    }

    public void ResetHighScore()
    {
        // TODO
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        gameScore.ResetHighestValue();
        SetHighscore();
    }
}
