using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This should be attached to any object that the player an click and drag onto the grid.
/// </summary>
public class ClickAndDrag : MonoBehaviour
{
    private Collider coll;
    //private SpriteRenderer sr;

    //Sorting layer values to apply on release.
    //[HideInInspector]
    //public string sortingLayerName;
    //[HideInInspector]
    //public int sortingOrder;

    //The mask to search for when trying to snap to a position on release.
    //Public in case I add things that snap to the green.
    [HideInInspector]
    public LayerMask snapMask;

    //Offsets to use for 'pretty' dragging.
    private Vector2 currentOffset;
    private Vector2 storedOffset;

    //Duh. :P
    private bool isGrabbed = false;
    private bool isGrabbed_Prev = false;

    //Used for rejecting the placement due to tower overlap.
    private List<GameObject> touchingTowers;

    private void Start()
    {
        coll = this.GetComponent<Collider>();
        touchingTowers = new List<GameObject>();
    }

    private void Update()
    {
        currentOffset = GetMousePosition() - transform.position;

        if(Input.GetButtonDown("Fire1") && IsUnderMouse() && !isGrabbed)
        {
            Grab();
        }

        if(Input.GetButtonUp("Fire1") && isGrabbed)
        {
            Release();
        }

        if(Input.GetButtonDown("Fire2") && !isGrabbed && IsUnderMouse())
        {
            Rotate(90f);
        }

        if (isGrabbed)
        {
            transform.position = (Vector2)GetMousePosition() - storedOffset;
            transform.position = new Vector3(transform.position.x, transform.position.y, -6);
        }

        if (!isGrabbed && isGrabbed_Prev)
        {
            StartCoroutine(HandleTowerPlacement());
        }

        isGrabbed_Prev = isGrabbed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            if (!touchingTowers.Contains(collision.gameObject))
            {
                touchingTowers.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            if (touchingTowers.Contains(collision.gameObject))
            {
                touchingTowers.Remove(collision.gameObject);
            }
        }
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsUnderMouse()
    {
        Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit hit;
        Physics.Raycast(origin, Vector3.forward, out hit, Mathf.Infinity);

        if(hit.collider != null && hit.collider.Equals(coll))
        {
            return true;
        }
        return false;
    }

    private void Rotate(float degrees)
    {
        Quaternion rot = transform.rotation;
        rot *= Quaternion.Euler(0, degrees, 0);
        transform.rotation = rot;
    }

    public void Grab()
    {
        //Debug.Log("Grabbed!");
        storedOffset = currentOffset;
        isGrabbed = true;
    }

    public void Release()
    {
        //Debug.Log("Released!");
        storedOffset = Vector2.zero;
        isGrabbed = false;

        //sr.sortingLayerName = sortingLayerName;
        //sr.sortingOrder = sortingOrder;

        //Try to snap, if not destroy ourselves.
        Vector3 origin = GetMousePosition();
        origin.z = Mathf.NegativeInfinity;
        RaycastHit2D snapHit = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, snapMask);

        if (snapHit.transform != null)
        {
            transform.position = snapHit.transform.position;
            AudioManager.instance.PlayEffect("Snap", 0.5f);
        }
        else if (snapHit.transform == null)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HandleTowerPlacement()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //Scrub the touching list of null entries.
        touchingTowers = touchingTowers.Where(x => x != null).ToList();

        if (touchingTowers.Count != 0)
        {
            Destroy(this.gameObject);
        }
    }
}
