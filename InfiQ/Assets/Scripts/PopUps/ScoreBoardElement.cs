using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardElement : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI place;
    [SerializeField]
    TextMeshProUGUI nick;
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    TextMeshProUGUI duration;

    public string FormatDuration(string duration)
    {
        if (duration.Contains("."))
        {
            string[] parts = duration.Split('.');
            if (parts.Length == 2)
            {
                string milliseconds = parts[1].Length > 2 ? parts[1].Substring(0, 2) : parts[1];
                return $"{parts[0]}.{milliseconds}";
            }
        }
        return duration;
    }

    public void FillElement(string place, string nick, string score, string duration)
    {
        this.place.text = place;
        this.nick.text = nick;
        this.score.text = score;
        this.duration.text = FormatDuration(duration);
    }
}
