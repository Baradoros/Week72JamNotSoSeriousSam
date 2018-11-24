using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 1;

    private Rigidbody2D rb2d;

	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        rb2d.velocity = -transform.right * speed;
	}

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("bullet");
        Destroy(gameObject);
    }
}
