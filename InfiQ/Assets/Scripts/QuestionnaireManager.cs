using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireManager : MonoBehaviour
{
    [SerializeField]
    string url;
    // Start is called before the first frame update
    public void OpenUrl()
    {
        Application.OpenURL(url);
    }
}
