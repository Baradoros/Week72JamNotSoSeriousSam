using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

    public PlayerController player;
    public PlayerController.WeaponSelected buttonFor;

    private bool isButtonSelected;
	// Update is called once per frame
	void Update () {
        if (player.weaponSelected == buttonFor && !isButtonSelected)
        {
            Button button = gameObject.GetComponentInChildren<Button>();
            if (button)
            {
                isButtonSelected = true;
                button.interactable = false;
            } else
            {
                Debug.Log("Not connected to Button");
            }
        }
        else if(player.weaponSelected != buttonFor && isButtonSelected)
        {
            Button button = gameObject.GetComponentInChildren<Button>();
            if (button)
            {
                isButtonSelected = false;
                button.interactable = true;
            }
            else
            {
                Debug.Log("Not connected to Button");
            }
        }
	}
}
