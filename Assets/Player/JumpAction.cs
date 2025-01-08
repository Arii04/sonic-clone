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
    void OnEnable()
    {
        playerPhysics.onGroundEnter += OnGroundEnter;
    }
    void OnGroundEnter()
    {
        currentJumps = jumps;
    }
    void OnDisable()
    {
        playerPhysics.onGroundExit -= OnGroundEnter;
    }
   
    [SerializeField] int jumps;
    [SerializeField] float jumpForce;
    [SerializeField] float airJumpForce;
    int currentJumps;
    public void Jump()
    {

      
        
        if (currentJumps <= 0) return;

        currentJumps--;

        float jumpForce = groundInfo.ground ? this.jumpForce : airJumpForce;

        RB.linearVelocity = (groundInfo.normal * jumpForce) + playerPhysics.horizontalVelocity;
    }
    
}
