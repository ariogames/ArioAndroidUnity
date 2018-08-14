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


}
