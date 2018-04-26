using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSwivel : MonoBehaviour
{
    private const float ROTATION_SPEED = 0.5f;

    public Vector2 swivelLimits;

    private void Update()
    {
        //The forward vector of the base.
        Vector3 baseForward = -transform.parent.right;
        baseForward.z = 0;
        baseForward.Normalize();

        Vector3 origin = transform.position;
        origin.z = 0;

        Vector3 target = GetMousePosition();
        target.z = 0;

        Vector3 trueRight = -transform.up;
        trueRight.z = 0;
        trueRight.Normalize();

        Vector3 trueForward = -transform.right;
        trueForward.z = 0;
        trueForward.Normalize();

        Vector3 directionToTarget = (target - origin).normalized;

        //One dot product to check the alignment to forward, another to check which side the cursor is on.
        float dot = Vector2.Dot(trueForward, directionToTarget);
        float side = Vector3.Dot(trueRight, directionToTarget);

        float angleFromBase = Vector3.Angle(trueForward, baseForward);
        angleFromBase *= Mathf.Sign(Vector3.Dot(trueRight, baseForward));

        Quaternion rot = transform.rotation;
        if (dot < 0.999f)
        {
            if (side < 0) //Turn LEFT
            {
                if (angleFromBase < swivelLimits.y)
                {
                    rot *= Quaternion.Euler(Quaternion.Inverse(rot) * new Vector3(0, 0, ROTATION_SPEED));
                }
            }
            else if (side > 0) //Turn RIGHT
            {
                if (angleFromBase > swivelLimits.x)
                {
                    rot *= Quaternion.Euler(Quaternion.Inverse(rot) * new Vector3(0, 0, -ROTATION_SPEED));
                }
            }
        }
        transform.rotation = rot;

        /*
        Debug.Log(angleFromBase);
        
        Debug.DrawRay(origin, directionToTarget, Color.blue);
        Debug.DrawRay(origin, trueRight, Color.red);
        Debug.DrawRay(origin, trueForward, Color.yellow);

        Debug.DrawRay(origin, baseForward * 1.5f, Color.gray);
        */
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
