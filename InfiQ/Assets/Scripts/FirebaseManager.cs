using UnityEngine;
using Firebase.Auth;
using Google;
using Firebase;
using Firebase.Extensions;
using TMPro;
using System;
using static ScoreManager;
using System.Collections;

[System.Serializable]
public class Response
{
    public string auth;
    public string results;
}

public class FirebaseManager : MonoBehaviour
{
    private const string WEB_CLIENT_ID = "844312363775-nf2lgo8lsrn6eev1aelmto602ucalkmv.apps.googleusercontent.com";

    [SerializeField] GameObject Username;
    [SerializeField] GameObject UsernameText;
    public static FirebaseManager instance;

    private FirebaseAuth auth;
    public FirebaseUser User;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                PopUpManager.instance.CreateErrorPopup("ERROR", $"Failed to check Firebase dependencies: {task.Exception}");
                Debug.LogError($"Failed to check Firebase dependencies: {task.Exception}");
                return;
            }

            InitializeFirebase();
        });

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        if (auth.CurrentUser != null)
        {
            Username.SetActive(true);
            UsernameText.GetComponent<TextMeshProUGUI>().text = GetGoogleUsername();
            auth.CurrentUser.TokenAsync(true).ContinueWithOnMainThread(tokenTask =>
            {
                if (tokenTask.IsCanceled)
                {
                    Debug.LogError("TokenAsync was canceled.");
                    return;
                }
                if (tokenTask.IsFaulted)
                {
                    Debug.LogError("TokenAsync encountered an error.");
                    return;
                }

                string firebaseToken = tokenTask.Result;
                APIManager.instance.AuthToken = firebaseToken;
            });
        }
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
            }
        }

        if (auth.CurrentUser != null)
        {
            Username.SetActive(true);
            UsernameText.GetComponent<TextMeshProUGUI>().text = GetGoogleUsername();
        }
    }

    void OnDestroy()
    {
        UsernameText.GetComponent<TextMeshProUGUI>().text = "Niezalogowany";
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void SignInWithGoogle(Action callback)
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            RequestEmail = true,
            WebClientId = WEB_CLIENT_ID
        };

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Sign-in canceled.");
                    GoogleSignIn.DefaultInstance.SignOut();
                    PopUpManager.instance.CreateErrorPopup("Anulowano", "Anulowano logowanie Google");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Sign-in encountered an error.");
                    Debug.LogError(task.Exception);
                    GoogleSignIn.DefaultInstance.SignOut();
                    PopUpManager.instance.CreateErrorPopup("Error", "Wyst¹pi³ b³¹d z logowaniem");
                }
                else
                {
                    GoogleSignInUser googleUser = task.Result;
                    Debug.Log("Google sign-in succeeded.");

                    string idToken = googleUser.IdToken;
                    string accessToken = googleUser.AuthCode;

                    SignInWithGoogleOnFirebase(idToken, accessToken, callback);
                }
            });
    }

    private void SignInWithGoogleOnFirebase(string idToken, string accessToken, Action callback)
    {
        try 
        {
            Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Sign-in canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Firebase Sign-in encountered an error.");
                    return;
                }

                User = task.Result;
                //Debug.Log("User signed in successfully" + User.DisplayName + " " + User.UserId);

                // Retrieve the Firebase token
                User.TokenAsync(true).ContinueWithOnMainThread(tokenTask =>
                {
                    if (tokenTask.IsCanceled)
                    {
                        Debug.LogError("TokenAsync was canceled.");
                        return;
                    }
                    if (tokenTask.IsFaulted)
                    {
                        Debug.LogError("TokenAsync encountered an error.");
                        return;
                    }

                    string firebaseToken = tokenTask.Result;
                    APIManager.instance.AuthToken = firebaseToken;

                    callback?.Invoke();
                });
            });
        }
        catch (System.Exception e) 
        {
            Debug.LogError(e.Message);
        }
    }

    public static bool IsUserLoggedIn()
    {
        if (FirebaseAuth.DefaultInstance != null)
            return FirebaseAuth.DefaultInstance.CurrentUser != null;
        
        return false;
    }

    public string GetGoogleUsername()
    {
        if (FirebaseAuth.DefaultInstance != null)
            return FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;

        return "";
    }

    public IEnumerator GetUserUID(Action<string> callback)
    {
        string uid = "";
        yield return APIManager.instance.GetRequest(APIManager.USERS_URL,
            result =>
            {
                if (result == null)
                    return;

                string auth = JsonUtility.FromJson<Response>(result).auth;
                uid = auth;
            }
        );

        callback?.Invoke(uid);
    }
}
