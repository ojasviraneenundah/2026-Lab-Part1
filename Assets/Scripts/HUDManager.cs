using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-293, 136, 0),
        new Vector3(-31, 0, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(270, 136, 0),
        new Vector3(0, -100, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;

    public GameObject gameOverPanel;

    public GameObject pauseButton;

    // related to SO
    public GameObject highscoreText;
    public IntVariable gameScore;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Awake()
    {
        // other instructions
        // subscribe to events
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        // GameManager.instance.gameRestart.AddListener(GameStart); // idk why your restart uses start but okay, we work with this i guess
        GameManager.instance.scoreChange.AddListener(SetScore);

    }

    public void GameStart()
    {
        Debug.Log("HUDManager.GameStart() called");
        // hide gameover panel
        gameOverPanel.SetActive(false);
        pauseButton.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];

        // related to SO
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}
