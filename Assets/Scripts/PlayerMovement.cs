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

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }
    }

    // to jump
    public float upSpeed = 10;
    private bool onGroundState = true;

    // when you collide on the ground, ongroundstate becomes true
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")){
            onGroundState = true;
        }
    }
    
    // just mario moving left and right without sliding and others
    public float maxSpeed = 20;

    void FixedUpdate(){
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // verify that it doesnt go above maxSpeed
            if(marioBody.linearVelocity.magnitude < maxSpeed){
                marioBody.AddForce(movement * speed);
            }
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // shtop
            marioBody.linearVelocity = Vector2.zero;
        }

        if(Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
         
    }

    void OnTriggerEnter2D(Collider2D other)
  {
      if (other.gameObject.CompareTag("Enemy"))
      {
          Debug.Log("Collided with goomba!");
        //   Time.timeScale = 0.0f;
          gameManager.GameOver();
      }
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
    
    // NEED TO FIX THE ADDITIONAL PHYSICS THAT BOTH MARIO AND GOOMBA GET WHEN RESTART BUTTON IS PRESSED
    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-0.86f, 0.68f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        // reset score
        jumpOverGoomba.score = 0;

    }
    
    
}
