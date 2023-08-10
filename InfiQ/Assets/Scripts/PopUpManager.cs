using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public GameObject ShowPopUp(GameObject PopUp, bool ret)
    {
        return Instantiate(PopUp, PopUpSpawner.position, Quaternion.identity, PopUpSpawner);
    }

    public void PrepFinishPopup(string result, List<Question> Questions, List<string>AnswersUIDS)
    {
        TestResultWrapper wrapper = JsonUtility.FromJson<TestResultWrapper>(result);
        TestResult testResult = wrapper.test_result;

        // Get finall popup prefab
        GameObject FinalPopUp = ShowPopUp(PopUps[2], false);
        GameObject IncorrectAnsPrefab = PopUps[2].GetComponent<TestFinishPopup>().IncorrectAnswerPrefab;

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

            Debug.Log(correctAnswer);
            Debug.Log(userAnswer);

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
        GameObject ErrorPopup = ShowPopUp(PopUps[3], false);
        ErrorPopup.GetComponent<ErrorPopUp>().Fill(sTitle, sDesc);
        //ShowPopUp(ErrorPopup);
    }
}
