using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreBoard;

    public void OpenScoreboard()
    {
        PopulateScoreboard();
        scoreBoard.SetActive(true);
    }
    public void CloseScoreboard()
    {
        scoreBoard.SetActive(false);
        DePopulateScoreboard();
    }
    private void PopulateScoreboard()
    {
        //TODO
    }
    private void DePopulateScoreboard()
    {
        //TODO
    }
}
