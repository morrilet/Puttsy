using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutterPusher : MonoBehaviour
{
    private GameObject ball;
    private Collider2D coll;
    private Vector3 dir;
    private Animator anim;
    
    private const float FORCE = 8.0f;
    private const float PUTT_INTERVAL = 1.0f;

    private float timer = PUTT_INTERVAL;

    private void Start()
    {
        ball = GameObject.Find("Ball"); //Do this... Better.
        coll = GetComponent<Collider2D>();
        anim = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        dir = transform.parent.forward;
        if (timer <= PUTT_INTERVAL)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.Equals(ball) 
            && timer >= PUTT_INTERVAL
            && GameManager.instance.finishedPutt)
        {
            PushBall(ball.GetComponent<Rigidbody2D>());
        }
    }

    private void PushBall(Rigidbody2D ball)
    {
        timer = 0.0f;

        //Use parent here to get the correct side of the wind zone. Maybe get the actual edge later.
        float distance = Vector3.Distance(transform.parent.position, ball.position);
        anim.SetTrigger("Putt");

        AudioManager.instance.PlayEffect("Putter", 0.75f);

        ball.AddForce(dir.normalized * FORCE, ForceMode2D.Impulse);
    }
}
