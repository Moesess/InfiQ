using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> PopUps = new List<GameObject>();
    [SerializeField] Transform PopUpSpawner;

    static GameObject POPUP;
    static GameObject WELCOMEPOPUP;
    static GameObject TESTFINISHPOPUP;
    static GameObject ERRORPOPUP;
    static GameObject CONFIRMATIONPOPUP;
    static GameObject SENDREPORTPOPUP;
    static GameObject SCOREBOARDPOPUP;
    static GameObject PROFILEPOPUP;

    public static PopUpManager instance;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        POPUP = PopUps[0];
        WELCOMEPOPUP = PopUps[1];
        TESTFINISHPOPUP = PopUps[2];
        ERRORPOPUP = PopUps[3];
        CONFIRMATIONPOPUP = PopUps[4];
        SENDREPORTPOPUP = PopUps[5];
        SCOREBOARDPOPUP = PopUps[6];
        PROFILEPOPUP = PopUps[7];

        if (!PlayerPrefs.HasKey("WelcomePopupShown"))
        {
            ShowPopUp(WELCOMEPOPUP);
            PlayerPrefs.SetInt("WelcomePopupShown", 1);
            PlayerPrefs.Save();

        }
        // ONLY FOR DEBUG, DELETE IN PROD
        //PlayerPrefs.DeleteKey("WelcomePopupShown");
    }

    public void ShowPopUp(GameObject PopUp)
    {
        Instantiate(PopUp, PopUpSpawner.position, Quaternion.identity, PopUpSpawner);
    }

    public GameObject ShowPopUp(GameObject PopUp, bool bNoPosition)
    {
        return Instantiate(PopUp, PopUpSpawner.position, Quaternion.identity, PopUpSpawner);
    }

    public void PrepFinishPopup(string result, List<Question> Questions, List<string>AnswersUIDS)
    {
        TestResultWrapper wrapper = JsonUtility.FromJson<TestResultWrapper>(result);
        TestResult testResult = wrapper.test_result;

        // Get finall popup prefab
        GameObject FinalPopUp = ShowPopUp(TESTFINISHPOPUP, false);
        GameObject IncorrectAnsPrefab = TESTFINISHPOPUP.GetComponent<TestFinishPopup>().IncorrectAnswerPrefab;

        FinalPopUp.GetComponent<TestFinishPopup>().TimeValue.GetComponent<TextMeshProUGUI>().text = testResult.duration;
        FinalPopUp.GetComponent<TestFinishPopup>().ScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.score;
        FinalPopUp.GetComponent<TestFinishPopup>().FinalScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.final_score.ToString();

        GameObject content = FinalPopUp.GetComponent<TestFinishPopup>().IncorrectAnswersContent;
        int counter = 0;

        foreach(Question question in Questions)
        {
            counter++;
            string userAnswer = "";
            string correctAnswer = "";
            for (int i = 0; i < 4; i++)
            {
                if (question.answers[i].correct)
                    correctAnswer = question.answers[i].text;

                if(question.answers[i].uid == AnswersUIDS[counter-1])
                    userAnswer = question.answers[i].text;
            }

            if(correctAnswer == userAnswer)
                continue;
            
            IncorrectAnsPrefab.GetComponent<IncorrectAnswer>().QuestionID.GetComponent<TextMeshProUGUI>().text = counter.ToString();
            IncorrectAnsPrefab.GetComponent<IncorrectAnswer>().QuestionText.GetComponent<TextMeshProUGUI>().text = question.text;
            IncorrectAnsPrefab.GetComponent<IncorrectAnswer>().UserAnswer.GetComponent<TextMeshProUGUI>().text = userAnswer;
            IncorrectAnsPrefab.GetComponent<IncorrectAnswer>().CorrectAnswer.GetComponent<TextMeshProUGUI>().text = correctAnswer;

            GameObject IncorrectAnswer = Instantiate(IncorrectAnsPrefab, Vector3.zero, Quaternion.identity);
            IncorrectAnswer.transform.SetParent(content.transform, false);

        }
        
    }

    public void CreateErrorPopup(string sTitle, string sDesc) 
    {
        GameObject ErrorPopup = ShowPopUp(ERRORPOPUP, false);
        ErrorPopup.GetComponent<ErrorPopUp>().Fill(sTitle, sDesc);
        //ShowPopUp(ErrorPopup);
    }

    public GameObject CreateConfirmationPopup(string sTitle, string sLeft, string sRight, Action<bool> buttonAction, bool bAction)
    {
        GameObject ConfirmPopup = ShowPopUp(CONFIRMATIONPOPUP, true);
        ConfirmPopup.GetComponent<ConfirmPopup>().Fill(sTitle, sLeft, sRight, buttonAction, bAction);
        ConfirmPopup.transform.SetParent(PopUpSpawner, false);

        return ConfirmPopup;
    }

    public void CreateSendReportPopup(bool isQuestion)
    {
        if (DateTime.Now.ToString("yyyy-MM-dd") != PlayerPrefs.GetString("LastEmailDate", ""))
        {
            PlayerPrefs.SetInt("EmailCount", 0);
        }

        if (!(PlayerPrefs.GetInt("EmailCount", 0) < 10))
        {
            CreateErrorPopup("B��D", "Odczekaj troch� przed wysy�aniem kolejnych zg�osze�. Pami�taj �e limit na dzie� to 10!");
            return;
        }

        if (Time.time - PlayerPrefs.GetFloat("LastEmailTime", 0f) < 30f)
        {
            CreateErrorPopup("B��D", "Odczekaj troch� przed wys�aniem kolejnego zg�oszenia.");
            return;
        }

        GameObject Popup = ShowPopUp(SENDREPORTPOPUP, true);

        if (isQuestion)
        {
            Popup.GetComponent<MailTo>().Subject.readOnly = true;
            Popup.GetComponent<MailTo>().Subject.interactable = false;
                
            Popup.GetComponent<MailTo>().Subject.text = "Zg�oszenie pytania: " + TestManager.Instance.sCurrentQuestionID;
        }
    }

    public void PrepScoreboardPopUp(string result)
    {
        GameObject scoreBoard = ShowPopUp(SCOREBOARDPOPUP, false);
        scoreBoard.GetComponent<ScoreManager>().PopulateScoreboard("INF.02");
    }

    public void PrepProfilePopUp()
    {
        ShowPopUp(PROFILEPOPUP, false);
    }
}
