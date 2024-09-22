using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Objective")]
    public int loc1 = 3;
    public int loc2 = 5;
    public int loc3 = 5;
    public int loc4 = 7;
    [Header("Player Settings")]
    public float driftFactor = 0.3f;
    public float accelerationFactor = 1.5f;
    public float turnFactor = 120f;
    public float maxSpeed = 3f;
    public int capacity = 5;
    float accelerationInput = 0;
    float steeringInput = 0;
    private GameManagerUrban gmu;
    private GameObject firstPrefabInstance1;
    private GameObject firstPrefabInstance2;
    private GameObject firstPrefabInstance3;
    private GameObject firstPrefabInstance4;
    private GameObject secondPrefabInstance;
    private int content;
    public TextMeshProUGUI obj;
    public TextMeshProUGUI cont;
    float rotationAngle = 0;

    float velocityVsUp = 0;

    Rigidbody rb;

    SPUrban sp;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        content = 0;
        sp = GameObject.Find("SpawnPositionUrban").GetComponent<SPUrban>();
        gmu = GameObject.Find("GameManagerUrban").GetComponent<GameManagerUrban>();
        obj.text = $"Pending - \nLoc1: {loc1}\nLoc2: {loc2}\nLoc3: {loc3}\nLoc4: {loc4}";
        cont.text = $"Dabbas: {content}\nCapacity: {capacity}";
        if (sp!= null)
        {
            if (sp.firstPrefabInstances.Count > 0)
            {
                firstPrefabInstance1 = sp.firstPrefabInstances[0];
                firstPrefabInstance2 = sp.firstPrefabInstances[1];
                firstPrefabInstance3 = sp.firstPrefabInstances[2];
                firstPrefabInstance4 = sp.firstPrefabInstances[3];
            }
            secondPrefabInstance = sp.secondPrefabInstance;
        }
        else
        {
            Debug.LogWarning("SPUrban script not found or not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(loc1+loc2+loc3+loc4 == 0)
        {
            gmu.GameOver();
        }
        if(loc1 + loc2 + loc3 + loc4 <= 17)
        {
            capacity = 7;
            maxSpeed = 5f;
            turnFactor = 150;
        }
        if (loc1 + loc2 + loc3 + loc4 <= 10)
        {
            capacity = 10;
            maxSpeed = 7f;
            turnFactor = 180;
        }
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

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
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
            //float turnAngle = steeringInput * turnFactor * Time.fixedDeltaTime;
            rotationAngle = steeringInput * turnFactor * Time.fixedDeltaTime;

            Quaternion turnRotation = Quaternion.Euler(0f, rotationAngle, 0f);

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


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("dabbas"))
        {
            Debug.Log("Dabbas");
        }
        else if(other.gameObject.CompareTag("delivery"))
        {
            Debug.Log("Delivery");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.E))
        {
            Debug.Log("Pressed E");
            if (other.gameObject.CompareTag("delivery"))
            {
                if (other.gameObject == firstPrefabInstance1)
                {
                    if (loc1 > content)
                    {
                        loc1 -= content;
                        content = 0;
                    }
                    else
                    {
                        content -= loc1;
                        loc1 = 0;
                    }
                }
                else if (other.gameObject == firstPrefabInstance2)
                {
                    if (loc2 > content)
                    {
                        loc2 -= content;
                        content = 0;
                    }
                    else
                    {
                        content -= loc2;
                        loc2 = 0;
                    }
                }
                else if (other.gameObject == firstPrefabInstance3)
                {
                    if (loc3 > content)
                    {
                        loc3 -= content;
                        content = 0;
                    }
                    else
                    {
                        content -= loc3;
                        loc3 = 0;
                    }
                }
                else
                {
                    if (loc4 > content)
                    {
                        loc4 -= content;
                        content = 0;
                    }
                    else
                    {
                        content -= loc4;
                        loc4 = 0;
                    }
                }
            }
            else if(other.gameObject.CompareTag("dabbas"))
            {
                content += (capacity - content);
            }
            obj.text = $"Pending - \nLoc1: {loc1}\nLoc2: {loc2}\nLoc3: {loc3}\nLoc4: {loc4}";
            cont.text = $"Dabbas: {content}\nCapacity: {capacity}";
        }
    }
}
