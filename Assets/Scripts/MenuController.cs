using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public void StartGame()
    {
        GameManager.manager.LoadScene("Main", 0);
    }

    public void QuitGame()
    {
        GameManager.manager.Quit();
    }
}
