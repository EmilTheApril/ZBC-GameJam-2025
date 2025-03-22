using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private float rotationTime = 1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float knockbackForceOnPlayer;
    [SerializeField] private PhysicsMaterial2D attackedMat;
    private bool isRotating = false;
    private float currentRotationTime = 0f;
    private float startRotation = 0f;
    private bool rotateClockwise = false;
    private bool jumpingHeld;
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isDisabled { get; private set; }
    [SerializeField] private float disabledTime;
    public AudioClip jumpSound;
    public AudioClip fallSound;
    private Rigidbody2D rb;
    private float timer;

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
        UnDisablePlayer();

        if (rb.linearVelocity.y != 0) timer = 0;
    }

    public void AttackPlayer(Transform otherTrans)
    {
        if (isDisabled) return;
        DisablePlayer();
        rb.linearVelocity = Vector2.zero;
        rb.linearDamping = 1;
        GetComponent<BoxCollider2D>().sharedMaterial = attackedMat;
        rb.AddForce(((new Vector2(transform.position.x, 0) - new Vector2(otherTrans.position.x, 0)).normalized + Vector2.up).normalized * knockbackForceOnPlayer, ForceMode2D.Impulse);
    }

    public void DisablePlayer()
    {
        isDisabled = true;
        timer = 0;
    }

    public void UnDisablePlayer()
    {
        if (!isDisabled) return;
        timer += Time.fixedDeltaTime;

        if(timer >= disabledTime)
        {
            isDisabled = false;
            GetComponent<BoxCollider2D>().sharedMaterial = null;
        }
    }

    public void OnJump(InputValue value)
    {
        jumpingHeld = value.Get<float>() == 1 ? true : false;
    }

    public void Jump()
    {
        if (!jumpingHeld || !isGrounded || isJumping || isDisabled) return;
        isJumping = true;
        SoundManager.instance.PlaySound(jumpSound);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Invoke(nameof(JumpingCooldown), 0.1f);
    }

    public void JumpingCooldown()
    {
        isJumping = false;
    }

    public void CheckIfGrounded()
    {
        RaycastHit2D ground = Physics2D.Raycast(transform.position - new Vector3(.3475f, .55f), Vector2.right, 0.695f);

        if (rb.linearVelocity.y < 0) return;
        if(ground.collider != null)
        {
            if (isGrounded) return;
            isGrounded = true;
            SoundManager.instance.PlaySound(fallSound);
        } else isGrounded = false;
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
