using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Header("Rigidbody")]
    public Rigidbody2D body;

    [Header("Player Interaction")]
    public PlayerController player;

    public Vector2 gravityDirection = Vector2.down;
    public int gravDirection = 0;
    public float gravityStrength = 9.81f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.AddForce(gravityDirection * gravityStrength);
    }
    // pass in degrees you want rotated and gravity will shift accordingly
    public void RotateGravity()
    {
        switch (gravDirection)
        {
            case 0:
                gravityDirection = Vector2.down;
                Debug.Log("Up should be up. GravDirection: (should be 0): " + gravDirection);
                break;
            case 1:
                gravityDirection = Vector2.right;
                Debug.Log("Up should be right. GravDirection (should be 1): " + gravDirection);
                break;
            case 2:
                gravityDirection = Vector2.up;
                Debug.Log("Up should be down. GravDirection (should be 2): " + gravDirection);
                break;
            case 3:
                gravityDirection = Vector2.left;
                Debug.Log("Up should be left. GravDirection (should be 3): " + gravDirection);
                break;
            default:
                Debug.LogError("Gravity direction out of bounds");
                break;
        }
    }

    public void RotateObject(float degrees)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, degrees);
        transform.rotation = targetRotation;
    }
}
