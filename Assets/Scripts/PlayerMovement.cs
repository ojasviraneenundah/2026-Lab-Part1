using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // for text mesh pro UGI

using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // public float speed = 10;
    private Rigidbody2D marioBody;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // for left and right movement
    // public float maxSpeed = 15;

    // to jump
    // public float upSpeed = 7;

    // ground state (yeah)
    private bool onGroundState = true;

    // for animation
    public Animator marioAnimator;

    // scriptable object for game constants
    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        // Set constants for SO
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;

        marioSprite = GetComponent<SpriteRenderer>();

        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

        //SceneManager.activeSceneChanged += SetStartingPosition;
    }

    // fix for mario being destroyed when next scene unfolds
    // void OnEnable()
    // {
    //     SceneManager.activeSceneChanged += SetStartingPosition;
    // }

    // void OnDisable()
    // {
    //     SceneManager.activeSceneChanged -= SetStartingPosition;
    // }

    // setting mario's starting position when changing scenes
    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-0.86f, 0.28f, 0.0f);
        }
    }


    void Awake(){
        // other instructions
        
        // hmm
        // gameManager = GameManager.instance;
        // // subscribe to Game Restart event
        // GameManager.instance.gameRestart.AddListener(GameRestart); // taking this out and adding SO listener in inspector
    }


    // refactored - Update is called once per frame
    void Update()
    {      
        // set xSpeed parameter with current velocity of rigidbody2d mario
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    // refactored 
    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
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

    private bool moving = false;
    // refactored - just mario moving left and right without sliding and others
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }

    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    private bool jumpedState = false;

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }


    // for mario death
    public AudioClip marioDeath;
    // public float deathImpulse = 2;

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
            EnemyMovement enemy = other.GetComponent<EnemyMovement>(); // for callback function from stomp()

            if (marioBody.linearVelocity.y < 0)
        {
            Debug.Log("STOMP!");
            enemy.Stomp();

            marioBody.linearVelocity = new Vector2(
                marioBody.linearVelocity.x,
                upSpeed
            );
            
        }
        else{
            Debug.Log("Collided with goomba!");

            // play death animation
            marioAnimator.Play("mario-die");
            marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
            alive = false;
            }
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
        GameRestart();
        // resume time
        Time.timeScale = 1.0f;

        // gameManager.gameOverPanel.SetActive(false);
        // gameManager.scoreText.SetActive(true);
        // gameManager.restartButton.SetActive(true);
    }

    // reset score
    // public JumpOverGoomba jumpOverGoomba;

    // reset camera to mario position
    public Transform gameCamera;

    // for question boxes
    public QuestionBox[] questionBoxes;
    public Coin_spawn[] coin_Spawns;
    public Bouncebox[] bricks;

    // need to fix mario and goomba physics after restart button pressed
    public void GameRestart()
    {
        Debug.Log("playermovement.GameRestart() called");
        // reset position
        marioBody.transform.position = new Vector3(-0.86f, 0.28f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba positions
        // foreach (Transform eachChild in enemies.transform)
        // {
        //     eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().startPosition;
        // }

        // reset score
        // jumpOverGoomba.score = 0;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position, took this out cuz i trying to gameRestart camera from camera controller
        // gameCamera.position = new Vector3(0, 1, -10);

        // foreach (QuestionBox box in questionBoxes)
        // {
        //     box.ResetBox();
        // }

        // foreach (Coin_spawn brickcoin in coin_Spawns)
        // {
        //     brickcoin.ResetBrickCoins();
        // }

        // foreach (Bouncebox brick in bricks)
        // {
        //     brick.resetBrick();
        // }
    }


    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

}
