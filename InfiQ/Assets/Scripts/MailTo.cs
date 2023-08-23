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
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Musisz byæ zalogowany by wys³aæ zg³oszenie.");
        }

        float emailCooldown = 30f;
        int emailCount = PlayerPrefs.GetInt("EmailCount", 0);
        float lastEmailTime = PlayerPrefs.GetFloat("LastEmailTime", 0f);
        float currentTime = Time.time;
        string lastEmailDate = PlayerPrefs.GetString("LastEmailDate", "");
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        if (currentTime - lastEmailTime < emailCooldown)
        { 
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Odczekaj trochê przed wys³aniem kolejnego zg³oszenia.");
            return;
        }

        if (currentDate != lastEmailDate)
        { 
            PlayerPrefs.SetInt("EmailCount", 0);
            emailCount = 0;
        }

        if (emailCount >= 10)
        {
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Osi¹gn¹³eœ limit zg³oszeñ na dziœ!.");
            return;
        }

        if (String.IsNullOrEmpty(Subject.text))
        {
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Pole tytu³ nie mo¿e byæ puste!");
            return;
        }

        if (String.IsNullOrEmpty(Description.text))
        {
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Pole opis nie mo¿e byæ puste!");
            return;
        }

        if (Subject.text.Length < 10)
        {
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Pole tytu³ jest za krótkie! Musi mieæ min 10 znaków");
            return;
        }

        Debug.Log(Description.text.Length);

        if (Description.text.Length < 30)
        {
            PopUpManager.instance.CreateErrorPopup("B£¥D", "Pole opis jest za krótkie! Musi mieæ min. 30 znaków");
            return;
        }

        Debug.Log("Wys³ano mail");
            
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
                PopUpManager.instance.CreateErrorPopup("B£¥D", "Coœ posz³o nie tak! Zg³oszenie nie zosta³o wys³ane.");
                return;
            }
            else
            {
                PopUpManager.instance.CreateErrorPopup("SUKCES", "Pomyœlnie wys³ano zg³oszenie.");               
            }            
        }));

        gameObject.GetComponent<PopUp>().Close();
    }
}
