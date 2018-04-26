using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Collider2D fastCollider; //The small collider to use if the ball is going fast.
    public Collider2D slowCollider; //The larger, more leinient collider to use if the ball is slow.

    private GameObject ball;
    private Rigidbody2D ballRB;

    private const float FAST_SPEED = 3.0f;

    private void Start()
    {
        ball = GameObject.Find("Ball");
        ballRB = ball.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, ball.transform.position);

        if(distance < (fastCollider as CircleCollider2D).radius) //We always hit if we hit the fast collider.
        {
            CollectBall();
        }
        else if (distance < (slowCollider as CircleCollider2D).radius) //We only hit the slow collider if we're moving slow enough.
        {
            if(ballRB.velocity.magnitude < FAST_SPEED)
            {
                CollectBall();
            }
        }
        
        if(distance > (fastCollider as CircleCollider2D).radius + 1.0f
            || distance > (slowCollider as CircleCollider2D).radius + 1.0f)
        {
            Debug.Log("STOP!");
            StopAllCoroutines();
        }

        //Debug.Log("Ball speed: " + ballRB.velocity.magnitude);
    }

    private void CollectBall()
    {
        StartCoroutine(CollectBall_Coroutine());
    }

    private IEnumerator CollectBall_Coroutine()
    {
        float distance = Vector2.Distance(ball.transform.position, transform.position);
        float startDistance = distance;
        float goalDistance = 0.01f;

        Vector3 startScale = ball.transform.localScale;
        Vector3 goalScale = new Vector3(.2f, .2f, .2f);

        while (distance > goalDistance)
        {
            distance = Vector2.Distance(ball.transform.position, transform.position);

            if (distance > goalDistance)
            {
                Vector3 pos = transform.position - ball.transform.position;
                pos = pos.normalized * 0.01f * Time.deltaTime;
                ball.transform.position += pos;
            }

            yield return null;
        }

        while(ball.transform.localScale.x - goalScale.x > 0.01f)
        {
            if (ball.transform.localScale.x > goalScale.x) //Use x because they're all the same. (Circles and such.)
            {
                Vector3 scaleTemp = ball.transform.localScale;
                scaleTemp = Vector3.Lerp(startScale, goalScale, goalScale.x / startScale.x);
                scaleTemp = goalScale;
                ball.transform.localScale = scaleTemp;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.PlayEffect("Echo", 0.01f);
        GameManager.instance.ballInHole = true;
    }
}