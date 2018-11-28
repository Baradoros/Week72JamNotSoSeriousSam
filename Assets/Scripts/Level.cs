using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Act " + GameManager.manager.level.ToString();
    }
}
