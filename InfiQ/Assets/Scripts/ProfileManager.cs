using Firebase.Auth;
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
        PopUpManager.instance.ShowLoadingPopUp();
        ProfileButtonClick();
    }

    public void PopulateUserProfile()
    {
        StartCoroutine(GetUserProfile());
    }

    private IEnumerator GetUserProfile()
    {
        if (FirebaseManager.IsUserLoggedIn())
        {
            yield return FirebaseManager.instance.GetUserUID(uid =>
            {
                StartCoroutine(FetchUserDetails(uid));
            });
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(GetUserProfile());
        }
    }

    private IEnumerator FetchUserDetails(string uid)
    {
        string result = null;
        yield return APIManager.instance.GetRequest(APIManager.USERS_URL + uid, res => { result = res; });

        if (result == null)
            yield break;

        User response = JsonUtility.FromJson<User>(result);
        nickName.text = response.name;
        completedTests.text = response.number_of_tests.ToString();
        allAnswers.text = response.all_answers.ToString();
        correctAnswers.text = response.correct_answers.ToString();
        accuracy.text = string.Format("{0:0.00}", response.accuracy * 100) + "%";

        PopUpManager.instance.CloseLoadingPopUp();
    }

    public void ProfileButtonClick()
    {
        if (!FirebaseManager.IsUserLoggedIn())
            FirebaseManager.instance.SignInWithGoogle( () => { PopulateUserProfile(); });
        else
            PopulateUserProfile();
    }
}
