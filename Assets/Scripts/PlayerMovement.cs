using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // for text mesh pro UGI

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D marioBody;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // for left and right movement
    public float maxSpeed = 15;

    // to jump
    public float upSpeed = 7;

    // ground state (yeah)
    private bool onGroundState = true;

    // for animation
    public Animator marioAnimator;

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            // for skid animation
            if (marioBody.linearVelocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        // set xSpeed parameter with current velocity of rigidbody2d mario
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }


    // when you collide on the ground + with other stuff, ongroundstate becomes true
    // below collision layer mask for ground, enemy, and obstacles
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        //if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Obstacles")) && !onGroundState)
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // just mario moving left and right without sliding and others
    void FixedUpdate()
    {
        if (alive)
        {

            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // verify that it doesnt go above maxSpeed
                if (marioBody.linearVelocity.magnitude < maxSpeed)
                {
                    marioBody.AddForce(movement * speed);
                }
            }

            // stop
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                // shtop
                marioBody.linearVelocity = Vector2.zero;
            }

            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }

        }

    }

    // for mario death
    public AudioClip marioDeath;
    public float deathImpulse = 5;

    // state
    [System.NonSerialized]
    public bool alive = true;

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    // calls game over in game manager when mario collides with goomba
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with goomba!");

            // gameManager.GameOver();

            // play death animation
            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    // game over pro max
    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        gameManager.GameOver();
    }

    // when restart button is clicked
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    // other methods

    // for game manager
    public GameManager gameManager;

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;

        gameManager.gameOverPanel.SetActive(false);
        gameManager.scoreText.SetActive(true);
        gameManager.restartButton.SetActive(true);
    }

    // reset score
    public JumpOverGoomba jumpOverGoomba;

    // reset camera to mario position
    public Transform gameCamera;

    // for question boxes
    public QuestionBox[] questionBoxes;

    // need to fix mario and goomba physics after restart button pressed
    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-0.86f, 0.28f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba positions
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        // reset score
        jumpOverGoomba.score = 0;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 1, -10);

        foreach (QuestionBox box in questionBoxes)
        {
            box.ResetBox();
        }
    }



    // for audio
    public AudioSource marioAudio;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }



}
