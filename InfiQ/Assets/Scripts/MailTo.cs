using Firebase.Auth;
using Google.MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailTo : MonoBehaviour
{
    [SerializeField] public TMP_InputField Subject;
    [SerializeField] public TMP_InputField Description;

    public void SendEmail()
    {
        if (FirebaseManager.IsUserLoggedIn())
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Musisz by� zalogowany by wys�a� zg�oszenie.");
        }

        float emailCooldown = 30f;
        int emailCount = PlayerPrefs.GetInt("EmailCount", 0);
        float lastEmailTime = PlayerPrefs.GetFloat("LastEmailTime", 0f);
        float currentTime = Time.time;
        string lastEmailDate = PlayerPrefs.GetString("LastEmailDate", "");
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        if (currentTime - lastEmailTime < emailCooldown)
        { 
            PopUpManager.instance.CreateErrorPopup("B��D", "Odczekaj troch� przed wys�aniem kolejnego zg�oszenia.");
            return;
        }

        if (currentDate != lastEmailDate)
        { 
            PlayerPrefs.SetInt("EmailCount", 0);
            emailCount = 0;
        }

        if (emailCount >= 10)
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Osi�gn��e� limit zg�osze� na dzi�!.");
            return;
        }

        if (String.IsNullOrEmpty(Subject.text))
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Pole tytu� nie mo�e by� puste!");
            return;
        }

        if (String.IsNullOrEmpty(Description.text))
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Pole opis nie mo�e by� puste!");
            return;
        }

        if (Subject.text.Length < 10)
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Pole tytu� jest za kr�tkie! Musi mie� min 10 znak�w");
            return;
        }

        Debug.Log(Description.text.Length);

        if (Description.text.Length < 30)
        {
            PopUpManager.instance.CreateErrorPopup("B��D", "Pole opis jest za kr�tkie! Musi mie� min. 30 znak�w");
            return;
        }

        Debug.Log("Wys�ano mail");
            
        string json = "{\"subject\": \"" + Subject.text + "\",\"username\": \"User " + "FirebaseAuth.DefaultInstance.CurrentUser.DisplayName" + "\",\"message\": \"" + Description.text + "\"}";
        Debug.Log(json);

        emailCount += 1;

        PlayerPrefs.SetFloat("LastEmailTime", currentTime);
        PlayerPrefs.SetString("LastEmailDate", currentDate);
        PlayerPrefs.SetInt("EmailCount", emailCount);

        StartCoroutine(APIManager.instance.PostRequestWithRetry(APIManager.EMAIL_URL, json, result => 
        {
            if (result == null)
            {
                PopUpManager.instance.CreateErrorPopup("B��D", "Co� posz�o nie tak! Zg�oszenie nie zosta�o wys�ane.");
                return;
            }
            else
            {
                PopUpManager.instance.CreateErrorPopup("SUKCES", "Pomy�lnie wys�ano zg�oszenie.");               
            }            
        }));

        gameObject.GetComponent<PopUp>().Close();
    }
}
