using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private Image tutorialImg = null;
    [SerializeField] private Sprite[] pages = null;

    [Header("Buttons")]
    [SerializeField] private GameObject backBtn = null;
    [SerializeField] private GameObject nextBtn = null;
    [SerializeField] private GameObject closeBtn = null;

    private int currentIndex = 0;
    private TutorialSign sign = null;

    private void Start()
    {
        currentIndex = 0;
        tutorialImg.sprite = pages[currentIndex];

        //Only 1 page, close btn shld be active
        if(pages.Length - 1 == currentIndex)
        {
            backBtn.SetActive(false);
            nextBtn.SetActive(false);
            closeBtn.SetActive(true);
        }
        else // More than 1 page, only next button shld be active
        {
            backBtn.SetActive(false);
            nextBtn.SetActive(true);
            closeBtn.SetActive(false);
        }

    }

    public void Init(Sprite[] images, TutorialSign tutorialSign)
    {
        pages = images;
        sign = tutorialSign;
    }

    private void UpdatePages()
    {
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
