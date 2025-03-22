using System.Collections;
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
    public Vector2 bulletSpreadAngle;
    public int maxBullets;
    private int currentBullets;

    public GameObject bullet;
    public Transform bulletTransform;
    public SpriteRenderer gunSpriteRenderer;
    public AudioClip shootSound;
    private Rigidbody2D rb;

    public bool canFire = true;
    private bool shootButtonHeld = false;
    public float timeBetweenShots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        currentBullets = maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        Shoot();
        DampenSpeed();

        if (GetComponent<PlayerRotate>().isGrounded && canFire) currentBullets = maxBullets;
    }

    public void SetRotation()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootDir = mousePos - transform.position;
        shootRot = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, shootRot);

        float zRot = transform.GetChild(0).eulerAngles.z;
        gunSpriteRenderer.flipY = zRot > 90 && zRot < 270 ? true : false;
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
        if (currentBullets <= 0) return;
        canFire = false;

        SoundManager.instance.PlaySound(shootSound);
        GunRecoil();
        GetComponent<PlayerRotate>().Rotate();

        for (int i = 0; i <= 5; i++)
        {
            GameObject bulletsOBJ = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            bulletsOBJ.transform.rotation = Quaternion.Euler(0f, 0f, shootRot + Random.Range(bulletSpreadAngle.x, bulletSpreadAngle.y));
        }
        //rb.linearVelocityY = 0;
        rb.AddForce(-shootDir.normalized * KnockbackForce, ForceMode2D.Impulse);
        currentBullets--;
        Invoke(nameof(ShootCooldown), timeBetweenShots);
    }

    public void GunRecoil()
    {
        StartCoroutine(RecoilCoroutine());
    }

    private IEnumerator RecoilCoroutine()
    {
        Vector3 originalPosition = gunSpriteRenderer.transform.localPosition;
        // Use shootDir for recoil direction (opposite to shooting direction)
        Vector3 recoilDirection = -new Vector3(shootDir.x, shootDir.y, 0).normalized;

        // Transform the recoil direction to local space of the gun
        recoilDirection = gunSpriteRenderer.transform.InverseTransformDirection(recoilDirection);

        float recoilAmount = 1.5f;
        float recoilSpeed = 15f;
        float returnSpeed = 2.5f;

        // Quick recoil backward
        float elapsedTime = 0f;
        float recoilDuration = 0.05f;

        while (elapsedTime < recoilDuration)
        {
            // Apply recoil
            Vector3 targetPosition = originalPosition + recoilDirection * recoilAmount;
            float progress = elapsedTime / recoilDuration * recoilSpeed;
            progress = Mathf.Clamp01(progress);

            gunSpriteRenderer.transform.localPosition = Vector3.Lerp(
                originalPosition,
                targetPosition,
                progress
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Slow return to original position
        elapsedTime = 0f;
        float returnDuration = 0.3f;
        Vector3 recoiledPosition = gunSpriteRenderer.transform.localPosition;

        while (elapsedTime < returnDuration)
        {
            float progress = elapsedTime / returnDuration * returnSpeed;
            progress = Mathf.Clamp01(progress);

            gunSpriteRenderer.transform.localPosition = Vector3.Lerp(
                recoiledPosition,
                originalPosition,
                progress
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it returns to exact original position
        gunSpriteRenderer.transform.localPosition = originalPosition;
    }
}
