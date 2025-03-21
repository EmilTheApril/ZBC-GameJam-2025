using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public float KnockbackForce;

    public GameObject bullet;
    public Transform bulletTransform;
    private Rigidbody2D PlayerRb;

    public bool Canfire;
    private float timer;
    public float timeBetweenShots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerRb = transform.parent.GetComponent<Rigidbody2D>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        if (!Canfire)
        {
           timer += Time.deltaTime;
           if (timer >= timeBetweenShots)
           {
               Canfire = true;
               timer = 0;
           }
        }
        if(Input.GetMouseButtonDown(0) && Canfire)
        {
            Canfire = false;
            for (int i = 0; i <= 5; i++)
            {
                GameObject bulletsOBJ = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
                bulletsOBJ.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + Random.Range(-5, 5));
            }
            PlayerRb.AddForce(-rotation * KnockbackForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}
