using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(MeshFilter), typeof(MeshRenderer))]
public class Explosion : MonoBehaviour {

    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private int maxVertices = 48;

    [SerializeField]
    private float duration = 1f;

    private float elapsed;

    private void Start() {
        List<Vector3> vertices = new List<Vector3>();
        float radius = GetComponent<CircleCollider2D>().radius;

        for (int i = 0; i < maxVertices; i++) {
            float x = radius * Mathf.Sin((2 * Mathf.PI * i) / maxVertices);
            float y = radius * Mathf.Cos((2 * Mathf.PI * i) / maxVertices);

            vertices.Add(new Vector3(x, y, 0f));
        }
        //triangles
        List<int> triangles = new List<int>();
        for (int i = 0; i < maxVertices - 2; i++) {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Update() {
        GetComponent<MeshRenderer>().material.color = gradient.Evaluate(elapsed / duration);

        elapsed += Time.deltaTime;
        if (elapsed >= duration) {
            Destroy(gameObject);
        }
    }
}
