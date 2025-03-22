using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (playerTransform == null)
            return;

        Vector3 currentPosition = transform.position;

        // Only update the Y position to match the player
        Vector3 targetPosition = new Vector3(
            currentPosition.x,
            playerTransform.position.y,
            currentPosition.z
        );

        // Smoothly move to the target position
        Vector3 smoothedPosition = Vector3.Lerp(
            currentPosition,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // Apply the new position
        transform.position = smoothedPosition;
    }
}