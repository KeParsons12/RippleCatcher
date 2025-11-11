using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 6f;          // Top speed
    [SerializeField] private float acceleration = 3f;      // How quickly you speed up
    [SerializeField] private float deceleration = 1.5f;    // How quickly you slow down when not accelerating

    [Header("Turning Settings")]
    [SerializeField] private float turnSpeed = 120f;       // How fast you can steer
    [SerializeField] private float rotationSmoothness = 2f;// How much "lag" your turning has

    private float currentRotation;  // Actual visual rotation
    private float targetRotation;   // Desired steering rotation
    private float currentSpeed;     // Actual forward speed
    private Vector2 velocity;       // Movement vector (with inertia)

    [Header("Water Settings")]
    [SerializeField] private SpriteRenderer water;
    private Material waterMaterial;
    [SerializeField] private float waterAccelSpeed;
    [SerializeField] private float waterDecelSpeed;
    private float waterSpeed;
    private Vector2 waterDir;

    private void Start()
    {
        if (water != null) { 
            waterMaterial = water.material;
        }
    }

    private void Update()
    {
        float turnInput = Input.GetAxisRaw("Horizontal"); // A/D keys
        float moveInput = Input.GetAxisRaw("Vertical");   // W/S keys

        // Update target rotation based on steering
        targetRotation += -turnInput * turnSpeed * Time.deltaTime;
        currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * rotationSmoothness);
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);

        // Accelerate or decelerate based on input
        if (moveInput > 0f)
        {
            // Accelerate up to max speed
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            
            waterSpeed = Mathf.MoveTowards(waterSpeed, velocity.magnitude, waterAccelSpeed * Time.deltaTime);
        }
        else
        {
            // Gradually slow down (friction / water drag)
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);

            waterSpeed = Mathf.MoveTowards(waterSpeed, 0f, waterDecelSpeed * Time.deltaTime);
        }

        // Calculate target velocity based on facing direction
        Vector2 forward = transform.up * currentSpeed;

        // Smoothly update velocity for inertia/drift effect
        velocity = Vector2.Lerp(velocity, forward, Time.deltaTime * 2f);

        // Apply movement
        transform.position += (Vector3)velocity * Time.deltaTime;

        waterMaterial.SetVector("_PlayerPosition", new Vector4(-transform.position.x, -transform.position.y, 0, 0));
    }
}
