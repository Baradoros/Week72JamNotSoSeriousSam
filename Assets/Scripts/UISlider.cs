using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour {

    public Slider slider;
    [HideInInspector]
    public float timeLimit;

    private float nextTime = 0;
    private bool scoreScreen;

    private void Start() {
        timeLimit = GameManager.manager.timeLimit;
        slider.maxValue = timeLimit;
        slider.value = 0;
        scoreScreen = true;
    }


    private void Update() {
        if (Time.time > nextTime) {
            nextTime = Time.time + 0.1f;
            slider.value += 0.1f;
        }

        if (slider.value == slider.maxValue && scoreScreen) {
            GameManager.manager.GoToScoreScreen();
            scoreScreen = false;
        }
    }

}
