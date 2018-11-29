using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

    public PlayerController player;
    public PlayerController.WeaponSelected buttonFor;
    private Button button;
    private Text text;

    private bool haveEnoughCredits;
    private bool isButtonSelected;

    void Start()
    {
        button = gameObject.GetComponentInChildren<Button>();
        text = gameObject.GetComponentInChildren<Text>();
    }


	// Update is called once per frame
	void Update () {
        haveEnoughCredits = player.weaponCost[(int)buttonFor] <= GameManager.manager.score;
        text.text = player.weaponCostString[(int)buttonFor];
        if (haveEnoughCredits)
        {
            if (player.weaponSelected == buttonFor && !isButtonSelected)
            {
                if (button)
                {
                    isButtonSelected = true;
                    button.interactable = false;
                }
                else
                {
                    Debug.Log("Not connected to Button");
                }
            }
            else if (player.weaponSelected != buttonFor && isButtonSelected)
            {
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
        } else
        {
            button.interactable = false;
        }
    }
}
