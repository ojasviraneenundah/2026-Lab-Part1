using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 2.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    // for restart
    public Vector3 startPosition = new Vector3(0.0f, 0.00f, 0.0f);

    // for delegates and events
    public delegate void GoombaStomped();
    public event GoombaStomped onGoombaStomped;

    public Animator goombaAnimator;
    public AudioSource goombaAudioSource;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        
        // where goomba is played in editor
        startPosition = transform.position;

        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // note that this is Update(), which still works but not ideal. See below.
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }


    public void Stomp()
    {
        Debug.Log("Goomba Stomped");
        onGoombaStomped?.Invoke();

        goombaAnimator.SetTrigger("stomped");
        goombaAudioSource.PlayOneShot(goombaAudioSource.clip);
        StartCoroutine(DisableAfterAnimation());
        
    }

    IEnumerator DisableAfterAnimation()
    {
        // wait for the animation to finish
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
  {
     Debug.Log(other.gameObject.name);
  }

    public void GameRestart()
    {
        gameObject.SetActive(true);
        goombaAnimator.ResetTrigger("stomped");
        transform.position = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }

}
