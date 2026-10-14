using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public GameObject pausePanel;

    public GameObject pauseButton;
    
    public void PauseGame()
{
    pausePanel.SetActive(true);
    pauseButton.SetActive(false);
    Time.timeScale = 0f;
    AudioListener.pause = true;
}
public void ResumeGame()
{
    pausePanel.SetActive(false);
    pauseButton.SetActive(true);
    Time.timeScale = 1f;
    AudioListener.pause = false;
}

}
