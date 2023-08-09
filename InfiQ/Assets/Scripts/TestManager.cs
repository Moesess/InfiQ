using System.Collections.Generic;
using UnityEngine;

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


public class TestManager : MonoBehaviour
{
    private Question CurrentQuestion;
    private List<Answer> CurrentAnswers;
    private TestManager Instance;

    private string sInf2_uid;
    private string sInf3_uid;
    private string sInf4_uid;

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

                Debug.Log("INF.02 uid: " + sInf2_uid);
                Debug.Log("INF.03 uid: " + sInf3_uid);
                Debug.Log("INF.04 uid: " + sInf4_uid);
            }
        ));
    }


    void LoadQuestion()
    {

    }

    public void AnswerButtonClicked(int buttonIndex)
    {
        // Store the answer
        StoreAnswer(CurrentQuestion.id, CurrentAnswers[buttonIndex].id);
    }

    public void NextButtonClicked()
    {
        // Load the next question
        LoadQuestion();
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

        string json = "{ \"testType\": \"" + sTestType_uid + "\" }";
       
        StartCoroutine(APIManager.instance.PostRequestWithRetry(APIManager.START_TEST_URL, json, result =>
        {
            Debug.Log(result);
        }));
    }


    void StoreAnswer(int questionId, int answerId)
    {
        // TODO: Store the answer in a suitable format
    }
}
