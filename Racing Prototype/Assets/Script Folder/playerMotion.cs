using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMotion : MonoBehaviour
{
    Rigidbody pRB;
    [SerializeField] Vector3 appliedForce;
    [SerializeField] float maxSpeed, maxForce, accelerationForce, decellerateForce, xZPlaneSpeed, jumpForce, turnSpeed,turnScaler;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundTransform, slopeTransform;

    [SerializeField] Vector3 rampNormal, rampHitPoint;

    Ray groundCheckRay;
    Ray rampRay;
    private void Awake()
    {
        pRB = this.gameObject.GetComponent<Rigidbody>();
        pRB.drag = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        groundCheckRay = new Ray(groundTransform.position, -transform.up);
        rampRay = new Ray(slopeTransform.position, transform.forward);
        SlopeCheck();
        Motion();
    }

    Vector3 CalculateForce()
    {
        return Vector3.zero;
    }

    void GroundCheck()
    {
        RaycastHit rH;
        if (Physics.Raycast(groundTransform.position, -transform.up, out rH, 0.5f))
        {
            if (rH.collider.gameObject.tag == "Ground")
            {
                 isGrounded =true;
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    void SlopeCheck()
    {
        RaycastHit rH;
        if (Physics.Raycast(rampRay, out rH, 1f))
        {
            if (rH.collider.tag == "Ground")
            {
                rampNormal= rH.normal;
                rampHitPoint = rH.point;
            }
            
        }
    }

    void Motion()
    {
        pRB.drag = 0;
        
        var velocity = pRB.velocity;

        //turning
        float turning = Input.GetAxis("Horizontal");
        pRB.AddRelativeTorque(transform.up * turning * (turnSpeed*(maxSpeed/Mathf.Max(15f, velocity.magnitude*turnScaler))));


        xZPlaneSpeed = Mathf.Sqrt((velocity.x * velocity.x) + (velocity.z * velocity.z));
        if (xZPlaneSpeed < maxSpeed)
        {
            pRB.AddForce(transform.forward * accelerationForce*Input.GetAxis("Vertical"), ForceMode.VelocityChange);
        }

        if (xZPlaneSpeed >= maxSpeed)
        {
            Vector3 normalizedXZMovement = new Vector3(velocity.x / xZPlaneSpeed, velocity.y, velocity.z / xZPlaneSpeed);

            Vector3 xzSpeed = new Vector3(normalizedXZMovement.x * maxSpeed, velocity.y, normalizedXZMovement.z * maxSpeed);

            pRB.velocity = xzSpeed;
        }

        if(Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            pRB.AddForce(transform.up * jumpForce,ForceMode.VelocityChange);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(slopeTransform.position, transform.forward+slopeTransform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rampHitPoint, rampNormal+rampHitPoint);
        
    }
}
