using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

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
    public Image blackImage;
    public int score = 0;
    public int level = 1;
    public float timeLimit = 60;


    [Header("Cursor Variables")]
    public Texture2D cursorTexture = null;
    private Vector2 cursorOffset = Vector2.zero;

    [Header("Camera Variables")]
    public Camera mainCamera;
    public Vector3 mainAreaLocation = new Vector3(0f, 0f, -10f);
    public Vector3 scoreAreaLocation = new Vector3();
    public float lerpSpeed;

    void Start() {

        // Ensure there can only be one GameManager in a scene
        if (manager == null) {
            manager = this;
        }
        else if (manager != this) {
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

        //Reset/Initiate the UISlider to 0
        UISlider slider = UI.GetComponentInChildren<UISlider>();
        if (slider)
        {
            slider.InitSlider();
        } else
        {
            Debug.Log("No UISlider found");
        }
    }

    // Called by playercontroller when health == 0
    public void Lose() {

        player.GetComponent<PlayerController>().damagable = false;                                  // Set player invulnerable
        player.GetComponent<PlayerController>().canShoot = false;                                   // Disable shooting
        player.GetComponent<PlayerController>().canMove = false;                                    // Disable movement

        // Clear all enemies and disable enemyspawner
        enemySpawner.GetComponent<EnemySpawner>().ClearEnemies();
        enemySpawner.SetActive(false);

        UI.GetComponent<UISlider>().enabled = false;
        FadeBlack(1);
        LoadScene("MainMenu", 1);
    }

    public void FadeBlack(int value) {
        if (value >= 0 && value <= 1) {
            blackImage.DOFade(value, 0.5f);
        } else {
            Debug.Log("FadeBlack(): Must use value of 0-1");
        }
    }

    // [!] Anything that needs to happen when the game moves to the scorescreen happens here [!]
    // Called by the timer when time is up
    public void GoToScoreScreen() {

        // Disable Main UI and enable Score Screen UI
        UI.SetActive(false);
        scoreScreenUI.SetActive(true);

        // Rewind and play DOTween animations so shop animates on return trips
        DOTween.RewindAll(true);
        DOTween.PlayAll();

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

    public void GoToPlayScreen() {
        // This method will need to revert everything done in GoToScoreScreen() as well as "soft reset" the game
        // We need this method to set everything up to effectively start another level, but without a scene change
        // Enable Main UI and disable Score Screen UI
        UI.SetActive(true);
        scoreScreenUI.SetActive(false);
        player.GetComponent<PlayerController>().health = 5;
        player.GetComponent<PlayerController>().ResetHealthImages();

        // Reset/Initiate the UISlider to 0
        
        UISlider slider = UI.GetComponentInChildren<UISlider>();
        slider.enabled = true;
        if (slider)
        {
            slider.InitSlider();
        }
        else
        {
            Debug.Log("No UISlider found");
        }

        //Increment the level to next act
        level++;

        // quick snap camera to main screen position to give the restart effect
        mainCamera.transform.DOMove(mainAreaLocation, lerpSpeed, false);

        //Remove invulnerable and set player position to start position
        player.GetComponent<PlayerController>().damagable = true;                                  // Set player invulnerable
        player.GetComponent<PlayerController>().canShoot = true;                                   // Disable shooting
        player.GetComponent<PlayerController>().canMove = true;                                    // Disable movement
        player.transform.DOMove(new Vector3(6.1f, -1.4f, player.transform.position.z), lerpSpeed, false); // Move player to predetermined point for score screen
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;                                 // Zero out velocity

        //enable enemyspawner
        enemySpawner.SetActive(true);

        // Set cursor texture back to default
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.ForceSoftware);
    }

    #region Utility Methods
    
    // Accepts what level we're on and returns how many enemies we should have
    public float LevelToDifficultyCurve(int level) {
        // Difficulty curve y = 2^0.25x - 1
        // x = (log2(y + 1) / 0.25)

        float difficulty = Mathf.Log((level + 1), 2) / 0.25f;
        
        return difficulty;
    }

    // Used by LoadScene() to delay scene loading
    private IEnumerator LoadSceneDelayed(string name, float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    // Streamline scene loading
    public void LoadScene(string name, float delay) {
        if (delay > 0) {
            StartCoroutine(LoadSceneDelayed(name, delay));
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