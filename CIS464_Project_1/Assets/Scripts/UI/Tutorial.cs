using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Transform currentSlide;
    private int currentSlideIndex;
  
    void Start()
    {
        currentSlideIndex = 0;
        currentSlide = this.gameObject.transform.GetChild(currentSlideIndex);
    }

    public void NextSlide()
    {
        currentSlideIndex += 1;
        SelectSlide(currentSlideIndex);
        AudioManager.Instance.PlaySound("Click");
    }

    public void PreviousSlide()
    {
        currentSlideIndex -= 1;
        SelectSlide(currentSlideIndex);
        AudioManager.Instance.PlaySound("Click");
    }

    void SelectSlide(int _index)
    {
        Transform newSlide = this.gameObject.transform.GetChild(_index);
        newSlide.gameObject.SetActive(true);
        currentSlide.gameObject.SetActive(false);
        currentSlide = newSlide;
    }
}
