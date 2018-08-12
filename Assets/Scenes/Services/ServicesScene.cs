using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//
// Sample scene which uses Ario achievements and leaderboard sdk
// please note that it's important to implement callbacks 
// and register them before calling ArioGameService.Instance methods
public class ServicesScene : MonoBehaviour
{
    public Text message = null;
    public InputField input = null;

    private int DEFAULT_FONT_SIZE = 36;

    // Sample leaderboard id in Ario Run&Jump example game
    // you should enter your leaderboard_id here
    string leaderboardID = "36";

    void Start()
    {
    }

    public void IsStorePackageInstalled()
    {
        message.text = ArioGameService.Instance.IsStorePackageInstalled() ? " Ario is installed " : " Ario is not installed";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    

   






    public void SubmitScoreToLeaderboard()
    {
        long score;
        if (input != null && input.text != null && input.text.Length > 0 && long.TryParse(input.text, out score))
        {
            ArioGameService.Instance.SubmitScoreToLeaderboard(leaderboardID, long.Parse(input.text));
        }
        else
        {
            message.text = "Enter Score, must be number!";
            message.fontSize = DEFAULT_FONT_SIZE;
        }
    }

    public void ShowLeaderboard()
    {
        ArioGameService.Instance.ShowLeaderboard(leaderboardID);
        message.text = "Show leaderboard " + leaderboardID + " called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void ShowAllLeaderboards()
    {
        ArioGameService.Instance.ShowAllLeaderboards();
        message.text = "Show all leaderboards called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }


    public void OnGetAchievementInfo(string achievement)
    {
        message.text = achievement;
        message.fontSize = 18;
    }

    public void onBackPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
