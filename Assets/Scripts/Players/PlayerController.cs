using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public float speed = 5f;
    public float bulletSpeed = 10.0f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(x * speed, y * speed);

        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(bulletSpawn.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        IgnoreCollisionOtherPlayers();
    }

    [Command]
    public void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 3.0f);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected");
        IgnoreCollisionOtherPlayers();
    }

    private void IgnoreCollisionOtherPlayers()
    {
        Collider2D[] otherPlayerColliders = GameObject.FindGameObjectWithTag("Player").GetComponents<Collider2D>();

        foreach (Collider2D col in otherPlayerColliders)
        {
            Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());
        }
    }
}
