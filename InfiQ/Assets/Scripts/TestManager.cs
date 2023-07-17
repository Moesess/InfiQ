using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    private Question currentQuestion;
    private List<Answer> currentAnswers;
    private QuizManager instance;

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


    void Start()
    {
        // Load the first question
        StartTest();
    }

    void LoadQuestion()
    {

    }

    public void AnswerButtonClicked(int buttonIndex)
    {
        // Store the answer
        StoreAnswer(currentQuestion.id, currentAnswers[buttonIndex].id);
    }

    public void NextButtonClicked()
    {
        // Load the next question
        LoadQuestion();
    }

    public void StartTest()
    {
        StartCoroutine(APIManager.instance.GetRequest(APIManager.QUESTION_URL, result =>
        {
            Debug.Log(result);
        }));
    }


    void StoreAnswer(int questionId, int answerId)
    {
        // TODO: Store the answer in a suitable format
    }
}
