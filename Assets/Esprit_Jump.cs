﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esprit_Jump : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    public void Jump()
    {
        Vector3 wantedVelocity = new Vector2(rb.velocity.x, 30f);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, wantedVelocity, ref velocity, .05f);
    }
}
