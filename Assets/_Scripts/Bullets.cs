
using UnityEngine;
public class Bullets : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public Vector2 force;
    public Vector2 destroyAfter;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = transform.right;
        rb.linearVelocity =  direction * Random.Range(force.x, force.y) * Time.fixedDeltaTime;
        Destroy(gameObject, Random.Range(destroyAfter.x, destroyAfter.y));
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground")) Destroy(gameObject);
    }
}