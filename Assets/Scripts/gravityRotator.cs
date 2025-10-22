using UnityEngine;

public class GravityRotator : MonoBehaviour
{
    [Header("Gravity Controller")]
    public GravityController gravityController;

    public Transform player;
    public Transform cameraTransform;
    public float rotationSpeed = 180f; // degrees per second

    private bool isRotating = false;
    private Quaternion targetRotation;   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isRotating)
        {
            // randomly pick 90, 180, or 270 degrees
            float[] angles = { 90f, 180f, 270f };
            float rotationAmount = angles[Random.Range(0, angles.Length)];

            // rotate gravity the amount chosen above
            RotateGravity(rotationAmount);
        }
        if (isRotating)
        {
            // adjust player and camera rotation to match gravity rotation
            player.rotation = Quaternion.RotateTowards(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // if player is oriented correctly, stop rotating
            if (Quaternion.Angle(player.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
            }
        }
    }
    void RotateGravity(float degrees)
    {
        gravityController.RotateGravity(degrees);
        targetRotation = Quaternion.Euler(0, 0, player.eulerAngles.z - degrees);
        isRotating = true;
    }
}
