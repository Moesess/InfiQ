using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    public const string QUESTION_URL = "http://localhost:8000/Questions/";
    public const string ANSWER_URL = "http://localhost:8000/Answers/";
    public const string TEST_TYPES = "http://localhost:8000/TestTypes/";
    public const string START_SINGLE_QUESTION_URL= "http://localhost:8000/Tests/random_question/";
    public const string VALIDATE_SINGLE_QUESTION_URL= "http://localhost:8000/Tests/random_question_answer/";
    public const string START_TEST_URL = "http://localhost:8000/Tests/random_40_question/";
    public const string VALIDATE_TEST_URL = "http://localhost:8000/Tests/test_validate/";

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
