using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySlider : MonoBehaviour {

    [Header("Story Images")]
    [Tooltip("Images in order of the story")]
    public Sprite[] StoryImages;
    [Tooltip("buttons next and play game")]
    public GameObject[] Buttons;
    public float spriteTransitionTime;

    private Image panelImage;
    private int imageShowing;

    // Use this for initialization
    void Start () {
        Buttons[0].SetActive(false);
        Buttons[1].SetActive(false);

        imageShowing = 0;
        panelImage = GetComponent<Image>();
        if (panelImage)
        {
            GameManager.manager.blackImage.enabled = true;
            GameManager.manager.FadeBlack(0);
            StartCoroutine(DelayedBlackImageDisable());
            Buttons[0].SetActive(true);
            Buttons[1].SetActive(false);
            panelImage.sprite = StoryImages[imageShowing];
            imageShowing++;
        }
        else
        {
            Debug.LogError("No Image in this panel");
        }
	}

    public void ShowNextStoryImage()
    {
        if (imageShowing < StoryImages.Length - 1)
        {
            Buttons[0].SetActive(true);
            Buttons[1].SetActive(false);
            panelImage.sprite = StoryImages[imageShowing];
            imageShowing++;
        }
        else
        {
            Buttons[1].SetActive(true);
            Buttons[0].SetActive(false);
            panelImage.sprite = StoryImages[imageShowing];
        }
    }

    public void StartGame()
    {
        GameManager.manager.blackImage.enabled = true;
        GameManager.manager.FadeBlack(1);
        GameManager.manager.LoadScene("Main", 1.5f);
    }

    public IEnumerator DelayedBlackImageDisable()
    {
        yield return new WaitForSeconds(1f);
        GameManager.manager.blackImage.enabled = false;
    }
}
