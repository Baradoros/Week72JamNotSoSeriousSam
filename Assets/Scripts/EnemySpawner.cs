using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public int level;

    [Tooltip("Place enemy prefabs to spawn here")]
    public GameObject[] enemies;            // Store enemy prefabs to spawn here and pick one from the array in SpawnWave()
                                            // Later we'll add functionality to sort by difficulty and such

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

    public GameObject GetEnemyFromArray() {

        // Add functionality to make earlier indexes more likely than later such that the numner of Miniguns > Shotguns > Rockets
        GameObject enemy = enemies[Random.Range(0, enemies.Length)];
        return enemy;
    }

    // Spawn a wave of enemies = half the difficulty
    private void SpawnWave() {

        //Spawn a wave of enemies containint half of difficulty enemies
        for (int i = 0; i < difficulty / 2.0f; i++) {
            GameObject enemy = Instantiate(GetEnemyFromArray(), new Vector3(Random.Range(-2, 2), transform.position.y, 0), Quaternion.identity);
        }
    }
}
