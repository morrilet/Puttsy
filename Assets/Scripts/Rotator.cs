﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis;
    public float speed;

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(speed, axis);
    }
}