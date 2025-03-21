using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField] private Vector2 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isJumping;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = Vector2.right;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        rb.linearVelocity = (dir * speed * Time.fixedDeltaTime) + new Vector2(0, rb.linearVelocity.y);
        CheckFloor();
    }

    public void Jump()
    {
        if (isJumping == true) return;
        isJumping = true;
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        Invoke(nameof(JumpCooldown), 0.25f);
    }

    public void JumpCooldown()
    {
        isJumping = false;
    }

    public void CheckFloor()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + new Vector3((dir.x / 2) + (dir.x * 0.05f), -0.5f, 0), Vector2.down, 2f);
        RaycastHit2D hitGroundInFront = Physics2D.Raycast(transform.position + new Vector3((dir.x / 2) + dir.x + (dir.x * 0.05f), -0.5f, 0), Vector2.down, 2f);
        RaycastHit2D hitUnder = Physics2D.Raycast(transform.position + new Vector3(0, -0.6f, 0), Vector2.down, 0.01f);
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position + new Vector3((dir.x / 2) + (dir.x * 0.05f), 0, 0), dir, 1f);
        RaycastHit2D hitWallOver = Physics2D.Raycast(transform.position + new Vector3((dir.x / 2) + (dir.x * 0.05f), 1, 0), dir, 1f);

        if(hitWall.distance <= 1 && hitWall.collider != null && hitWallOver.collider == null && hitUnder.collider != null || hitGroundInFront.distance <= 1.5f && hitGroundInFront.collider != null && hitUnder.collider != null && hitGround.collider == null)
        {
            if (hitUnder.collider == null) return;
            Jump();
            return;
        }

        if (hitGround.collider == null || hitGround.distance > 1.05f || hitWall.distance <= 0.05f && hitWall.collider != null)
        {
            if (hitUnder.collider == null) return;
            dir = dir * -1;
        }
    }
}
