using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // public GameObject gameOverPanel;
    // public GameObject scoreText;
    // public GameObject restartButton;

    // public void GameOver()
    // {
    //     Time.timeScale = 0.0f;
    //     gameOverPanel.SetActive(true);
    //     scoreText.SetActive(false);
    //     restartButton.SetActive(false);
    // }

    
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    // initialise enemy movement to later subscribe to the event/delegate
    public EnemyMovement[] enemy;

    private int score = 0;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        
        // subscribed to event and added a callback function
        foreach (EnemyMovement enemy in this.enemy)
        {
            enemy.onGoombaStomped += theActualGoombaStomped;
            //enemy.goombaAnimator = enemy.GetComponent<Animator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }

    // the callback function 
    void theActualGoombaStomped()
    {
        IncreaseScore(1);
    }

} 
