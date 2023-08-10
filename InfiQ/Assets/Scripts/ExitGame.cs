using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    void Update()
    {
        // Check if the back button is pressed on Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame(false);
        }
    }

    public static void QuitGame(bool bQuit)
    {
        if (bQuit) 
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        else
            PopUpManager.instance.CreateConfirmationPopup("Czy na pewno chcesz wyjœæ z gry?", "Wychodzê", "Zostaje!");
    }
}
