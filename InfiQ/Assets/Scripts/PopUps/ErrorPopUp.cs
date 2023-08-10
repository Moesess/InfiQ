using TMPro;
using UnityEngine;

public class ErrorPopUp : MonoBehaviour
{
    [SerializeField] GameObject Title;
    [SerializeField] GameObject Desc;

    public void Fill(string sTitle, string sDesc)
    {
        Title.GetComponent<TextMeshProUGUI>().text = sTitle;
        Desc.GetComponent<TextMeshProUGUI>().text = sDesc;
    }
}
