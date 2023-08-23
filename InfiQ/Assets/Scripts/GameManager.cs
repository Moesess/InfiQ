using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        PlayerPrefs.SetFloat("LastEmailTime", -300f); // Po w��czeniu gry by mo�na by�o wys�a� zg�oszenie
        PlayerPrefs.SetInt("EmailCount", 0); // DEBUG ONLY
    }
}
