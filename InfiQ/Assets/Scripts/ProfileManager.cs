using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nickName;
    [SerializeField]
    GameObject avatar;
    [SerializeField]
    TextMeshProUGUI completedTests;
    [SerializeField]
    TextMeshProUGUI allAnswers;
    [SerializeField]
    TextMeshProUGUI correctAnswers;
    [SerializeField]
    TextMeshProUGUI accuracy;

    [System.Serializable]
    public class User
    {
        public string uid;
        public string name;
        public int number_of_tests;
        public int best_score;
        public float best_time;
        public int correct_answers;
        public int all_answers;
        public float accuracy;
    }

    public void Start()
    {
        ProfileButtonClick();
    }

    public void PopulateUserProfile()
    {
        FirebaseManager.instance.GetUserUID(
            x => StartCoroutine(APIManager.instance.GetRequest(APIManager.USERS_URL + x,
            result =>
            {
                if (result == null)
                    return;
                Debug.Log(result);
                User response = JsonUtility.FromJson<User>(result);
                nickName.text = response.name;
                completedTests.text = response.number_of_tests.ToString();
                allAnswers.text = response.all_answers.ToString();
                correctAnswers.text = response.correct_answers.ToString();
                accuracy.text = string.Format("{0:0.00}", response.accuracy) + "%";
            })));
    }

    public void ProfileButtonClick()
    {
        FirebaseManager.instance.SignInWithGoogle(PopulateUserProfile);
    }

}
