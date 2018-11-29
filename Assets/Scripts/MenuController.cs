using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    private void Start() {
        GameManager.manager.FadeBlack(0);
        GameManager.manager.blackImage.enabled = false;
    }

    public void StartGame()
    {
        GameManager.manager.blackImage.enabled = true;
        GameManager.manager.FadeBlack(1);
        GameManager.manager.LoadScene("StoryScreen", 1.5f);
    }

    public void QuitGame()
    {
        GameManager.manager.Quit();
    }
}
