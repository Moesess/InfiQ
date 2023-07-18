using UnityEngine;
using Firebase.Auth;
using Google;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignInWithGoogle()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "844312363775-nf2lgo8lsrn6eev1aelmto602ucalkmv.apps.googleusercontent.com"
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
                    SignInWithGoogleOnFirebase(googleUser.IdToken);
                }
            });
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(
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
                    FirebaseUser newUser = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                }
            });
    }
}

