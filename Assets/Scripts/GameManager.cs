using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour // Singleton<GameManager>
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

    public IntVariable gameScore;
    
    // events
    public UnityEvent gameStart;
    // public UnityEvent gameRestart;  
    // public UnityEvent<int> scoreChange;

    // public UnityEvent gameOver;

    // initialise enemy movement to later subscribe to the event/delegate
    // public EnemyMovement[] enemy;

    private int score = 0;
    public UnityEvent setScore;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;

        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
        
        // subscribed to event and added a callback function
        // foreach (EnemyMovement enemy in this.enemy)
        // {

        //     enemy.onGoombaStomped += theActualGoombaStomped;
        //     //enemy.goombaAnimator = enemy.GetComponent<Animator>();
        // }
        
    }
// removed the delegate done and replacing with lab 5 SO, will hook up listener in inspector
    // public void RegisterEnemy(EnemyMovement e)
    // {
    //     e.onGoombaStomped -= theActualGoombaStomped; // avoid double-subscribing
    //     e.onGoombaStomped += theActualGoombaStomped;
    // }

    // public void UnregisterEnemy(EnemyMovement e)
    // {
    //     e.onGoombaStomped -= theActualGoombaStomped;
    // }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(score);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        // score = 0;
        // SetScore(score);
        Debug.Log("GameManager.GameRestart() called");

        // reset score
        gameScore.Value = 0; // probably need to take this out later also 
        // gameRestart.Invoke();// taking this out and letting the restart button raise instead, lab5
        Time.timeScale = 1.0f;
    }

    // will call this function in inspector when listening for OnIncreaseScore
    public void IncreaseScore(int increment)
    {
        Debug.Log("IncreasedScore() ran from GM with following increment: "+increment.ToString());
        // score += increment;
        // SetScore(score);

        // increase score by 1
        // gameScore.ApplyChange(1);

        // better use like this
        gameScore.ApplyChange(increment);
        SetScore(gameScore.Value);
    }

    public void SetScore(int score)
    {
        // scoreChange.Invoke(score);

        // invoke score change event with current score to update HUD
        // scoreChange.Invoke(gameScore.Value);
        setScore.Invoke();
    }


    public void GameOver()
    {
        Debug.Log("GameManager.GameOver() called");
        Time.timeScale = 0.0f;
        // gameOver.Invoke(); //lab5
    }

    // the callback function, took out this function, and am going to use the increaseScore function directly 
    // void theActualGoombaStomped()
    // {
    //     Debug.Log("Goomba stomped callback, score now: " + gameScore.Value);
    //     IncreaseScore(1);
    // }

} 
