using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Header("Rigidbody")]
    public Rigidbody2D body;
    
    public Vector2 gravityDirection = Vector2.down; // gravity starts pointing down
    public float gravityStrength = 9.81f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
            body.AddForce(gravityDirection * gravityStrength);
    }
    // pass in degrees you want rotated and gravity will shift accordingly
    public void RotateGravity(float degreesClockwise)
    {
        gravityDirection = Quaternion.Euler(0, 0, -degreesClockwise) * gravityDirection;
    }
}
