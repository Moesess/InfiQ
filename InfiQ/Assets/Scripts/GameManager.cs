using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        PlayerPrefs.SetFloat("LastEmailTime", -300f); // Po w³¹czeniu gry by mo¿na by³o wys³aæ zg³oszenie
        PlayerPrefs.SetInt("EmailCount", 0); // DEBUG ONLY
    }
}
