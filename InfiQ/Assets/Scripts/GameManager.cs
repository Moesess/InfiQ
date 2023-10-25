using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        PlayerPrefs.SetFloat("LastEmailTime", -30f); // Po w��czeniu gry by mo�na by�o wys�a� zg�oszenie
        PlayerPrefs.SetInt("EmailCount", 0); // DEBUG ONLY
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        {
            Debug.Log("First Time Opening");

            
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
            PlayerPrefs.SetInt("WASQUESTIONNAIREFILLED", 0);

        }
        else
        {
            Debug.Log("NOT First Time Opening");

            //Do your stuff here
        }
    }
}
