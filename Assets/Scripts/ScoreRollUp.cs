using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRollUp : MonoBehaviour {

    private bool rollScore = false;
    private int score;
    private float initialScore = 0;

    void Start() {
        score = GameManager.manager.score;
    }

	void Update() {
        score = GameManager.manager.score;
        if (rollScore) {
            initialScore = Mathf.Lerp(initialScore, score, 5 * Time.deltaTime);
            this.GetComponent<Text>().text = Mathf.RoundToInt(initialScore).ToString().PadLeft(5, '0');

        }
	}

    // Method to be called by Score DOTween animation upon completion
    public void StartRollUp() {
        rollScore = true;
    }
}
