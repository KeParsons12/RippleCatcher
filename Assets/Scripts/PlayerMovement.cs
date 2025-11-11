using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boatRotateSpeed;

    private Vector2 currentWaterDir = Vector2.zero;
    //[SerializeField] private float waterSmoothSpeed = 5f;
    [SerializeField] private SpriteRenderer water;
    private Material waterMaterial;

    private void Start()
    {
        waterMaterial = water.material;

        waterMaterial.SetFloat("_WaterSpeed", moveSpeed);
    }

    private void Update()
    {
        // move input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 inputDir = new Vector2(moveX, moveY);

        // point object in direction of movement
        RotateInDirection(inputDir);

        // update the water to move in direction of input
        //currentWaterDir = Vector2.Lerp(currentWaterDir, inputDir.normalized, Time.deltaTime * waterSmoothSpeed);
        //waterMaterial.SetVector("_WaterDirection", new Vector4(-currentWaterDir.x, -currentWaterDir.y, 0, 0));

        waterMaterial.SetVector("_WaterDirection", new Vector4(-inputDir.x, -inputDir.y, 0, 0));
    }

    private void RotateInDirection(Vector2 direction)
    { 
        if (direction.sqrMagnitude > 0.01f)
        {
            // smooth transition
            Quaternion targetRotation = Quaternion.Euler(0, 0, CalculateAngle(direction));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * boatRotateSpeed);

            // instant snap
            //transform.rotation = Quaternion.Euler(0,0, CalculateAngle(direction));
        }
    }

    private float CalculateAngle(Vector2 direction)
    {
        // Calculate angle in degrees
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
    }
}
