using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject scoreText;
    public GameObject restartButton;

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOverPanel.SetActive(true);
        scoreText.SetActive(false);
        restartButton.SetActive(false);
    }

}
