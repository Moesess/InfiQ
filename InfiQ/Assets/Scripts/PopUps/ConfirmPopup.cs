using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : PopUp
{
    [SerializeField] public TextMeshProUGUI TitleText;
    [SerializeField] public TextMeshProUGUI LeftButtonText;
    [SerializeField] public TextMeshProUGUI RightButtonText;
    [SerializeField] public Button RightButton;

    public void Fill(string sTitle, string sLeft, string sRight, Action<bool> buttonAction, bool bAction)
    {
        if (TitleText != null) TitleText.text = sTitle;
        if (LeftButtonText != null) LeftButtonText.text = sRight;
        if (RightButtonText != null) RightButtonText.text = sLeft;
        if (RightButton != null) RightButton.onClick.AddListener(() => buttonAction(bAction));
    }
}
