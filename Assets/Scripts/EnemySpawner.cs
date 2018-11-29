using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public int level;

    [Tooltip("Place enemy prefabs to spawn here. Last must be the boss.")]
    public GameObject[] enemies;            // Store enemy prefabs to spawn here and pick one from the array in SpawnWave()
    [Tooltip("Spawn weights for the enemies in enemies list. Boss weight doesn't matter")]
    public float[] spawnWeights;

    private float[] adjustedWeights;
    private float difficulty;                                           // The total ammount of enemies allowed on screen at once
    private List<GameObject> enemyList = new List<GameObject>();

	void Start () {
        level = GameManager.manager.level;
        difficulty = GameManager.manager.LevelToDifficultyCurve(level);

        if(enemies.Length != spawnWeights.Length)
        {
            Debug.LogError("Enemies and spawnWeights are not matching");
        }

        //Normalizing the spawnWeights
        float totalWeights = 0;
        foreach (float weights in spawnWeights) totalWeights += weights;

        adjustedWeights = new float[spawnWeights.Length];
        for(int index = 0; index < spawnWeights.Length; index++)
        {
            adjustedWeights[index] = (spawnWeights[index] / totalWeights) * 100;
            if (index > 0)
            {
                adjustedWeights[index] += adjustedWeights[index-1];
            }
        }

        // Spawn Initial enemies up to difficulty cap
        SpawnWave();
        SpawnWave();
	}

	void Update () {

        level = GameManager.manager.level;
        difficulty = GameManager.manager.LevelToDifficultyCurve(level);

        // Spawn maximum enemies if there are no enemies
        if (enemyList.Count == 0) {
            SpawnWave();
            SpawnWave();
        }

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
        // I know it's hardcoded don't judge me
        // 40% chance of minigun enemy
        // 35% chance of shotgun enemy
        // 25% chance of rocket enemy
        float chance = Random.Range(0, 100);
        int select = 0;
        for(;select < enemies.Length; select++)
        {
            if(chance < adjustedWeights[select])
            {
                break;
            }
        }

        GameObject enemy = enemies[select];
        return enemy;
    }

    // Spawn a wave of enemies = half the difficulty
    private void SpawnWave() {
        if (level % 5 != 0)
        {
         Debug.Log("Spawning " + difficulty / 2 + " enemies");
        Debug.Log(difficulty);
            for (int i = 0; i < difficulty / 2.0f; i++)
            {
                GameObject enemy = Instantiate(GetEnemyFromArray(), new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, 0), Quaternion.identity);
                enemyList.Add(enemy);
            }
       } else
       {
           if(enemyList.Count == 0)
           {
                for (int count = 0; count < Mathf.FloorToInt(difficulty / 10); count++)
                {
                    GameObject enemy = Instantiate(enemies[enemies.Length - 1], new Vector3(this.transform.position.x + Random.Range(-2, 2), this.transform.position.y, 0), Quaternion.identity);
                    enemyList.Add(enemy);
                }
           }
       }
    }

    public void ClearEnemies() {
        foreach (GameObject enemy in enemyList) {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }
}
