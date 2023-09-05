using UnityEngine;
using Firebase.Auth;
using Google;
using Firebase;
using Firebase.Extensions;
using TMPro;
using System;

public class FirebaseManager : MonoBehaviour
{
    private const string WEB_CLIENT_ID = "844312363775-nf2lgo8lsrn6eev1aelmto602ucalkmv.apps.googleusercontent.com";

    [SerializeField] GameObject Username;
    [SerializeField] GameObject UsernameText;

    private FirebaseAuth auth;
    public FirebaseUser User;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to check Firebase dependencies: {task.Exception}");
                return;
            }

            InitializeFirebase();
        });
    }

    private void Start()
    {
        if (!IsUserLoggedIn())
        {
            UsernameText.GetComponent<TextMeshProUGUI>().text = "Niezalogowany";
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
            UsernameText.GetComponent<TextMeshProUGUI>().text = GetUsername();
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
            UsernameText.GetComponent<TextMeshProUGUI>().text = GetUsername();
        }
    }

    void OnDestroy()
    {
        UsernameText.GetComponent<TextMeshProUGUI>().text = "Niezalogowany";
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            RequestEmail = true,
            WebClientId = WEB_CLIENT_ID
        };

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Sign-in canceled.");
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError("Sign-in encountered an error.");
                    Debug.LogError(task.Exception);
                }
                else
                {
                    GoogleSignInUser googleUser = task.Result;
                    Debug.Log("Google sign-in succeeded.");

                    string idToken = googleUser.IdToken;
                    string accessToken = googleUser.AuthCode;

                    SignInWithGoogleOnFirebase(idToken, accessToken);
                }
            });
    }

    private void SignInWithGoogleOnFirebase(string idToken, string accessToken)
    {
        try 
        {
            Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
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
                User.TokenAsync(true).ContinueWith(tokenTask =>
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

    public string GetUsername()
    {
        if (FirebaseAuth.DefaultInstance != null)
            return FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;

        return "";
    }
}
