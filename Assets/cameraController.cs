using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Controller : PlayerAction
{
    public float player_speed;
    public float last_speed;
    public float currentFov; //currentQuantity
    public float desiredFov; //desiredQuantity
    const float zoomStep = 4.0f;
    const float minFov = 60f; // Minimum FOV value
    const float maxFov = 120f; // Maximum FOV value

    void Start()
    {
        currentFov = 60f; // Default FOV value
        desiredFov = currentFov;
        last_speed = playerPhysics.RB.linearVelocity.magnitude;
    }

    void CheckSpeed()
    {
        if (player_speed < last_speed)
        {
            last_speed = player_speed;
            desiredFov = minFov;
        }
        else if (player_speed > last_speed)
        {
            last_speed = player_speed;
            desiredFov = maxFov;
        }
    }

    void ProcessFOV()
    {
        currentFov = Mathf.MoveTowards(currentFov, desiredFov, zoomStep * Time.deltaTime);
        currentFov = Mathf.Clamp(currentFov, minFov, maxFov); // Clamp the FOV value
    }

    void SetFOV()
    {
        Camera.main.fieldOfView = currentFov;
    }

    void Update()
    {
        player_speed = playerPhysics.RB.linearVelocity.magnitude;
        CheckSpeed();
        ProcessFOV();
        SetFOV();
    }
}
