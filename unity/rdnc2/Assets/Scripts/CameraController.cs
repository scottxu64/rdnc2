using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float leftLimit = -100f;
    public float rightLimit = 100f;
    public float speed = 1f;

    public float totalMovement = 0f;    // to display value in game mode

    private void FixedUpdate()
    {
        float input = Input.acceleration.x;
        if (input == 0) input = Input.GetAxisRaw("Horizontal");

        totalMovement = Mathf.Clamp(totalMovement + input, leftLimit, rightLimit);
        if (input != 0) Debug.Log("input=" + input + ", totalMovement=" + totalMovement);

        if (totalMovement < rightLimit && totalMovement > leftLimit)
        {
            transform.RotateAround(target.position, Vector3.up, input * speed);
        }
    }
}
