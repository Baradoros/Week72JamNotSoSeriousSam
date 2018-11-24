using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour {

    public Slider slider;
    public float timeLimit = 30;

    private float nextTime = 0;

    private void Start() {
        slider.maxValue = timeLimit;
    }
    private void Update() {
        if (Time.time > nextTime) {
            nextTime = Time.time + 0.1f;
            slider.value += 0.1f;
        }

    }

}
