using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    public LayerMask layerMask;
    public Rigidbody RB;
    
    public Vector3 verticalVelocity  => Vector3.Project(RB.linearVelocity, RB.transform.up);


    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump();
        }
    }

    [SerializeField] float jumpForce;
    void jump()
    {
        if (!ground) return;
        RB.linearVelocity  = Vector3.up * jumpForce;
    }



    void FixedUpdate()
    {
        Move();
        Ground();
        if(!ground) Gravity();
       
    }

    [SerializeField] float speed;
    void Move()
    {
        RB.linearVelocity = (Vector3.right * Input.GetAxis("Horizontal") * speed) + (Vector3.forward * Input.GetAxis("Vertical") * speed) + verticalVelocity;
    }




    [SerializeField] float gravity;

    void Gravity()
    {
        RB.linearVelocity -= Vector3.up * gravity * Time.deltaTime;
    }

    [SerializeField] float groundDistance;

    bool ground;
    void Ground()
    {
         ground = Physics.Raycast(RB.worldCenterOfMass, -RB.transform.up, out RaycastHit hit, groundDistance, layerMask ,QueryTriggerInteraction.Ignore);
    }
}

