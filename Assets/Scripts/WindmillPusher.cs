using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is attached to the windy zone of a windmill object.
/// </summary>
public class WindmillPusher : MonoBehaviour
{
    private GameObject ball;
    private Collider2D coll;
    private Vector3 dir;

    //Perhaps add use a curve to handle force along the length of the zone in the future.
    //For now, simple distance checks with a max force and a dropoff.
    private const float MAX_FORCE = 15f;
    private const float DROPOFF_DISTANCE = 1.5f; //At what distance do we start applying dropoff?
    private const float DROPOFF_AMOUNT = 1.5f; //How much dropoff for each unit after the dropoff distance?

    private void Start()
    {
        ball = GameObject.Find("Ball"); //Do this... Better.
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        dir = transform.parent.forward;

        if (coll.IsTouching(ball.GetComponent<Collider2D>())
            && GameManager.instance.finishedPutt
            && !GameManager.instance.ballInHole)
        {
            PushBall(ball.GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.Equals(ball))
        {
            GameManager.instance.windmillsTouchingBall += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.Equals(ball))
        {
            GameManager.instance.windmillsTouchingBall -= 1;
        }
    }

    private void PushBall(Rigidbody2D ball)
    {
        //Use parent here to get the correct side of the wind zone. Maybe get the actual edge later.
        float distance = Vector3.Distance(transform.parent.position, ball.position);

        float force = MAX_FORCE;
        if (distance >= DROPOFF_DISTANCE)
        {
            float distPast = distance - DROPOFF_DISTANCE;
            force = Mathf.Clamp(MAX_FORCE - (distPast * DROPOFF_AMOUNT), 0, MAX_FORCE);
        }

        ball.AddForce(dir.normalized * force);
    }
}
