using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TestType
{
    public string uid;
    public string name;
    public string text;
    public string created_at;
};

[System.Serializable]
public class TestTypesResponse
{
    public int count;
    public string next;
    public string previous;
    public List<TestType> results;
};

[System.Serializable]
public class TestResult
{
    public string uid;
    public string test;
    public bool isDone;
    public string score;
    public string date_start;
    public string date_end;
}

[System.Serializable]
public class TestResponse
{
    public string uid;
    public string testType;
    public List<Question> questions;
    public TestResult testResult;
    public string created_at;
}

[System.Serializable]
public class Question
{
    public string uid;
    public string id;
    public string testType;
    public string text;
    public string img;
    public string created_at;
    public List<Answer> answers;
}

[System.Serializable]
public class Answer
{
    public string uid;
    public string text;
    public string created_at;
    public string question;
    public bool correct;
}

public class TestManager : MonoBehaviour
{
    private TestManager Instance;

    [SerializeField]
    private List<Question> Questions = new();

    private int iCurrentQuestion = 0;

    [SerializeField]
    private List<string> AnswersUIDS = new();

    private string sInf2_uid;
    private string sInf3_uid;
    private string sInf4_uid;

    [SerializeField] GameObject QuestionCanvas;
    [SerializeField] GameObject QuestionID;
    [SerializeField] GameObject QuestionDesc;
    [SerializeField] GameObject AnsA;
    [SerializeField] GameObject AnsB;
    [SerializeField] GameObject AnsC;
    [SerializeField] GameObject AnsD;
    [SerializeField] GameObject MainMenuCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        StartCoroutine(APIManager.instance.GetRequest(
            APIManager.TEST_TYPES_URL,
            result =>
            {
                TestTypesResponse response = JsonUtility.FromJson<TestTypesResponse>(result);

                foreach (TestType testType in response.results)
                {
                    if (testType.name == "INF.02")
                    {
                        sInf2_uid = testType.uid;
                    }
                    else if (testType.name == "INF.03")
                    {
                        sInf3_uid = testType.uid;
                    }
                    else if (testType.name == "INF.04")
                    {
                        sInf4_uid = testType.uid;
                    }
                }
            }
        ));
    }

    private void DisplayQuestion(int index)
    {

        if (index < 0 || index >= Questions.Count)
            return;

        EnableButtons();

        Question question = Questions[index];
        iCurrentQuestion = index;

        // Set the question ID and description
        QuestionID.GetComponent<TextMeshProUGUI>().text = question.id;
        QuestionDesc.GetComponent<TextMeshProUGUI>().text = question.text;

        // Assuming the answers are in the order A, B, C, D
        AnsA.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[0].text;
        AnsB.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[1].text;
        AnsC.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[2].text;
        AnsD.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[3].text;
    }

    bool GenerateQuestions(string result)
    {
        TestResponse testResponse = JsonUtility.FromJson<TestResponse>(result);

        // Clear the existing questions (if any)
        Questions.Clear();
        AnswersUIDS.Clear();
        iCurrentQuestion = 0;

        // Add the new questions from the response
        foreach (Question question in testResponse.questions)
        {
            Questions.Add(question);
        }

        return Questions.Count > 0;
    }

    void DisableButtons()
    {
        AnsA.GetComponent<Button>().enabled = false;
        AnsB.GetComponent<Button>().enabled = false;
        AnsC.GetComponent<Button>().enabled = false;
        AnsD.GetComponent<Button>().enabled = false;
    }

    void EnableButtons()
    {
        AnsA.GetComponent<Button>().enabled = true;
        AnsB.GetComponent<Button>().enabled = true;
        AnsC.GetComponent<Button>().enabled = true;
        AnsD.GetComponent<Button>().enabled = true;
        AnsA.GetComponent<Image>().color = Color.white;
        AnsB.GetComponent<Image>().color = Color.white;
        AnsC.GetComponent<Image>().color = Color.white;
        AnsD.GetComponent<Image>().color = Color.white;
    }

    IEnumerator WaitAndDisplayNextQuestion(int nextQuestionIndex)
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        DisplayQuestion(nextQuestionIndex);
    }

    public void SelectAnswer(int iAns)
    {
        DisableButtons();
        Question question = Questions[iCurrentQuestion];
        GameObject selectedAns = AnsA;

        AnswersUIDS.Add(question.answers[iAns].uid);

        switch (iAns)
        {
            case 0:
                selectedAns = AnsA;
                break;
            case 1:
                selectedAns = AnsB;
                break;
            case 2:
                selectedAns = AnsC;
                break;
            case 3:
                selectedAns = AnsD;
                break;
        }

        if (question.answers[0].correct)
            AnsA.GetComponent<Image>().color = Color.green;
        if (question.answers[1].correct)
            AnsB.GetComponent<Image>().color = Color.green;
        if (question.answers[2].correct)
            AnsC.GetComponent<Image>().color = Color.green;
        if (question.answers[3].correct)
            AnsD.GetComponent<Image>().color = Color.green;

        if (!question.answers[iAns].correct)
            selectedAns.GetComponent<Image>().color = Color.red;

        StartCoroutine(WaitAndDisplayNextQuestion(iCurrentQuestion+1));
    }

    public void StartTest(string sTestType)
    {
        string sTestType_uid;

        switch (sTestType)
        {
            case "INF.02":
                sTestType_uid = sInf2_uid;
                break;
            case "INF.03":
                sTestType_uid = sInf3_uid;
                break;
            case "INF.04":
                sTestType_uid = sInf4_uid;
                break;
            default:
                sTestType_uid = "";
                break;
        }

        if (sTestType_uid == "") 
            return;
        
        string json = "{ \"testType\": \"" + sTestType_uid + "\" }";
       
        StartCoroutine(APIManager.instance.PostRequestWithRetry(APIManager.START_TEST_URL, json, result =>
        {
            if (result == null)
                return;

            if (GenerateQuestions(result))
            {
                MainMenuCanvas.SetActive(false);
                QuestionCanvas.SetActive(true);

                DisplayQuestion(iCurrentQuestion);
            }
            
        }));
    }
}
