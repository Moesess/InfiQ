using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreBoard;

    [SerializeField]
    GameObject scoreBoardElement;

    [System.Serializable]
    public class Score
    {
        public string u_uid;
        public string username;
        public string best_score;
        public string duration;
    }

    [System.Serializable]
    public class ScoreBoardResponse
    {
        public Score[] scoreList;
    }

    public void PopulateScoreboard(string actualTestType)
    {
        DePopulateScoreboard();
        StartCoroutine(APIManager.instance.GetRequest(APIManager.HIGH_SCORES_URL + actualTestType,
            result =>
            {
                if (result == null)
                    return;
                Debug.Log(result);
                Score[] response = JsonUtility.FromJson<ScoreBoardResponse>("{\"scoreList\":" + result + "}").scoreList;
                int place = 1;
                foreach(Score score in response)
                {
                    GameObject scoreElement = Instantiate(scoreBoardElement, scoreBoard.transform.position, Quaternion.identity, scoreBoard.transform);
                    scoreElement.GetComponent<ScoreBoardElement>().FillElement(place.ToString()+".", score.username, score.best_score, score.duration);
                    place++;
                }
            }
            ));
    }
    private void DePopulateScoreboard()
    {
        foreach(Transform score in scoreBoard.transform)
        {
            GameObject.Destroy(score.gameObject);
        }    
    }
    public void ChangeToINF02()
    {
        PopulateScoreboard("INF.02");
    }
    public void ChangeToINF03()
    {
        PopulateScoreboard("INF.03");
    }
    public void ChangeToINF04()
    {
        PopulateScoreboard("INF.04");
    }
}
