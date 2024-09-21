using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float driftFactor = 0.3f; // Not as applicable in 3D but can be tweaked for lateral friction
    public float accelerationFactor = 5f;
    public float turnFactor = 5f;
    public float maxSpeed = 10f;

    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;

    float velocityVsUp = 0;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ApplyForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    void ApplyForce()
    {
        velocityVsUp = Vector3.Dot(transform.forward, rb.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        if (velocityVsUp < -maxSpeed * 0.25f && accelerationInput < 0)
            return;

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        if (accelerationInput == 0 || (velocityVsUp > 0 && accelerationInput < 0) || (velocityVsUp < 0 && accelerationInput > 0))
        {
            rb.drag = Mathf.Lerp(rb.drag, 5.0f, Time.fixedDeltaTime);
        }
        else
        {
            rb.drag = 0;
        }

        Vector3 engineForceVector = transform.forward * accelerationInput * accelerationFactor;
        rb.AddForce(engineForceVector, ForceMode.Force);
    }

    void ApplySteering()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            float turnAngle = steeringInput * turnFactor * Time.fixedDeltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    void KillOrthogonalVelocity()
    {
        Vector3 forwardVel = transform.forward * Vector3.Dot(rb.velocity, transform.forward);
        Vector3 rightVel = transform.right * Vector3.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVel + rightVel * driftFactor;
    }

    public void SetInputVector(Vector2 input)
    {
        steeringInput = input.x;
        accelerationInput = input.y;
    }
}
