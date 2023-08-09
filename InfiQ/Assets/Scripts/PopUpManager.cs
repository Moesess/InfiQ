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

    public void PrepFinishPopup(string result, List<Question> Questions, List<string>AnswersUIDS)
    {
        TestResultWrapper wrapper = JsonUtility.FromJson<TestResultWrapper>(result);
        TestResult testResult = wrapper.test_result;

        // Get finall popup prefab
        PopUps[2].GetComponent<TestFinishPopup>().TimeValue.GetComponent<TextMeshProUGUI>().text = testResult.duration;
        PopUps[2].GetComponent<TestFinishPopup>().ScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.score;
        PopUps[2].GetComponent<TestFinishPopup>().FinalScoreValue.GetComponent<TextMeshProUGUI>().text = testResult.final_score.ToString();

        GameObject content = PopUps[2].GetComponent<TestFinishPopup>().IncorrectAnswersContent;

        foreach(Question question in Questions)
        {
            
        }

        ShowPopUp(PopUps[2]);
    }
}
