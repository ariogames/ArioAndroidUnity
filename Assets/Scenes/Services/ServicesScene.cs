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

    public void SignIn()
    {
        ArioGameService.Instance.OnConnectListener = OnConnectListener;
        ArioGameService.Instance.SignIn();
        message.text = "Sign in called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void SignOut()
    {
        ArioGameService.Instance.OnDisconnectListener = OnDisconnectListener;
        ArioGameService.Instance.SignOut();
        message.text = "Sign out called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void IsConnected()
    {
        message.text = (ArioGameService.Instance.IsConnected()) ? " Ario is connected " : " Ario is not connected";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void UnlockAchievement()
    {
        // you can use 60 as achievement id for test, which is TEST_ACHIEVEMENT in Ario Run&Jump sample game
        // keep in mind that achievement is given to user if user has installed your app from Ario, 
        // otherwise this has no effect; 
        if (input != null && input.text != null && input.text.Length > 0)
        {
            ArioGameService.Instance.UnlockAchievement(input.text);
            message.text = "Unlock Achievement called";
            message.fontSize = DEFAULT_FONT_SIZE;
        }
        else
        {
            message.text = "Enter Achievement id first";
            message.fontSize = DEFAULT_FONT_SIZE;
        }

    }

    public void IncrementAchievement()
    {   // you can use 60 as achievement id for test, which is TEST_ACHIEVEMENT in Ario Run&Jump sample game
        // keep in mind that achievement is given to user if user has installed your app from Ario, 
        // otherwise this has no effect; 
        if (input != null && input.text != null && input.text.Length > 0)
        {
            ArioGameService.Instance.IncrementAchievement(input.text, 1);
            message.text = "Increment Achievement called";
            message.fontSize = DEFAULT_FONT_SIZE;
        }
        else
        {
            message.text = "Enter Achievement id first";
            message.fontSize = DEFAULT_FONT_SIZE;
        }
    }

    public void ShowAllAchievements()
    {
        ArioGameService.Instance.ShowAllAchievements();
        message.text = "Show all achievements called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void GetAchievement()
    {
        if (input != null && input.text != null && input.text.Length > 0)
        {
            ArioGameService.Instance.LoadAchievement(input.text);
            ArioGameService.Instance.OnGetAchievementInfo = OnGetAchievementInfo;
        }
        else
        {
            message.text = "Enter Achievement id first";
            message.fontSize = DEFAULT_FONT_SIZE;
        }
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

    private void OnDisconnectListener()
    {
        message.text = "Ario successfully disconnected";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    private void OnConnectListener(bool isConnected)
    {
        message.text = isConnected ? " Ario connected successfully" : " Ario connection failed";
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
