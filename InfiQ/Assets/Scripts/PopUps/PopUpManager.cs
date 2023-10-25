using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ProfileManager;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> PopUps = new List<GameObject>();
    [SerializeField] Transform PopUpSpawner;

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
        if (!PlayerPrefs.HasKey("WelcomePopupShown"))
        {
            ShowPopUp(PopUps[1]);
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

    public void PrepFinishPopup(string result, List<Question> Questions, List<string> AnswersUIDS)
    {
        TestResultWrapper wrapper = JsonUtility.FromJson<TestResultWrapper>(result);
        TestResult testResult = wrapper.test_result;

        // Get finall popup prefab
        GameObject FinalPopUp = ShowPopUp(PopUps[2], false);
        GameObject IncorrectAnsPrefab = PopUps[2].GetComponent<TestFinishPopup>().IncorrectAnswerPrefab;

        FinalPopUp.GetComponent<TestFinishPopup>().TimeValue.GetComponent<TextMeshProUGUI>().text = Math.Round(float.Parse(testResult.duration), 2).ToString() + "s";
        FinalPopUp.GetComponent<TestFinishPopup>().ScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.score;
        FinalPopUp.GetComponent<TestFinishPopup>().FinalScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.final_score.ToString();

        GameObject content = FinalPopUp.GetComponent<TestFinishPopup>().IncorrectAnswersContent;
        int counter = 0;

        foreach (Question question in Questions)
        {
            counter++;
            string userAnswer = "";
            string correctAnswer = "";
            for (int i = 0; i < 4; i++)
            {
                if (question.answers[i].correct)
                    correctAnswer = question.answers[i].text;

                if (question.answers[i].uid == AnswersUIDS[counter - 1])
                    userAnswer = question.answers[i].text;
            }

            if (correctAnswer == userAnswer)
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
        GameObject ErrorPopup = ShowPopUp(PopUps[3], false);
        ErrorPopup.GetComponent<ErrorPopUp>().Fill(sTitle, sDesc);
        //ShowPopUp(ErrorPopup);
    }

    public GameObject CreateConfirmationPopup(string sTitle, string sLeft, string sRight, Action<bool> buttonAction, bool bAction)
    {
        GameObject ConfirmPopup = ShowPopUp(PopUps[4], true);
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

        GameObject Popup = ShowPopUp(PopUps[5], true);

        if (isQuestion)
        {
            Popup.GetComponent<MailTo>().Subject.readOnly = true;
            Popup.GetComponent<MailTo>().Subject.interactable = false;

            Popup.GetComponent<MailTo>().Subject.text = "Zg�oszenie pytania: " + TestManager.Instance.sCurrentQuestionID;
        }
    }

    public void PrepScoreboardPopUp(string result)
    {
        GameObject scoreBoard = ShowPopUp(PopUps[6], false);
        scoreBoard.GetComponent<ScoreManager>().PopulateScoreboard("INF.02");
    }

    public void PrepProfilePopUp()
    {
        ShowPopUp(PopUps[7], false);
    }
    public void ShowQuestionnairePopUp()
    {
        FirebaseManager.instance.GetUserUID(
    x => StartCoroutine(APIManager.instance.GetRequest(APIManager.USERS_URL + x,
    result =>
    {
        if (result == null)
            return;

        User response = JsonUtility.FromJson<User>(result);
        if(response.number_of_tests > 10 && PlayerPrefs.GetInt("WASQUESTIONNAIREFILLED", 0) == 0)
        {
            ShowPopUp(PopUps[8], false);
        }
    })));
    }
    public void ShowQuestionnaire()
    {
        Application.OpenURL("https://forms.gle/oZAMAKqL3yLRU8Z26");
        PlayerPrefs.SetInt("WASQUESTIONNAIREFILLED", 1);
    }
    public void ShowLoadingPopUp()
    {
        ShowPopUp(PopUps[9], false);
    }
    public void CloseLoadingPopUp()
    {
        var loadingPopup = PopUps[9].GetComponent<PopUp>();
        loadingPopup.Close();
    }
}
