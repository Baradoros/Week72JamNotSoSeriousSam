using UnityEngine;

public class RocketMovement : MonoBehaviour {

    [HideInInspector]
    public Vector3 Target;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float speedPerSecond;

    /// <summary>
    /// Proximity to target which triggers the explosion.
    /// </summary>
    [SerializeField]
    private float threshold;

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, Target, speedPerSecond * Time.deltaTime);

        if (Vector2.Distance(transform.position, Target) < threshold) {
            Explode();
        }
    }

    private void Explode() {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("PlayerBullet")) {
            Explode();
        }
    }
}
