using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object is attached to the ball and allows the player to fling it once, and then it's destroyed.
/// </summary>
public class BallFlinger : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrow;

    private Rigidbody2D rb;
    private Vector2 direction;

    private const float MAX_FORCE = 10.0f;
    private float force;

    private float timer = 0.0f;

    private bool putting = false;
    
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        arrow = Instantiate(arrowPrefab, this.transform) as GameObject;
        arrow.SetActive(false);
    }

    private void Update()
    {
        if(GameManager.instance.planningMode)
        {
            return;
        }

        direction = (GetMousePosition() - transform.position);
        direction.Normalize();
        force = Mathf.Abs(Mathf.Sin(timer * 2.0f) * MAX_FORCE);

        if (Input.GetButton("Fire1"))
        {
            putting = true;

            if(!arrow.activeSelf)
            {
                arrow.SetActive(true);
            }
            arrow.transform.position = this.transform.position + (arrow.transform.up * 0.2f);
            arrow.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(transform.right, direction) - 90f, transform.forward);
            arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, (force / MAX_FORCE) * 2.5f, arrow.transform.localScale.z);

            timer += Time.deltaTime;
        }

        if(Input.GetButtonUp("Fire1") && putting)
        {
            Putt();
        }
        
        //Debugging...
        Debug.DrawRay(transform.position, direction * (force / MAX_FORCE));
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Putt()
    {
        GameManager.instance.finishedPutt = true;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        AudioManager.instance.PlayEffect("Pop");

        Destroy(arrow);
        Destroy(this);
    }
}