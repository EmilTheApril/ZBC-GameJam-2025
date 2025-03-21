
using UnityEngine;
public class Bullets : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public Vector2 force;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = transform.right;
        rb.linearVelocity =  direction * Random.Range(force.x, force.y);
        Destroy(gameObject, Random.Range(0.1f, 0.08f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}