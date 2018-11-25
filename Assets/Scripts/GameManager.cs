using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is meant to hold any methods that are core to the functioning of the game that may need to be called from anywhere.
/// This GameObject will be carried over scene changes, so any variables will keep their values.
/// This script is meant to be a toolbox.
/// </summary>
public class GameManager : MonoBehaviour {


    // Public referrence to this script so any GameObject can call these methods
    public static GameManager manager;
    public int score = 0;

    void Start() {

        // Ensure there can only be one GameManager in a scene
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (manager != this) {
            Destroy(gameObject);
        }
    }

    #region Utility Methods

    public float LevelToDifficultyCurve(int level) {
        // Difficulty curve y = 50log(2x)
        // y = a * log(b (x - h)) + k
        // x = (10 ^ ((y - k) / a)) / b + h


        float verticalStretch = 50;
        float horizontalStretch = 2;
        float verticalShift = 0;
        float horizontalShift = 0;

        float difficulty = (Mathf.Pow(10, ((level - verticalShift) / verticalStretch)) / horizontalStretch) + horizontalShift;

        return difficulty;

    }

    private IEnumerator LoadSceneDelayed(float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    // Streamline scene loading
    public void LoadScene(string name, float delay) {
        if (delay > 0) {
            StartCoroutine(LoadSceneDelayed(delay));
        }
        else {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }
    }

    public void Quit() {

        #if UNITY_EDITOR // If we're in Unity Editor, stop play mode
            if (UnityEditor.EditorApplication.isPlaying == true)
                UnityEditor.EditorApplication.isPlaying = false;
        #else // If we are in a built application, quit to desktop
            Application.Quit();
        #endif
    }
    #endregion
}
