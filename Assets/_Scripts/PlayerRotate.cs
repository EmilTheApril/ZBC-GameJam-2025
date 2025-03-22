using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private float rotationTime = 1f;
    [SerializeField] private float jumpForce;
    private bool isRotating = false;
    private float currentRotationTime = 0f;
    private float startRotation = 0f;
    private bool rotateClockwise = false;
    private bool jumpingHeld;
    public bool isGrounded { get; private set; }
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isRotating)
        {
            RotatePlayer();
        }
        Jump();
        CheckIfGrounded();
    }

    public void OnJump(InputValue value)
    {
        jumpingHeld = value.Get<float>() == 1 ? true : false;
    }

    public void Jump()
    {
        if (!jumpingHeld || !isGrounded) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void CheckIfGrounded()
    {
        RaycastHit2D ground = Physics2D.Raycast(transform.position - new Vector3(0, .51f), Vector2.down, 0.1f);

        isGrounded = ground.collider == null ? false : true;
    }

    public void Rotate()
    {
        rotateClockwise = !gunSpriteRenderer.flipY;
        StartRotation();
    }

    private void StartRotation()
    {
        isRotating = true;
        currentRotationTime = 0f;
        playerSprite.transform.eulerAngles = new Vector3(0, 0, 0);
        startRotation = playerSprite.transform.eulerAngles.z;
    }

    public void RotatePlayer()
    {
        currentRotationTime += Time.deltaTime;
        float progress = currentRotationTime / rotationTime;

        float easedProgress = 1 - Mathf.Pow(1 - progress, 3);

        if (progress < 1f)
        {
            float rotationAmount = 360f * easedProgress;
            float newRotation;

            if (rotateClockwise)
            {
                newRotation = startRotation + rotationAmount;
            }
            else
            {
                newRotation = startRotation - rotationAmount;
            }

            playerSprite.transform.eulerAngles = new Vector3(0, 0, newRotation);
        }
        else
        {
            float finalRotation = rotateClockwise ?
                startRotation :
                startRotation - 360f;

            playerSprite.transform.eulerAngles = new Vector3(0, 0, finalRotation % 360);
            isRotating = false;
        }
    }
}
