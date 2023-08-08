using UnityEngine;
using Firebase.Auth;
using Google;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    private const string WEB_CLIENT_ID = "844312363775-nf2lgo8lsrn6eev1aelmto602ucalkmv.apps.googleusercontent.com";

    [SerializeField] GameObject info;

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
                    info.GetComponent<TextMeshProUGUI>().text += "Sign-in canceled.";
                }
                else if (task.IsFaulted)
                {
                    info.GetComponent<TextMeshProUGUI>().text += "Sign-in encountered an error.";
                    Debug.LogError(task.Exception);
                }
                else
                {
                    GoogleSignInUser googleUser = task.Result;
                    info.GetComponent<TextMeshProUGUI>().text += "Google sign-in succeeded.";

                    string idToken = googleUser.IdToken;
                    string accessToken = googleUser.AuthCode;

                    SignInWithGoogleOnFirebase(idToken, accessToken);
                }
            });
    }

    private void SignInWithGoogleOnFirebase(string idToken, string accessToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                info.GetComponent<TextMeshProUGUI>().text += "Sign-in canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                info.GetComponent<TextMeshProUGUI>().text += "Firebase Sign-in encountered an error.";
                return;
            }

            FirebaseUser newUser = task.Result;
            info.GetComponent<TextMeshProUGUI>().text += ("User signed in successfully" + newUser.DisplayName + " " + newUser.UserId);

            // Retrieve the Firebase token
            newUser.TokenAsync(true).ContinueWith(tokenTask =>
            {
                if (tokenTask.IsCanceled)
                {
                    info.GetComponent<TextMeshProUGUI>().text += ("TokenAsync was canceled.");
                    return;
                }
                if (tokenTask.IsFaulted)
                {
                    info.GetComponent<TextMeshProUGUI>().text += ("TokenAsync encountered an error.");
                    return;
                }

                string firebaseToken = tokenTask.Result;
                info.GetComponent<TextMeshProUGUI>().text += ("Token: " + firebaseToken);
                APIManager.instance.AuthToken = firebaseToken;
            });
        });
    }

    private void HandleError(string message, System.Exception exception = null)
    {
        Debug.LogError(message);
        info.GetComponent<TextMeshProUGUI>().text += message;
        if (exception != null)
        {
            Debug.LogError(exception);
        }
    }
}
