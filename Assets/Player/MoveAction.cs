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



    [SerializeField] float speed;
    void Move()
    {
    RB.linearVelocity = Vector3.ProjectOnPlane((Vector3.right * move.x * speed) + (Vector3.forward * move.y * speed), groundInfo.normal) + playerPhysics.verticalVelocity;
    }

}




