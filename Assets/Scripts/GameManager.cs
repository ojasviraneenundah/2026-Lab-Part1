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
    
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
