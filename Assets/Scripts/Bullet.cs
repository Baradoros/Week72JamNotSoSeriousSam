using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 1;

    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject bulletHitPrefab;

    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        rb2d.velocity = -transform.right * speed;
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Enemy Bullet"))
        {
            GameObject bulletHit = Instantiate(bulletHitPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
