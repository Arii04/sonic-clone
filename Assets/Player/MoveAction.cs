using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class MoveAction : PlayerAction
{


    Vector2 move;
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        move = callbackContext.ReadValue<Vector2>();
    }


    

    void OnEnable()
    { 
       playerPhysics.onPlayerPhysicsUpdate += Move;
    }

    void OnDisable()
    {
        playerPhysics.onPlayerPhysicsUpdate -= Move;
    }
    [SerializeField] Transform cameraTrasform;

    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxSpeed;
    [SerializeField] float minTurnSpeed;
    [SerializeField] float maxTurnSpeed;
    [SerializeField, Range(0, 1)] float turnDeceleration;
    [SerializeField, Range(0, 1)] float softBrakeThreshold;
    [SerializeField] float brakespeed;
    [SerializeField] float brakeThreshold;
    [SerializeField] float brakeTime;

    bool braking;

    float brakeTimer;
    
    void Move()
    {

        Vector3 moveVector=GetMoveVector(cameraTrasform, groundInfo.normal, move);

        bool wasBraking = braking;

        braking = groundInfo.ground;

        braking &= playerPhysics.speed > RB.sleepThreshold;

        braking &= braking && brakeTime > 0 && brakeTimer > 0
            || Vector3.Dot(moveVector.normalized, playerPhysics.horizontalVelocity) < -brakeThreshold;

        if(braking)
        {
            brakeTimer -= Time.deltaTime;
        }

        if(braking && !wasBraking)
        {
            brakeTimer = brakeTime;
        }

        if (braking)
        {
            Decelerate(brakespeed);
        }

        else if (move.magnitude > 0 )
        {
            if(Vector3.Dot(moveVector.normalized, playerPhysics.horizontalVelocity.normalized) >= (groundInfo.ground ? -softBrakeThreshold : 0))
            { 
            Accelerate(acceleration);
            }
            else
            {
                Decelerate(brakespeed);
            }
        }
        else
        {
            Decelerate(deceleration);
        }

        void Accelerate(float speed)
        {

            float maxRadDelta = Mathf.Lerp(minTurnSpeed,maxSpeed, playerPhysics.speed / maxSpeed) * Mathf.PI * Time.deltaTime;

            float maxDistDelta = speed * Time.deltaTime;

            Vector3 velocity = Vector3.RotateTowards(playerPhysics.horizontalVelocity, moveVector * maxSpeed, maxRadDelta, maxDistDelta);

            velocity -= velocity * (Vector3.Angle(playerPhysics.horizontalVelocity, velocity) / 180 * turnDeceleration);

            RB.linearVelocity  = velocity+ playerPhysics.verticalVelocity;

        }

        void Decelerate(float speed)
        {
            RB.linearVelocity = Vector3.MoveTowards(playerPhysics.horizontalVelocity, Vector3.zero, speed* Time.deltaTime) + playerPhysics.verticalVelocity;

        }


    }

   Vector3 GetMoveVector(Transform relativeTo, Vector3 upNormal, Vector2 move)
    {
        Vector3 rightNormal = Vector3.Cross(upNormal, relativeTo.forward);

        Vector3 forwardNormal = Vector3.Cross(relativeTo.right, upNormal);

        Vector3.OrthoNormalize(ref upNormal,ref forwardNormal ,ref rightNormal );

        Debug.DrawRay(RB.transform.position, rightNormal *10, Color.red);

        Debug.DrawRay(RB.transform.position, forwardNormal * 10, Color.green);

        return (rightNormal * move.x )+ (forwardNormal * move.y);
    }


}




