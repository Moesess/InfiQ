using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
public class TestResultWrapper
{
    public TestResult test_result;
}

[System.Serializable]
public class TestResult
{
    public string uid;
    public string test;
    public bool isDone;
    public string score;
    public int final_score;
    public string date_start;
    public string date_end;
    public string duration;
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

[System.Serializable]
public class TestValidateData
{
    public string test_uid;
    public Dictionary<string, string> answers;
}

public class TestManager : MonoBehaviour
{
    // TODO ONLY FOR DEBUG
    private const bool DEBUG_MODE = true;

    public static TestManager Instance;

    [SerializeField]
    private List<Question> Questions = new();

    public int iCurrentQuestion;
    public string sCurrentQuestionID;

    [SerializeField]
    private List<string> AnswersUIDS = new();

    private string sCurrentTest;

    private string sInf2_uid;
    private string sInf3_uid;
    private string sInf4_uid;

    GameObject Confirm;

    [SerializeField] GameObject INF2Btn;
    [SerializeField] GameObject INF3Btn;
    [SerializeField] GameObject INF4Btn;
    [SerializeField] GameObject QuestionCanvas;
    [SerializeField] public GameObject QuestionID;
    [SerializeField] GameObject QuestionDesc;
    [SerializeField] GameObject AnsA;
    [SerializeField] GameObject AnsB;
    [SerializeField] GameObject AnsC;
    [SerializeField] GameObject AnsD;
    [SerializeField] GameObject MainMenuCanvas;
    [SerializeField] GameObject ShowImageButton;
    [SerializeField] RawImage QuestionImage;
    [SerializeField] Color ButtonColor;
    [SerializeField] Color CorrectAnswerColor;
    [SerializeField] Color WrongAnswerColor;

    IEnumerator WaitAndDisplayNextQuestion(int nextQuestionIndex, bool correctAnswer)
    {
        if (!correctAnswer)
        {
            yield return StartCoroutine(Shake(QuestionCanvas));
            yield return new WaitForSeconds(1.2f);
        }

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(ScaleDown(QuestionCanvas));

        DisplayQuestion(nextQuestionIndex);
    }

    IEnumerator LoadImageFromURL(string imageURL, RawImage imageComponent)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load the image: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            imageComponent.texture = texture;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Confirm = null;
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
        ReturnToMenu(true);
        ShowImageButton.SetActive(false);
        QuestionImage.enabled = false;
        QuestionCanvas.transform.localScale = Vector3.zero;
        iCurrentQuestion = 0;
        sCurrentQuestionID = "";
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
        StartCoroutine(ScaleUp(QuestionCanvas));
     
        if (index < 0) 
        {
            return;
        }
        if (index >= Questions.Count)
        {
            ValidateTest();
            ReturnToMenu(true);
            return;
        }

        EnableButtons();

        Question question = Questions[index];
        iCurrentQuestion = index;
        sCurrentQuestionID = Questions[index].id;

        // Set the question ID and description
        QuestionID.GetComponent<TextMeshProUGUI>().text = (index + 1).ToString();
        QuestionDesc.GetComponent<TextMeshProUGUI>().text = question.text;

        AnsA.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[0].text;
        AnsB.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[1].text;
        AnsC.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[2].text;
        AnsD.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = question.answers[3].text;

        if (question.img != "")
        {
            ShowImageButton.SetActive(true);
            StartCoroutine(LoadImageFromURL(APIManager.URL + question.img, QuestionImage));
        }
        else 
        {
            ShowImageButton.SetActive(false);
            QuestionImage.enabled = false;
        }
    }

    public IEnumerator ScaleUp(GameObject objToSlide)
    {
        LeanTween.scale(objToSlide, Vector3.one, 0.35f).setEaseInOutQuad().setIgnoreTimeScale(true);
        yield return new WaitForSeconds(0.35f);
    }

    public IEnumerator ScaleDown(GameObject objToSlide)
    {
        Vector3 pos = new(0.95f, 0.95f, 0.95f);
        LeanTween.scale(objToSlide, pos, 0.35f).setEaseInOutQuad().setIgnoreTimeScale(true);
        yield return new WaitForSeconds(0.35f);
    }

    public IEnumerator Shake(GameObject objToShake)
    {
        Vector3 originalPosition = objToShake.transform.position;
        float shakeAmount = 40f;
        float shakeDuration = 0.1f;

        LeanTween.moveX(objToShake, originalPosition.x - shakeAmount, shakeDuration).setEaseShake();
        yield return new WaitForSeconds(shakeDuration);

        LeanTween.moveX(objToShake, originalPosition.x + shakeAmount, shakeDuration).setEaseShake();
        yield return new WaitForSeconds(shakeDuration);

        LeanTween.move(objToShake, originalPosition, shakeDuration).setEaseInOutQuad();
        yield return new WaitForSeconds(shakeDuration);
    }


    private void ValidateTest()
    {
        string json = "{ \"test_uid\": \"" + sCurrentTest + "\", \"answers\": {";

        // Add each answer to the JSON string
        for (int i = 0; i < Questions.Count; i++)
        {
            json += "\"" + Questions[i].uid + "\": \"" + AnswersUIDS[i] + "\"";
            if (i < Questions.Count - 1) // If not the last item, add a comma
            {
                json += ",";
            }
        }

        json += "} }"; 

        StartCoroutine(APIManager.instance.PostRequestWithRetry(APIManager.VALIDATE_TEST_URL, json, result =>
        {
            if (result == null)
                return;

            PopUpManager.instance.PrepFinishPopup(result, Questions, AnswersUIDS);
        }));
    }

    public void ReturnToMenu(bool bQuit)
    {
        if(bQuit)
        {
            MainMenuCanvas.SetActive(true);
            QuestionCanvas.SetActive(false);

            if(Confirm != null)
                Confirm.GetComponent<ConfirmPopup>().Close();
        }
        else
        {
            Confirm = PopUpManager.instance.CreateConfirmationPopup(
                "Czy na pewno chcesz wyjœæ? \n Test zostanie uniewa¿niony!", "Wychodzê", "Zostaje!", ReturnToMenu, true);
        }
    }

    bool GenerateQuestions(string result)
    {
        TestResponse testResponse = JsonUtility.FromJson<TestResponse>(result);
        sCurrentTest = testResponse.uid;

        // Clear the existing questions and answers (if any)
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
        AnsA.GetComponent<Image>().color = ButtonColor;
        AnsB.GetComponent<Image>().color = ButtonColor;
        AnsC.GetComponent<Image>().color = ButtonColor;
        AnsD.GetComponent<Image>().color = ButtonColor;
    }

    public void ShowImage()
    {
        QuestionImage.enabled = !QuestionImage.enabled;
    }

    public void SelectAnswer(int iAns)
    {
        DisableButtons();
        Question question = Questions[iCurrentQuestion];
        GameObject selectedAns = AnsA;

        AnswersUIDS.Add(question.answers[iAns].uid);

        bool correctAnswer = true;

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
            AnsA.GetComponent<Image>().color = CorrectAnswerColor;
        if (question.answers[1].correct)
            AnsB.GetComponent<Image>().color = CorrectAnswerColor;
        if (question.answers[2].correct)
            AnsC.GetComponent<Image>().color = CorrectAnswerColor;
        if (question.answers[3].correct)
            AnsD.GetComponent<Image>().color = CorrectAnswerColor;

        if (!question.answers[iAns].correct)
        {
            selectedAns.GetComponent<Image>().color = WrongAnswerColor;
            correctAnswer = false;
            StartCoroutine(Shake(selectedAns));
        }

        StartCoroutine(WaitAndDisplayNextQuestion(iCurrentQuestion+1, correctAnswer));
    }

    public void StartTest(string sTestType)
    {
        INF2Btn.SetActive(false);
        INF3Btn.SetActive(false);
        INF4Btn.SetActive(false);

        if (FirebaseManager.IsUserLoggedIn() || DEBUG_MODE)
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

            INF2Btn.SetActive(true);
            INF3Btn.SetActive(true);
            INF4Btn.SetActive(true);
        }
        else
        {
            PopUpManager.instance.CreateErrorPopup("ERROR", "Nie jesteœ zalogowany! Tylko zalogowani u¿ytkownicy maj¹ dostêp do testów!");
        }
    }
}
