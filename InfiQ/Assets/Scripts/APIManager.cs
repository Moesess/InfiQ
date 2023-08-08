using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;

    // Add a property to store the authentication token
    public string AuthToken { get; set; }

    public const string QUESTION_URL = "https://4648-109-173-228-222.ngrok-free.app/Questions/";
    public const string LOGIN_URL = "https://4648-109-173-228-222.ngrok-free.app/login/";
    public const string ANSWER_URL = "https://4648-109-173-228-222.ngrok-free.app/Answers/";
    public const string TEST_TYPES_URL = "https://4648-109-173-228-222.ngrok-free.app/TestTypes/";
    public const string START_SINGLE_QUESTION_URL = "https://4648-109-173-228-222.ngrok-free.app/Tests/random_question/";
    public const string VALIDATE_SINGLE_QUESTION_URL = "https://4648-109-173-228-222.ngrok-free.app/Tests/random_question_answer/";
    public const string START_TEST_URL = "https://4648-109-173-228-222.ngrok-free.app/Tests/random_40_question/";
    public const string VALIDATE_TEST_URL = "https://4648-109-173-228-222.ngrok-free.app/Tests/test_validate/";
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

    public IEnumerator Login(string username, string password, System.Action<string> callback)
    {
        string json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        var request = new UnityWebRequest(APIManager.LOGIN_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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

}

