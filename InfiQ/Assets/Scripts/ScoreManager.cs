using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreBoard;

    public static ScoreManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if( instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OpenScoreboard()
    {
        PopulateScoreboard();
        Instantiate(scoreBoard);
        scoreBoard.SetActive(true);
        
    }
    public void CloseScoreboard()
    {
        scoreBoard.SetActive(false);
        DestroyImmediate(scoreBoard);
        DePopulateScoreboard();
    }
    private void PopulateScoreboard()
    {
        //StartCoroutine(APIManager.instance.GetRequest(APIManager.USERS_URL),
        //    result =>
        //    {

        //    });
    }
    private void DePopulateScoreboard()
    {
        //TODO
    }
}
