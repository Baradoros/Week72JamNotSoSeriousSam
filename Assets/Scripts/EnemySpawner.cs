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

        // Remove destroyed objects from enemyList
        foreach(GameObject enemy in enemyList) {
            if (enemy == null) {
                enemyList.Remove(enemy);
                break;      // To avoid IndexOutOfBounds
            }
        }
	}

    public GameObject GetEnemyFromArray() {

        // Functionality to make earlier indexes more likely than later such that the numner of Miniguns > Shotguns > Rockets
        // 40% chance of minigun enemy
        // 35% chance of shotgun enemy
        // 25% chance of rocket enemy
        float chance = Random.Range(0, 100);
        int select = 0;
        if (chance < 40)
            select = 0;
        else if (chance > 40 && chance < 75)
            select = 1;
        else if (chance > 75)
            select = 2;


        GameObject enemy = enemies[select];
        return enemy;
    }

    // Spawn a wave of enemies = half the difficulty
    private void SpawnWave() {
        Debug.Log("Spawning " + difficulty / 2 + " enemies");
        //Spawn a wave of enemies containint half of difficulty enemies
        for (int i = 0; i < difficulty / 2.0f; i++) {
            GameObject enemy = Instantiate(GetEnemyFromArray(), new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, 0), Quaternion.identity);
            enemyList.Add(enemy);
        }
    }
}
