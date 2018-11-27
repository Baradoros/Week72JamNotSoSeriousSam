using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and regulates scrolling tiles.
/// Compensates for scale and auto adjusts to the size of the image
/// </summary>
public class BackgroundScroller : MonoBehaviour {

    public GameObject background;
    public GameObject streetLight;
    public GameObject parallaxBuildings1;
    public GameObject parallaxBuildings2;
    public float streetLightSpawnDelay = 1;

    private List<GameObject> bgList = new List<GameObject>();
    private float distance;
    private bool spawn = false;
    private float nextSpawn = 0;

    void Start() {

        // Spawn Initial background on camera before scrolling begins
        distance = background.GetComponent<SpriteRenderer>().bounds.size.x * background.transform.localScale.x;
        GameObject bg = Instantiate(background, new Vector3(distance, 0, 1), Quaternion.identity);
        bg = Instantiate(background, new Vector3(0, 0, 1), Quaternion.identity);
        bgList.Add(bg);
        SpawnBackground();
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
            if (bg.transform.position.x > distance) {
                spawn = false;
                break;
            } else {
                spawn = true;
            }
        }

        // If new background needs to be spawned, spawn one
        if (spawn) {
            SpawnBackground();
        }


        // Street Lights
        if (Time.time > nextSpawn) {
            SpawnStreetLight();
            nextSpawn = Time.time + streetLightSpawnDelay;
        }
    }

    private void SpawnStreetLight() {
        Instantiate(streetLight, new Vector3(10, -3.197597f, -2), Quaternion.identity);
    }

    private void SpawnBackground() {

        // Spawn a background and add it to the list
        GameObject bg = Instantiate(background, new Vector3(distance * 2, 0, 1), Quaternion.identity);
        bgList.Add(bg);

        // Spawn Parallax
        Instantiate(parallaxBuildings1, new Vector3(distance * 2, 2.893f, 5), Quaternion.identity);
        Instantiate(parallaxBuildings2, new Vector3(distance * 2, 3.04f, 10), Quaternion.identity);
    }
}
