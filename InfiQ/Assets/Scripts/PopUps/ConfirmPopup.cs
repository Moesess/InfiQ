using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmPopup : PopUp
{
    [SerializeField] public TextMeshProUGUI TitleText;
    [SerializeField] public TextMeshProUGUI LeftButtonText;
    [SerializeField] public TextMeshProUGUI RightButtonText;

    public void Fill(string sTitle, string sLeft, string sRight)
    {
        if (TitleText != null) TitleText.text = sTitle;
        if (LeftButtonText != null) LeftButtonText.text = sRight;
        if (RightButtonText != null) RightButtonText.text = sLeft;
    }

    public void ConfirmQuit()
    {
        ExitGame.QuitGame(true);
    }
}
