
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{

    private Vector3 originalPosition;
    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
        GameManager.instance.gameRestart.AddListener(GameRestart);
        originalPosition = transform.localPosition;
    }

    // void Awake()
    // {
    //     GameManager.instance.gameRestart.AddListener(GameRestart);
    // }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            

            // then destroy powerup (optional)
            // DestroyPowerup();
            gameObject.SetActive(false);
            Debug.Log("MUSHROOM POWERUP OBTAINED!");

        }
        else if (col.gameObject.layer == 10) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
    }

    public AudioSource magicMushroomAudio;

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;
        magicMushroomAudio.PlayOneShot(magicMushroomAudio.clip);
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 0.2f;
        rigidBody.AddForce(Vector2.right * 0.05f, ForceMode2D.Impulse); // move to the right
    }

    void Update()
{
    if (spawned)
    {
        Debug.Log("Mushroom position: " + transform.position);
    }
}


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
        PlayerMovement mario;
        bool result = i.TryGetComponent<PlayerMovement>(out mario);
        if(result)
        {
            //mario.MakeSuperMario();
        }

    }

    void OnDestroy()
{
    Debug.Log("MUSHROOM DESTROYED!");
}

    public void GameRestart()
    {
        this.gameObject.SetActive(true);
        spawned = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.gravityScale = 0f;

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        transform.localPosition = originalPosition;
        Debug.Log("Mushroom reset called!");
    }
}