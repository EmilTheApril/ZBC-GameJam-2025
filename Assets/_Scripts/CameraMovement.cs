using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraPosHeight = 20f; // Height increment (0, 20, 40, etc.)
    [SerializeField] private float thresholdDistance = 10.5f; // Distance threshold to trigger camera movement

    private float currentCameraY = 0f;

    private void Update()
    {
        ChangeCameraHeight();
    }

    public void ChangeCameraHeight()
    {
        float playerY = transform.position.y;
        float cameraY = Camera.main.transform.position.y;

        float distance = playerY - cameraY;

        if (Mathf.Abs(distance) > thresholdDistance)
        {
            if (distance > 10)
            {
                currentCameraY += cameraPosHeight;
            }
            else
            {
                currentCameraY -= cameraPosHeight;
            }

            Camera.main.transform.position = new Vector3(0, currentCameraY, -10);
        }
    }
}