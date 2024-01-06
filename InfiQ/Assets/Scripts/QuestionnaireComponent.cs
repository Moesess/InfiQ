using UnityEngine;
using static System.Net.WebRequestMethods;

public class QuestionnaireComponent : MonoBehaviour
{
    public void OpenUrl()
    {
        string questionnaireUrl = "https://forms.gle/PAEnC97J365huiUB8";
        Application.OpenURL(questionnaireUrl);
        PlayerPrefs.SetInt("WASQUESTIONNAIREFILLED", 1);
    }
    public void OpenQuestionUrl()
    {
        string questionUrl = "https://docs.google.com/forms/d/e/1FAIpQLScale" +
            "SEghMTgaof9uBV11W9B4J4OkgaOPkMIaiXG1nsHJH-fg/viewform?usp=pp_url&" +
            "entry.1041730861=Ekran+pytania&entry.234038676="
            + TestManager.Instance.sCurrentQuestionID;
        Debug.Log(questionUrl);
        Application.OpenURL(questionUrl);
    }
    public void OpenErrorUrl()
    {
        string errorUrl = "https://forms.gle/pSRqAbPWMLUDmBKY9";
        Application.OpenURL(errorUrl);
    }
}
