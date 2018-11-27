using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// This script is meant to hold any methods that are core to the functioning of the game that may need to be called from anywhere.
/// This GameObject will be carried over scene changes, so any variables will keep their values.
/// This script is meant to be a toolbox.
/// </summary>
public class GameManager : MonoBehaviour {


    // Public reference to this script so any GameObject can call these methods
    public static GameManager manager;
    public GameObject player;
    public GameObject enemySpawner;
    public GameObject UI;
    public GameObject scoreScreenUI;
    public int score = 0;
    public int level = 1;
    public float timeLimit = 60;


    [Header("Cursor Variables")]
    public Texture2D cursorTexture = null;
    private Vector2 cursorOffset = Vector2.zero;

    [Header("Camera Variables")]
    public Camera mainCamera;
    public Vector3 scoreAreaLocation = new Vector3();
    public float lerpSpeed;

    void Start() {

        // Ensure there can only be one GameManager in a scene
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (manager != this) {
            ResetVariables();
            Destroy(gameObject);
        }

        //Check if the CursorTexture is set. If default window cursor is required, do not set the cursor Texture.
        if(cursorTexture != null)
        {
            cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2); //The offset should be half the size of the cusor image.
            Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.ForceSoftware);
        } else
        {
            Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
        }

        // Ensure Main UI is active and Scorescreen UI is inactive
        UI.SetActive(true);
        scoreScreenUI.SetActive(false);
        DOTween.RewindAll();
    }

    // [!] Anything that needs to happen when the game moves to the scorescreen happens here [!]
    // Called by the timer when time is up
    public void GoToScoreScreen() {

        // Disable Main UI and enable Score Screen UI
        UI.SetActive(false);
        scoreScreenUI.SetActive(true);

        // Move camera to scores screen position
        mainCamera.transform.DOMove(scoreAreaLocation, lerpSpeed, false);

        // Move player into position and set invulnerable
        player.GetComponent<PlayerController>().damagable = false;                                  // Set player invulnerable
        player.GetComponent<PlayerController>().canShoot = false;                                   // Disable shooting
        player.GetComponent<PlayerController>().canMove = false;                                    // Disable movement
        player.transform.DOMove(new Vector3(4, -3, player.transform.position.z), lerpSpeed, false); // Move player to predetermined point for score screen
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;                                 // Zero out velocity

        // Clear all enemies and disable enemyspawner
        enemySpawner.GetComponent<EnemySpawner>().ClearEnemies();
        enemySpawner.SetActive(false);

        // Set cursor texture back to default
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }



    #region Utility Methods
    //Reset variables to the new game manager values set
    void ResetVariables()
    {
        //Main variables
        GameManager.manager.player = player;
        GameManager.manager.enemySpawner = enemySpawner;
        GameManager.manager.timeLimit = timeLimit;

        //Cursor variables
        GameManager.manager.cursorTexture = cursorTexture;
        GameManager.manager.cursorOffset = cursorOffset;

        //Camera variables
        GameManager.manager.mainCamera = mainCamera;
        GameManager.manager.scoreAreaLocation = scoreAreaLocation;
        GameManager.manager.lerpSpeed = lerpSpeed;
    }

    // Accepts what level we're on and returns how many enemies we should have
    public float LevelToDifficultyCurve(int level) {
        // Difficulty curve y = log(2x)

        float verticalStretch = 1;             // Raises the difficulty cap
        float horizontalStretch = 2f;         // Smooths and elongates the difficulty curve
        float verticalShift = 0;                // Increases starting difficulty
        float horizontalShift = 0;              // Increases level number that curve starts at (keep at 0)

        // y = a * log(b (x - h)) + k
        // x is our difficulty so we solve:
        // x = (10 ^ ((y - k) / a)) / b + h
        // therefore:
        float difficulty = (Mathf.Pow(10, ((level - verticalShift) / verticalStretch)) / horizontalStretch) + horizontalShift;

        return difficulty;
    }

    // Used by LoadScene() to delay scene loading
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