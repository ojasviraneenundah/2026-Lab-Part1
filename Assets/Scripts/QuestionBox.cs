using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class QuestionBox : MonoBehaviour
{
    public UnityEvent<int> increaseScore;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool bouncing = false;

    // for coin
    public GameObject coin;
    private Animator coinAnimator;
    private Vector2 coinStartPosition;

    // audio sound effect for coin
    public AudioSource coinAudio;

    public AudioClip coinSound;

    // for question box to only be hit once
    private bool used = false;
    private Animator boxAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;

        // question box stays completely fixed
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Coin setup 
        coinAnimator = coin.GetComponent<Animator>();
        coinStartPosition = coin.transform.position;
        
        boxAnimator = GetComponent<Animator>();
    }

    // trigger bounce only when mario hits bottom of box
    void OnCollisionEnter2D(Collision2D collision)
    {
        // no contact
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // box has been hit already
        if (used)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // when mario hits bottom of bbox
            if (contact.normal.y > 0.5f && !bouncing)
            {
                // question box has been hit from underneath confirm
                used = true;
                StartCoroutine(Bounce());
                break;
            }
        }
    }

    // function for bounce/ oscillation for the question box
    IEnumerator Bounce()
    {
        bouncing = true;

        Vector2 topPosition = startPosition + Vector2.up * 0.15f;

        // Move box up
        while (Vector2.Distance(rb.position, topPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(
                    rb.position,
                    topPosition,
                    1.5f * Time.fixedDeltaTime
                )
            );

            yield return new WaitForFixedUpdate();
        }

        // Trigger coin as box reaches the top; only spawn coin if this box has one
        StartCoroutine(CoinPop());

 
        // Move box back down
        while (Vector2.Distance(rb.position, startPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(
                    rb.position,
                    startPosition,
                    1.5f * Time.fixedDeltaTime
                )
            );

            yield return new WaitForFixedUpdate();
        }

        rb.position = startPosition;
        bouncing = false;

        boxAnimator.SetTrigger("used");
    }


    IEnumerator CoinPop()
    {
        // Start coin rotation animation
        coinAnimator.SetTrigger("hit");

        Vector2 coinTopPosition =
            coinStartPosition + Vector2.up * 0.5f;

        coinAudio.PlayOneShot(coinSound);

        // Coin goes up
        while (Vector2.Distance(
                   coin.transform.position,
                   coinTopPosition) > 0.01f)
        {
            coin.transform.position = Vector2.MoveTowards(
                coin.transform.position,
                coinTopPosition,
                1.5f * Time.deltaTime
            );

            yield return null;
        }

        // Coin comes back down
        while (Vector2.Distance(
                   coin.transform.position,
                   coinStartPosition) > 0.01f)
        {
            coin.transform.position = Vector2.MoveTowards(
                coin.transform.position,
                coinStartPosition,
                1.5f * Time.deltaTime
            );

            yield return null;
        }

        // Reset coin only if this box has one
        coin.transform.position = coinStartPosition;
        increaseScore.Invoke(1);

    }

    // reset when restart button is pressed
    public void ResetBox()
{
    used = false;
    bouncing = false;

    // Reset box animation

        boxAnimator.ResetTrigger("used");
        boxAnimator.Play("question_box");

        coinAnimator.ResetTrigger("hit");

        coin.transform.position = coinStartPosition;
   
}
}