using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private Transform myTransform;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float shootCooldown = .15f;

    private new Camera camera;
    private bool canShoot = true;
    private WaitForSeconds shootWait;

    private void Awake()
    {
        camera = Camera.main;
        shootWait = new WaitForSeconds(shootCooldown);
    }

    private void Update()
    {
        Move();
        Rotate();
        Shoot();
    }

    private void Shoot()
    {
        if (!canShoot || !Input.GetKey(KeyCode.Mouse0))
            return;
        
        GameObject bulletGO = Pooler.Instance.Pop("Bullet", myTransform.position, myTransform.rotation * Quaternion.Euler(0, 0, -90));
        bulletGO.GetComponent<Bullet>().Shoot();
        canShoot = false;

        StartCoroutine(ShootResetCoroutine());
    }

    private IEnumerator ShootResetCoroutine()
    {
        yield return shootWait;
        canShoot = true;
    }

    private void Move()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.Z))
        {
            dir.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x += 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            dir.x -= 1;
        }

        rigidbody2D.AddForce(moveSpeed * 100 * Time.deltaTime * dir.normalized);
        
    }

    private void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
 
        Vector3 objectPos = camera.WorldToScreenPoint (transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("yess");
    }
}
