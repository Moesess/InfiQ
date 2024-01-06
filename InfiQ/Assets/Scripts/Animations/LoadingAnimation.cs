using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField]
    public float tweenTime;

    [SerializeField]
    public GameObject snail;
    [SerializeField]
    public GameObject[] evenDots;
    [SerializeField]
    public GameObject[] oddDots;
    [SerializeField]
    public TMP_Text loadingText;

    int dotCount = 0;
    string baseText = "Ładowanie";

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("TweenSnail", 0.0f, tweenTime / 4);
        InvokeRepeating("AnimateDots", 0.0f, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TweenSnail()
    {
        LeanTween.cancel(snail);

        transform.localScale = Vector3.one;

        LeanTween.scaleX(snail, 1.3f, tweenTime).setEasePunch();



        if (DateTime.Now.Second % 2 == 0)
        {
            LeanTween.scale(evenDots[0], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(evenDots[1], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(evenDots[2], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(evenDots[3], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
        }
        else
        {
            LeanTween.scale(oddDots[0], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(oddDots[1], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(oddDots[2], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
            LeanTween.scale(oddDots[3], Vector3.one * 1.3f, tweenTime / 2).setEasePunch();
        }
    }

    private void AnimateDots()
    {
        LeanTween.value(gameObject, 0, 1, 0.5f).setOnComplete(() =>
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = baseText + new string('.', dotCount);
        });
    }
}
