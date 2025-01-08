using System;
using System.Collections;
using UnityEngine;


public class PlayerPhysics : MonoBehaviour
{
    public LayerMask layerMask;
    public Rigidbody RB;
    
    public Vector3 verticalVelocity  => Vector3.Project(RB.linearVelocity, RB.transform.up);
    public Vector3 horizontalVelocity => Vector3.ProjectOnPlane(RB.linearVelocity, RB.transform.up);

    public float verticalSpeed => Vector3.Dot(RB.linearVelocity, RB.transform.up);


    public Action onPlayerPhysicsUpdate;

    public float speed => horizontalVelocity.magnitude;

    void FixedUpdate()
    {
       
        onPlayerPhysicsUpdate?.Invoke();
        
        if(!groundInfo.ground) Gravity();

        if(groundInfo.ground && verticalSpeed < RB.sleepThreshold) RB.linearVelocity = horizontalVelocity;

        StartCoroutine(LateFixedUpdateRoutine());

        IEnumerator LateFixedUpdateRoutine()
        { 
            yield return new WaitForFixedUpdate();
            LateFixedUpdate();
        }
       
    }

  



    [SerializeField] float gravity;

    void Gravity()
    {
        RB.linearVelocity -= Vector3.up * gravity * Time.deltaTime;
    }

    void LateFixedUpdate()
    {
        Ground();

        Snap();

        if (groundInfo.ground) RB.linearVelocity = horizontalVelocity;

    }

    [SerializeField] float groundDistance;

    public struct GroundInfo
    {
        public Vector3 point;
        public Vector3 normal;
        public bool ground;
    }

    [HideInInspector] public GroundInfo groundInfo;

    public Action onGroundEnter;
    public Action onGroundExit;
    void Ground()
    {

        float maxDistance = Mathf.Max(RB.centerOfMass.y, 0) + (RB.sleepThreshold * Time.fixedDeltaTime);

        if (groundInfo.ground && verticalSpeed < RB.sleepThreshold) 
            maxDistance += groundDistance;
        
       bool ground = Physics.Raycast(RB.worldCenterOfMass, -RB.transform.up, out RaycastHit hit, maxDistance, layerMask ,QueryTriggerInteraction.Ignore);

        Vector3 point = ground ? hit.point : RB.transform.position;

        Vector3 normal = ground ? hit.normal: Vector3.up;

        if (ground != groundInfo.ground)
        {
            if (ground) 
            {
                onGroundEnter?.Invoke(); 
            }
            else

            {
                onGroundExit?.Invoke();
            }

        }
        

        groundInfo = new () 
        {   
            point = point,
            normal = normal,
            ground = ground 
        };
    }


    void Snap() 
    { 
        RB.transform.up = groundInfo.normal;

        Vector3 goal = groundInfo.point;

        Vector3 difference = goal - RB.transform.position;

        if (RB.SweepTest(difference, out _, difference.magnitude, QueryTriggerInteraction.Ignore)) return;

        RB.transform.position = goal;
    }
}

