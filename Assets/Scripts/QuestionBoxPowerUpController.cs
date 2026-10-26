using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        // GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // void Awake()
    // {
    //     GameManager.instance.gameRestart.AddListener(GameRestart);
    // }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerup.hasSpawned)
        {
            // show disabled sprite
            this.GetComponent<Animator>().SetTrigger("used");
            // spawn the powerup
            powerup.ShowPowerup();
            powerupAnimator.SetTrigger("spawned");
        }
    }

    // used by animator
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void GameRestart()
{
    Debug.Log("Question Box reset called!");

    

    // reset question box animation
    Animator boxAnimator = GetComponent<Animator>();
    boxAnimator.Rebind();
    boxAnimator.Update(0f);

    // reset powerup animation
    powerupAnimator.Rebind();
    powerupAnimator.Update(0f);

    transform.localPosition = originalPosition;

    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

}

}