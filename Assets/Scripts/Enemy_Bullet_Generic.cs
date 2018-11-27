using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet_Generic : MonoBehaviour {
    public float speed = 1;

    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject bulletHitPrefab;

    bool hasSpawned = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2d.velocity = transform.right * speed;

        if (!hasSpawned)
        {
            hasSpawned = true;
            Destroy(this.gameObject, 3.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (bulletHitPrefab && (collision.gameObject.layer == 8))
        {
            GameObject bulletHit = Instantiate(bulletHitPrefab, transform.position, Quaternion.identity);
            Destroy(bulletHit, 0.583f);
        }
        Destroy(gameObject);
    }
}
