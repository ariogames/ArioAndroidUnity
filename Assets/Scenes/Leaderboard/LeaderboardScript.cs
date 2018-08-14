using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ario.Models;

public class LeaderboardScript : MonoBehaviour
{

    public InputField inputLeaderboardIdSubmitScore;
    public InputField inputScore;
    public InputField inputLeaderboardIdGetRecords;
    public InputField inputLeaderboardIdShow;
    public Text textLeaderboardOne;
    public Text textLeaderboardTwo;
    public Text textRecords;
    public Text textCurrentPlayerScore;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void SubmitScoreToLeaderboard()
    {
        if (inputLeaderboardIdSubmitScore != null && inputLeaderboardIdSubmitScore.text != null && inputLeaderboardIdSubmitScore.text.Length > 0
        && inputScore != null && inputScore.text != null && inputScore.text.Length > 0)
        {
            ArioGameService.Instance.SubmitScoreToLeaderboard(inputLeaderboardIdSubmitScore.text, long.Parse(inputScore.text));
        }
    }

    public void ShowLeaderboard()
    {
        if (inputLeaderboardIdShow != null && inputLeaderboardIdShow.text != null && inputLeaderboardIdShow.text.Length > 0)
        {
            ArioGameService.Instance.ShowLeaderboard(inputLeaderboardIdShow.text);
        }
    }

    public void ShowAllLeaderboards()
    {
        ArioGameService.Instance.ShowAllLeaderboards();
    }

    public void GetLeaderboardMetadata()
    {
        ArioGameService.Instance.loadLeaderboardsMetadata();
        ArioGameService.Instance.onGetLeaderboardsMetada = OnGetLeaderboardsMetadata;
    }

    public void OnGetLeaderboardsMetadata(LeaderboardList leaderboardList)
    {
        if (leaderboardList.list.Length == 0) {
            textLeaderboardOne.text = "no leaderboard data found";
        } else if (leaderboardList.list.Length >= 1){
            LeaderboardList.Leaderboard leaderboard = leaderboardList.list[0];
            textLeaderboardOne.text = "id: " + leaderboard.id + "\nname: " + leaderboard.name + "\nScoreOrder: " + (leaderboard.scoreOrder == LeaderboardList.SCORE_ORDER_LARGER_IS_BETTER ? "Larger_is_Better" : "Smaller_is_Better") + "\niconUrl:" + leaderboard.iconUrl;
        } if (leaderboardList.list.Length >=2) {
            LeaderboardList.Leaderboard leaderboard = leaderboardList.list[1];
            textLeaderboardTwo.text = "id: " + leaderboard.id + "\nname: " + leaderboard.name + "\nScoreOrder: " + (leaderboard.scoreOrder == LeaderboardList.SCORE_ORDER_LARGER_IS_BETTER ? "Larger_is_Better" : "Smaller_is_Better") + "\niconUrl:" + leaderboard.iconUrl;
        }
    }

    public void GetScores()
    {
        if (inputLeaderboardIdGetRecords != null && inputLeaderboardIdGetRecords.text != null && inputLeaderboardIdGetRecords.text.Length > 0) {
            ArioGameService.Instance.loadLeaderboardScore(inputLeaderboardIdGetRecords.text, ScoreList.TIME_STAMP_ALL_TIME, ScoreList.COLLECTION_PUBLIC, 5);
            ArioGameService.Instance.onGetLeaderboardScore = OnGetLeaderboardScores;
        }
    }

    private void OnGetLeaderboardScores(ScoreList scoreList) {
        if (scoreList.list.Length == 0) {
            textRecords.text = "there is no score submitted yet";
        } else {
            for (int i=0; i<scoreList.list.Length; i++) {
                ScoreList.Score score = scoreList.list[i];
                textRecords.text = textRecords.text + score.rank.ToString() + "." + score.playerName + "|" + score.score.ToString() + "  ";
            }
        }
    }

    public void GetScoreOfCurrentUser()
    {
        if (inputLeaderboardIdGetRecords != null && inputLeaderboardIdGetRecords.text != null && inputLeaderboardIdGetRecords.text.Length > 0) {
            ArioGameService.Instance.loadLeaderboardScoreCurrentPlayer(inputLeaderboardIdGetRecords.text, ScoreList.TIME_STAMP_ALL_TIME, ScoreList.COLLECTION_PUBLIC);
            ArioGameService.Instance.onGetLeaderboardScoreCurrentPlayer = OnGetScoreOfCurrentPlayer;

        }
    }

    private void OnGetScoreOfCurrentPlayer(ScoreList scoreList) {
        if (scoreList.list.Length == 0) {
            textCurrentPlayerScore.text = "this player still has no score!";
        } else {
            textCurrentPlayerScore.text = "name: " + scoreList.list[0].playerName + "   rank: " + scoreList.list[0].rank.ToString() + "   score: " + scoreList.list[0].score.ToString();
        }
    }
}
