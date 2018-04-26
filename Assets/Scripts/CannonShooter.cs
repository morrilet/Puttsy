using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShooter : MonoBehaviour
{
    private GameObject ball;
    private SpriteRenderer ballRenderer;
    private Rigidbody2D ballRigidbody;
    private bool hasBall = false;

    private Transform shootPos;
    private CannonSwivel swivel;

    private const float FORCE = 15.0f;
    private Vector2 forceDir;

    private void Start()
    {
        ball = GameObject.Find("Ball");
        ballRenderer = ball.GetComponent<SpriteRenderer>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();

        swivel = transform.parent.GetComponentInChildren<CannonSwivel>();
        forceDir = Vector2.zero;

        Transform[] children = transform.parent.GetComponentsInChildren<Transform>();
        for(int i = 0; i < children.Length; i++)
        {
            if(children[i].tag == "ShootPos")
            {
                shootPos = children[i];
                break;
            }
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && hasBall)
        {
            AudioManager.instance.PlayEffect("CastleDrop", 0.5f);
            ShootBall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.Equals(ball)
            && GameManager.instance.finishedPutt)
        {
            TakeBall();
        }
    }

    private void TakeBall()
    {
        hasBall = true;
        ballRenderer.enabled = false;
        swivel.enabled = true;

        GameManager.instance.ballInCannon = true;
    }

    private void ShootBall()
    {
        hasBall = false;

        ballRenderer.enabled = true;
        swivel.enabled = false;

        ball.transform.position = shootPos.position;
        ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, 0.0f);

        ball.AddComponent<BallFlight>();

        forceDir = -swivel.transform.right;
        ballRigidbody.AddForce(forceDir * FORCE, ForceMode2D.Impulse);

        GameManager.instance.ballInCannon = false;
    }
}
