using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* This Class can work with and without the TutorialSign class
* 
* This Class is the PopUp canvas that will show to display tutorials
*/

public class TutorialPopUp : MonoBehaviour
{
    [Header("Tutorial")]
    ///Reference to the image gameobject in the scene
    [SerializeField] private Image tutorialImg = null;

    /**
    * Leave This Empty if it is to be used with TutorialSign.
    * 
    * TutorialSign will set it upon creating this object in the Init function
    * 
    * If is used alone, it stores all the sprite of the tutorial to show.
    * 
    * index 0 is page 1
    */
    [SerializeField] private Sprite[] pages = null;

    ///Reference to the gameobject in the scene
    [SerializeField] private Image thumbImage = null;

    [Header("Buttons")]
    [SerializeField] private GameObject backBtn = null;
    [SerializeField] private GameObject nextBtn = null;
    [SerializeField] private GameObject closeBtn = null;

    private int currentIndex = 0;
    private TutorialSign sign = null;
    private Dictionary<int, bool> enableThumb = new Dictionary<int, bool>();

    public float speed = 5f;

    private void Start()
    {
        for (int i = 0; i < pages.Length; ++i)
        {
            if (!enableThumb.ContainsKey(i))
                enableThumb.Add(i, false);
        }

        currentIndex = 0;
        UpdatePages();
    }

    /**
     * This Function is called by TutorialSign to initalise the page sprites 
     * and set whether the thumb animation should play
     */
    public void Init(Sprite[] images, TutorialSign tutorialSign, int[] pageWThumbs)
    {
        pages = images;
        sign = tutorialSign;

        foreach(int page in pageWThumbs)
        {
            if (!enableThumb.ContainsKey(page))
                enableThumb.Add(page, false);

            enableThumb[page] = true;
        }
    }

    /**
     * This Function is called each time the player changes a page
     */
    private void UpdatePages()
    {
        //Reset thumb pos
        thumbImage.rectTransform.anchoredPosition = Vector2.zero;
        thumbImage.gameObject.SetActive(enableThumb[currentIndex]);
        if (enableThumb[currentIndex])
            thumbImage.GetComponent<Animator>().SetBool("Play", true);
        else
            thumbImage.GetComponent<Animator>().SetBool("Play", false);

        tutorialImg.sprite = pages[currentIndex];

        backBtn.SetActive(false);
        nextBtn.SetActive(false);
        closeBtn.SetActive(false);

        if (currentIndex > 0)//Not on first page
        {
            backBtn.SetActive(true);
        }

        if(currentIndex == pages.Length - 1)//At Last Page
        {
            closeBtn.SetActive(true);
        }
        else//Not Last Page
        {
            nextBtn.SetActive(true);
        }
    }

    public void NextPage()
    {
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, pages.Length - 1);

        UpdatePages();
    }

    public void PreviousPage()
    {
        currentIndex = Mathf.Clamp(currentIndex - 1, 0, pages.Length - 1);

        UpdatePages();
    }

    public void ClosePopUp()
    {
        sign.PopUpClosed();
        Destroy(gameObject);
    }
}
