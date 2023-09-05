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

    public void FillElement(string place, string nick, string score, string duration)
    {
        this.place.text = place;
        this.nick.text = nick;
        this.score.text = score;
        this.duration.text = duration;
    }
}
