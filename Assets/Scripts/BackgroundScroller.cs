using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and regulates scrolling tiles.
/// Compensates for scale and auto adjusts to the size of the image
/// </summary>
public class BackgroundScroller : MonoBehaviour {

    public GameObject background;

    private List<GameObject> bgList = new List<GameObject>();
    private float distance;
    private bool spawn = false;

    void Start() {
        distance = background.GetComponent<SpriteRenderer>().bounds.size.x * background.transform.localScale.x;
        GameObject bg = Instantiate(background, Vector3.zero, Quaternion.identity);
        bgList.Add(bg);
        Spawn();
    }

    void Update() {

        // Check if backgrounds have scrolled too far and should be deleted
        foreach(GameObject bg in bgList) {
            if (bg.transform.position.x < -distance * 1.5) {
                bgList.Remove(bg);
                Destroy(bg);
                break;
            }
        }

        // Check if new background needs to be spawned
        foreach(GameObject bg in bgList) {
            if (bg.transform.position.x > 0) {
                spawn = false;
                break;
            } else {
                spawn = true;
            }
        }

        // If new background needs to be spawned, spawn one
        if (spawn) {
            Spawn();
        }
    }

    private void Spawn() {

        // Spawn a background and add it to the list
        GameObject bg = Instantiate(background, new Vector3(distance, 0, 0), Quaternion.identity);
        bgList.Add(bg);
    }
}
