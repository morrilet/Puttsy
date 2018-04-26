using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is attached to the ball and allows the ball to glide over the grid.
/// For now it will simply ignore wall collisions until the ball is back on the green.
/// </summary>
public class BallFlight : MonoBehaviour
{
    private Collider2D coll;
    private bool isFlying = true;

    private void Awake()
    {
        coll = this.GetComponent<Collider2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Ball"));
    }

    private void Update()
    {
        Vector3 origin = transform.position;
        origin.z = Mathf.NegativeInfinity;
        RaycastHit2D hitGreen = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Green"));
        RaycastHit2D hitWall = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));

        if(hitGreen.transform != null) //Above the ground.
        {
            Land();
        }
        else if (hitWall.transform == null) //Above nothing.
        {
            Fall();
        }
    }

    private void Land()
    {
        isFlying = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Ball"), false);
        Destroy(this);
    }

    private void Fall()
    {
        isFlying = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Ball"), false);
        StartCoroutine(ShrinkBall(0.75f));
    }

    private IEnumerator ShrinkBall(float duration)
    {
        Vector3 startScale = transform.localScale;
        float timer = 0.0f;

        while (timer < duration)
        {
            Vector3 newScale = transform.localScale;
            newScale = Vector3.Lerp(startScale, Vector3.zero, timer / duration);
            transform.localScale = newScale;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(this);
    }
}
