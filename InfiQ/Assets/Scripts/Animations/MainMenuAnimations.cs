using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuAnimations : MonoBehaviour
{
    [SerializeField] GameObject TopMenu;
    [SerializeField] GameObject TestButton1;
    [SerializeField] GameObject TestButton2;
    [SerializeField] GameObject TestButton3;
    [SerializeField] GameObject TestButtonsText;
    [SerializeField] GameObject TestButtons;
    [SerializeField] GameObject BottomButtons;

    // Start is called before the first frame update
    void Start()
    {
        TestButton1.GetComponent<CanvasGroup>().alpha = 0;
        TestButton2.GetComponent<CanvasGroup>().alpha = 0;
        TestButton3.GetComponent<CanvasGroup>().alpha = 0;
        TopMenu.GetComponent<CanvasGroup>().alpha = 0;
        TestButtonsText.GetComponent<CanvasGroup>().alpha = 0;
        TestButtons.GetComponent<CanvasGroup>().alpha = 0;
        BottomButtons.GetComponent<CanvasGroup>().alpha = 0;
        
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        LeanTween.alphaCanvas(TopMenu.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);

        yield return null;
        LeanTween.alphaCanvas(TestButton1.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
        SlideInFromLeft(TestButton1);

        yield return new WaitForSeconds(.1f);
        LeanTween.alphaCanvas(TestButton2.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
        SlideInFromLeft(TestButton2);

        yield return new WaitForSeconds(.1f);
        LeanTween.alphaCanvas(TestButton3.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
        SlideInFromLeft(TestButton3);

        yield return new WaitForSeconds(.1f);
        LeanTween.alphaCanvas(TestButtonsText.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
        SlideInFromLeft(TestButtonsText);

        yield return new WaitForSeconds(.1f);
        LeanTween.alphaCanvas(TestButtons.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
        SlideInFromLeft(TestButtons);

        yield return new WaitForSeconds(.1f);
        LeanTween.alphaCanvas(BottomButtons.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true);
    }

    public void SlideInFromLeft(GameObject objToSlide)
    {
        // Store the original position
        Vector3 originalPosition = objToSlide.transform.position;

        // Set the starting position (e.g., off-screen to the left)
        objToSlide.transform.position = new Vector3(-Screen.width, originalPosition.y, originalPosition.z);

        // Slide the object to its original position over a duration of 0.5 seconds
        LeanTween.move(objToSlide, originalPosition, 1f).setEaseOutBounce().setIgnoreTimeScale(true);
    }

    public void SlideOutToRight(GameObject objToSlide)
    {
        // Calculate the target position (e.g., off-screen to the right)
        Vector3 targetPosition = new Vector3(Screen.width, objToSlide.transform.position.y, objToSlide.transform.position.z);

        // Slide the object to the target position over a duration of 0.5 seconds
        LeanTween.move(objToSlide, targetPosition, 1f).setEaseOutBounce().setIgnoreTimeScale(true);
    }

}
