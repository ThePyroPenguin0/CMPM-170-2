using UnityEngine;

public class GravityRotator : MonoBehaviour
{
    [Header("Gravity Controller")]
    public GravityController gravityController;

    [Header("Player")]
    public PlayerController player;
    public Transform cameraTransform;

    public float rotationSpeed = 180f;

    private bool isRotating = false;
    private Quaternion targetRotation;
    private Collider2D playerCollider;

    [Header("Die Roll Settings")]
    [SerializeField] private float rollInterval = 5f;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float rollTimer = 5f;

    void Start()
    {
        playerCollider = player.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRotating)
        {
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(player.transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
                playerCollider.enabled = true;
            }
        }
        else
        {
            rollTimer -= Time.fixedDeltaTime;
            if (rollTimer <= 0f)
            {
                float roll = Random.value;
                if (roll < 0.25f)
                {
                    int[] angles = { 0, 90, 180, 270 };
                    int rotationAmount = angles[Random.Range(0, angles.Length)];
                    player.direction = rotationAmount % 360 / 90;
                    RotateGravity(rotationAmount);

                    rollInterval = 5f;
                    rollTimer = rollInterval;
                    Debug.Log($"Rolled a {roll}, time for a {rotationAmount} degree rotation! :)");
                }
                else
                {
                    rollInterval = Mathf.Max(minInterval, rollInterval - 1f);
                    rollTimer = rollInterval;
                    Debug.Log($"Rolled a {roll}, you survive this time. New interval: {rollInterval} seconds. :(");
                }
            }
        }
    }
    public void RotateGravity(int degrees)
    {
        switch (degrees)
        {
            case 0:
                gravityController.gravDirection = 0;
                Debug.Log("Gravity direction set to 0 (0 degrees)");
                break;
            case 90:
                gravityController.gravDirection = 1;
                Debug.Log("Gravity direction set to 1 (90 degrees)");
                break;
            case 180:
                gravityController.gravDirection = 2;
                Debug.Log("Gravity direction set to 2 (180 degrees)");
                break;
            case 270:
                gravityController.gravDirection = 3;
                Debug.Log("Gravity direction set to 3 (270 degrees)");
                break;
        }
        gravityController.RotateGravity();
        targetRotation = Quaternion.Euler(0, 0, degrees);
        isRotating = true;
        playerCollider.enabled = false;
    }
}
