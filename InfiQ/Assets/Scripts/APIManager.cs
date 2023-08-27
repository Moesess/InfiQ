using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    // Add a property to store the authentication token
    public string AuthToken { get; set; }

    //public const string URL = "https://4648-109-173-228-222.ngrok-free.app";
    public const string URL = "http://145.239.23.39:8000";
    public const string EMAIL_URL = URL + "/send-email/";
    public const string QUESTION_URL = URL + "/Questions/";
    public const string LOGIN_URL = URL + "/login/";
    public const string ANSWER_URL = URL + "/Answers/";
    public const string TEST_TYPES_URL = URL + "/TestTypes/";
    public const string START_SINGLE_QUESTION_URL = URL + "/Tests/random_question/";
    public const string VALIDATE_SINGLE_QUESTION_URL = URL + "/Tests/random_question_answer/";
    public const string START_TEST_URL = URL + "/Tests/random_40_question/";
    public const string VALIDATE_TEST_URL = URL + "/Tests/test_validate/";
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


    public IEnumerator GetRequest(string url, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            if (!string.IsNullOrEmpty(AuthToken))
            {
                www.SetRequestHeader("Authorization", "Bearer " + AuthToken);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                callback(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }

    public IEnumerator PostRequest(string url, string json, System.Action<string> callback)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(AuthToken)) 
        {
            request.SetRequestHeader("Authorization", "Bearer " + AuthToken);
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            callback(request.downloadHandler.text);
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    public IEnumerator PostRequestWithRetry(string url, string json, System.Action<string> callback, int maxRetries = 3)
    {
        int attempts = 0;

        while (attempts < maxRetries)
        {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(APIManager.instance.AuthToken))
            {
                request.SetRequestHeader("Authorization", "Bearer " + APIManager.instance.AuthToken);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback(request.downloadHandler.text);
                yield break;
            }
            else if (request.responseCode != 403 && attempts < maxRetries - 1)
            {
                attempts++;
                yield return new WaitForSeconds(1);
            }
            else
            {
                Debug.Log(request.error);
                yield break;
            }
        }
    }
}

