using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    public float thrustMax = 20f;
    [Tooltip("How responsive the plane is when rolling, pitching and yawning")]
    public float responsiveness = 10f;
    [Tooltip("How much lift force this plane generates as it gains speed")]
    public float lift = 135f;

    private float throttle;     //Percentage of maximum engine thrust currently being used.
    private float roll;         //Tilting left to right.
    private float pitch;        //Tilting front to back.
    private float yaw;          //"Turning" left to right.

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInput()
    {
        //set rotational values from axis input
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        //Handle throttle value being sure to clamp it between 0 and 100.
        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInput();
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        //Apply forces to plane
        rb.AddForce(transform.forward * thrustMax * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle " + throttle.ToString("F0") + "%\n";
        hud.text = "Airspeed " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        hud.text = "Altitude " + transform.position.y.ToString("F0") + " m";

    }
}
