using System;
using TMPro;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    GameObject ScoreBoard;

    [SerializeField]
    GameObject ScoreBoardElement;

    [SerializeField]
    TextMeshProUGUI PlayerPlace;

    [SerializeField]
    TextMeshProUGUI PlayerName;

    [SerializeField]
    TextMeshProUGUI PlayerScore;

    [SerializeField]
    TextMeshProUGUI PlayerTime;

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

    [System.Serializable]
    public class User
    {
        public string username;
        public int rank;
        public int score;
        public float duration;
    }

    public void PopulateScoreboard(string actualTestType)
    {
        DePopulateScoreboard();

        FirebaseManager.instance.GetUserUID(
            x => StartCoroutine(APIManager.instance.GetRequest(APIManager.USER_HIGH_SCORES_URL + actualTestType + "&user_uid=" + x,
            result =>
            {
                if (result == null)
                    return;

                User response = JsonUtility.FromJson<User>(result);
                PlayerPlace.text = response.rank.ToString();
                PlayerName.text = response.username;
                PlayerScore.text = response.score.ToString();
                PlayerTime.text = Math.Round(response.duration, 2).ToString();
            }   
            ))
        );

        StartCoroutine(APIManager.instance.GetRequest(APIManager.HIGH_SCORES_URL + actualTestType,
            result =>
            {
                if (result == null)
                    return;
                //Debug.Log(result);
                Score[] response = JsonUtility.FromJson<ScoreBoardResponse>("{\"scoreList\":" + result + "}").scoreList;
                int place = 1;
                foreach(Score score in response)
                {
                    GameObject scoreElement = Instantiate(ScoreBoardElement, ScoreBoard.transform.position, Quaternion.identity, ScoreBoard.transform);
                    scoreElement.GetComponent<ScoreBoardElement>().FillElement(place.ToString()+".", score.username, score.best_score, score.duration);
                    place++;
                }
            }
            ));
    }
    private void DePopulateScoreboard()
    {
        foreach(Transform score in ScoreBoard.transform)
        {
            Destroy(score.gameObject);
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
