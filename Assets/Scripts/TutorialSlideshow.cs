using UnityEngine;
using UnityEngine.UI;

public class TutorialSlideshow : MonoBehaviour
{
    public Image tutorialImage;
    public Sprite[] tutorialSlides;

    private int currentIndex = 0;

    void Start()
    {
        UpdateSlide();
    }

    public void NextSlide()
    {
        currentIndex++;

        if (currentIndex >= tutorialSlides.Length)
            currentIndex = tutorialSlides.Length - 1;

        UpdateSlide();
    }

    public void PreviousSlide()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = 0;

        UpdateSlide();
    }

    void UpdateSlide()
    {
        tutorialImage.sprite = tutorialSlides[currentIndex];
    }
}
