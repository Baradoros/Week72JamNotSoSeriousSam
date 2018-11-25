using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public int level;

    [Tooltip("Place enemy prefabs to spawn here")]
    public GameObject[] enemies;

    private float difficulty;                                           // The total ammount of enemies allowed on screen at once
    private List<GameObject> enemyList = new List<GameObject>();

	void Start () {
        level = GameManager.manager.level;
        difficulty = GameManager.manager.LevelToDifficultyCurve(level);
	}

	void Update () {

        // Wait for the player to kill half of the enemies before spawning another wave of them
		if (enemyList.Count < difficulty/ 2.0f) {
            SpawnWave();
        }
	}

    // Spawn a wave of enemies = half the difficulty
    private void SpawnWave() {

    }
}
