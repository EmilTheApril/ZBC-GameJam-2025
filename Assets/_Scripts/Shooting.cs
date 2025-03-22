using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector2 shootDir;
    private float shootRot;
    public float KnockbackForce;
    public float velocityBreakForce;
    public float maxVelocity;
    public float slowdownSpeed;

    public GameObject bullet;
    public Transform bulletTransform;
    public SpriteRenderer gunSpriteRenderer;
    private Rigidbody2D rb;

    public bool canFire = true;
    private bool shootButtonHeld = false;
    public float timeBetweenShots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        Shoot();
        DampenSpeed();
    }

    public void SetRotation()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootDir = mousePos - transform.position;
        shootRot = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, shootRot);

        gunSpriteRenderer.flipY = transform.GetChild(0).rotation.z <= 90 && transform.GetChild(0).rotation.z >= -90 ? false : true;
    }

    public void OnFire(InputValue value)
    {
        shootButtonHeld = value.Get<float>() == 1 ? true : false;
    }

    public void ShootCooldown()
    {
        if (canFire) return;
        canFire = true;
    }

    public void DampenSpeed()
    {
        if (Mathf.Abs(rb.linearVelocity.magnitude) <= maxVelocity && rb.linearVelocityY <= 0 && Mathf.Abs(rb.linearVelocityX) < 3.5f)
        {
            rb.linearDamping = 1f;
            return;
        }
        rb.linearDamping = Mathf.Lerp(Mathf.Abs(rb.linearVelocity.magnitude) * velocityBreakForce, maxVelocity, slowdownSpeed);
    }

    public void Shoot()
    {
        if (!canFire || !shootButtonHeld) return;
        canFire = false;
        for (int i = 0; i <= 5; i++)
        {
            GameObject bulletsOBJ = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            bulletsOBJ.transform.rotation = Quaternion.Euler(0f, 0f, shootRot + Random.Range(-5, 5));
        }
        rb.linearVelocityY = 0;
        rb.AddForce(-shootDir.normalized * KnockbackForce, ForceMode2D.Impulse);
        Invoke(nameof(ShootCooldown), timeBetweenShots);
    }
}
