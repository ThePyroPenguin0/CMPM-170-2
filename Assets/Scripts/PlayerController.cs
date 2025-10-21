using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public GameObject player;
    public int direction = 5; // I was thinking 0 for original orientation, going in 90 degree clockwise increments (so 90 is 1, 180 is 2, etc)
    public float doubleJumpTimer = 0.8f;
    public float jumpForce = 7f;
    private bool doubleJumped = false;
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        directionUpdate();
    }
    
    private void directionUpdate()
    {
        switch (direction)
        {
            case 0:
                player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // to do - original orientation. This is just a a placeholder command
                break;
            case 1:
                player.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); // to do - 90 degrees clockwise
                break;
            case 2:
                player.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime); // to do - 180 degrees
                break;
            case 3:
                player.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); // to do - 270 degrees
                break;
            default:
                Debug.LogError("How did you manage to pass a direction of " + direction + "?");
                break;
        }
    }
}
