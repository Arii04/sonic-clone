using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAction : PlayerAction
{

  

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Jump();
        }
    }


        [SerializeField] float jumpForce;
        void Jump()
        {
            if (!groundInfo.ground) return;
            RB.linearVelocity = (Vector3.up * jumpForce) + playerPhysics.horizontalVelocity;
        }
    
}
