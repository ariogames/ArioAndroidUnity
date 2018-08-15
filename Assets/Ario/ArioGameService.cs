using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ario.Models;


public class ArioGameService : MonoBehaviour
{

#if !UNITY_EDITOR && UNITY_ANDROID
    private AndroidJavaClass androidClass;
#endif

    private Action<bool> _onLoginStateListener = null;

    public System.Action<bool> OnLoginStateListener
    {
        get { return _onLoginStateListener; }
        set { _onLoginStateListener = value; }
    }

    private Action<bool> _onConnectListener = null;
    public System.Action<bool> OnConnectListener
    {
        get { return _onConnectListener; }
        set { _onConnectListener = value; }
    }

    private String APP_ID = "";
    private String APP_KEY = "";

    static private ArioGameService _instance = null;
    static public ArioGameService Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ArioGameServiceObject");
                obj.name = "ArioGameServiceObject";
                _instance = obj.AddComponent<ArioGameService>();
            }

            return _instance;
        }
    }


    public void init(string app_id, string APP_KEY)
    {
        this.APP_ID = app_id;
        this.APP_KEY = APP_KEY;
    }

    public void setAutoStartSignInFlow(bool isAutoStartSignInFlow)
    {
        Debug.Log("AriogameService: set auto start sign in flow into " + isAutoStartSignInFlow);

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("setAutoStartSignInFlow", isAutoStartSignInFlow);
        #endif
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        #if !UNITY_EDITOR && UNITY_ANDROID
            AndroidJNI.AttachCurrentThread();
            androidClass = new AndroidJavaClass("com.arioclub.unity.sdk.android.ArioUnitySdkInterface");         
        #endif
    }

    public bool IsStorePackageInstalled()
    {
        bool isAppInstalled = false;

        Debug.Log("ArioGameService : IsStorePackageInstalled() is called  ");

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            isAppInstalled = (bool)androidClass.CallStatic<bool>( "isStorePackageInstalled" );
        #endif

        if (isAppInstalled)
            Debug.Log("Ario android app is installed");
        else
            Debug.Log("Ario android app is not installed");


        return isAppInstalled;
    }

//================================================================================================= Authentication
    public void SignIn()
    {
        Debug.Log("ArioGameService :  SignIn() called");
        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("signIn",
                                    gameObject.name,
                                    "OnConnectedToArioListener" );
        #endif
    }

    public void SignOut()
    {
        Debug.Log("ArioGameService :  SignOut() is called");

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("signOut") ; 
        #endif
    }

    public void isLogin()
    {
        Debug.Log("ArioGameService :  isLogin() called");
        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("isLoginInArio",
                                    gameObject.name,
                                    "OnLoginStateInArioListener" );
        #endif
    }

    public bool IsConnected()
    {
        bool result = false;


        #if (UNITY_ANDROID) && !UNITY_EDITOR
            result =  (bool) androidClass.CallStatic<bool>("isConnetedToArio");
        #endif
        Debug.Log("ArioGameService :  IsConnected() is called , Ario is " + ((result) ? "connected" : " not connected"));

        return result;
    }


    private void OnLoginStateInArioListener(String response)
    {
        bool result = (response == "1") ? true : false;
        Debug.Log("ArioGameService : OnLoginStateInArioListener() is called , User in Ario" + ((result) ? "is Login" : "not Login"));
        if (_onLoginStateListener != null)
            _onLoginStateListener(result);
    }
    private void OnConnectedToArioListener(String response)
    {
        bool result = (response == "1") ? true : false;

        Debug.Log("ArioGameService : OnConnectSucceedJavaListener() is called , Ario is" + ((!result) ? "not connected" : "connected"));
        if (_onConnectListener != null)
            _onConnectListener(result);
    }


//================================================================================================= Achievement

    public void UnlockAchievement(string achievementID)
    {
        int temp;
        if (!int.TryParse(achievementID, out temp))
        {
            Debug.LogError("ArioGameService :  Invalid achivement id: " + achievementID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("unlockAchievement", achievementID, APP_KEY) ; 
        #endif

        Debug.Log("ArioGameService :  UnlockAchievement() is called");
    }

    public void IncrementAchievement(string achievementID, int incrementNumber)
    {
        int temp;
        if (!int.TryParse(achievementID, out temp))
        {
            Debug.LogError("ArioGameService :  Invalid achivementId: " + achievementID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("incrementAchievement", achievementID, incrementNumber, APP_KEY) ; 
        #endif
        Debug.Log("ArioGameService :  IncrementAchievement() is called");

    }

    public void ShowAllAchievements()
    {
        int temp;
        if (!(int.TryParse(APP_ID, out temp)))
        {
            Debug.LogError("ArioGameService :  Invalid APP_ID: " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("showAllAchievements", APP_ID) ; 
        #endif
        Debug.Log("ArioGameService :  ShowAllAchievements() is called");

    }

    private Action<AchievementList> _onGetAchievementInfo = null;
    public System.Action<AchievementList> OnGetAchievementInfo
    {
        get { return _onGetAchievementInfo; }
        set { _onGetAchievementInfo = value; }
    }
    public void LoadAllAchievement()
    {
        Debug.Log("ArioGameService :  LoadAchievement() is called");

        if ((APP_ID.Equals("")))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }

            #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("loadAchievement",
                                    gameObject.name,
                                    "OnGetAchievement",
                                    APP_ID) ; 
            #endif
    }

    private void OnGetAchievement(string achievementArray)
    {
        if (_onGetAchievementInfo != null)
        {
            AchievementList result = JsonUtility.FromJson<AchievementList>(achievementArray);
            _onGetAchievementInfo(result);
        }
        else
        {
            Debug.Log("ArioGameService :  onGetAchievement Callback not defined!!");
        }
    }


    //================================================================================================= Leaderboard

    public void SubmitScoreToLeaderboard(string leaderboardID, long score)
    {
         if (APP_ID.Equals("") || APP_KEY.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("submitScore", leaderboardID, score.ToString(), APP_ID, APP_KEY) ; 
        #endif
        Debug.Log("ArioGameService :  SubmitScoreToLeaderboard() is called");
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        int temp;
        if (!(int.TryParse(leaderboardID, out temp) && int.TryParse(APP_ID, out temp)))
        {
            Debug.LogError("ArioGameService :  Invalid leaderboardID or APP_ID: " + leaderboardID + ", " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
                androidClass.CallStatic("showLeaderboard", leaderboardID, APP_ID) ; 
        #endif
            Debug.Log("ArioGameService :  ShowLeaderboard() is  called");
    }

    public void ShowAllLeaderboards()
    {
        int temp;
        if (!int.TryParse(APP_ID, out temp))
        {
            Debug.LogError("ArioGameService :  Invalid APP_ID: " + APP_ID);
            return;
        }
        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("showAllLeaderboards", APP_ID) ; 
        #endif
        Debug.Log("ArioGameService :  ShowAllLeaderboards() is  called");

    }

    private Action<LeaderboardList> _OnGetLeaderboardsMetadata = null;
    public Action<LeaderboardList> onGetLeaderboardsMetada 
    {
        get {return _OnGetLeaderboardsMetadata;}
        set {_OnGetLeaderboardsMetadata = value;}
    }
    public void loadLeaderboardsMetadata() {
        if (APP_ID.Equals("") || APP_KEY.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("loadAllLeaderboardMetadata", gameObject.name, "OnGetLeaderboardsMetadata", APP_ID, APP_KEY) ; 
        #endif      
        Debug.Log("ArioGameService :  loadLeaderboardsMetadata() is called");
    }

    private void OnGetLeaderboardsMetadata(string leaderboardsMetadata) 
    {
        if (_OnGetLeaderboardsMetadata != null)
        {
            LeaderboardList result = JsonUtility.FromJson<LeaderboardList>(leaderboardsMetadata);
            _OnGetLeaderboardsMetadata(result);
        }
        else
        {
            Debug.Log("ArioGameService :  OnGetLeaderboard Callback not defined!!");
        }
    }

    private Action<ScoreList> _OnGetLeaderboardScore = null;
    public Action<ScoreList> onGetLeaderboardScore 
    {
        get {return _OnGetLeaderboardScore;}
        set {_OnGetLeaderboardScore = value;}
    }
    public void loadLeaderboardScore(string leaderboardID, int timeSpan, int leaderboardCollection, int maxResult) {
        if (APP_ID.Equals("") || APP_KEY.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("loadLeaderboardRecords", gameObject.name, "OnGetLeaderboardScore", leaderboardID, timeSpan, leaderboardCollection, maxResult, APP_ID, APP_KEY); 
        #endif      
        Debug.Log("ArioGameService :  loadLeaderboardScore() is called");
    }

    private void OnGetLeaderboardScore(string leaderboardScores) 
    {
        if (_OnGetLeaderboardScore != null)
        {
            ScoreList result = JsonUtility.FromJson<ScoreList>(leaderboardScores);
            _OnGetLeaderboardScore(result);
        }
        else
        {
            Debug.Log("ArioGameService :  OnGetLeaderboard Callback not defined!!");
        }
    }

    private Action<ScoreList> _OnGetLeaderboardScoreCurrentPlayer = null;
    public Action<ScoreList> onGetLeaderboardScoreCurrentPlayer
    {
        get {return _OnGetLeaderboardScoreCurrentPlayer;}
        set {_OnGetLeaderboardScoreCurrentPlayer = value;}
    }
    public void loadLeaderboardScoreCurrentPlayer(string leaderboardID, int timeSpan, int leaderboardCollection) {
        if (APP_ID.Equals("") || APP_KEY.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }

        #if (UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("loadCurrentPlayerRecord", gameObject.name, "OnGetLeaderboardScoreCurrentPlayer", leaderboardID, timeSpan, leaderboardCollection, APP_ID, APP_KEY); 
        #endif      
        Debug.Log("ArioGameService :  loadLeaderboardScoreCurrentPlayer() is called");
    }

    private void OnGetLeaderboardScoreCurrentPlayer(string leaderboardScores) 
    {
        if (_OnGetLeaderboardScore != null)
        {
            ScoreList result = JsonUtility.FromJson<ScoreList>(leaderboardScores);
            _OnGetLeaderboardScoreCurrentPlayer(result);
        }
        else
        {
            Debug.Log("ArioGameService :  OnGetLeaderboard Callback not defined!!");
        }
    }

    //================================================================================================= Social Services

    public void ShowScreeenShotPage()
    {
        if (APP_ID.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }
        
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string fileName = "Screeenshot" + timeStamp + ".png";
            ScreenCapture.CaptureScreenshot(fileName);
            string[] pathes = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");
            androidClass.CallStatic("showPostScreenshot", pathes[0], APP_ID); 
        }
        #endif   

        Debug.LogError("ArioGameService :  ShowScreeenShotPage called");
    }

    public void ShowScreeenShotPage(string fileName)
    {
        if (APP_ID.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }
        
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            androidClass.CallStatic("showPostScreenshot", fileName, APP_ID); 
        } else {
            Debug.LogError("Ario Application not installed on this device");    
        }
        #endif   

        Debug.LogError("ArioGameService :  ShowScreeenShotPage called");
    }

    public void RateOnGame()
    {
        if (APP_ID.Equals(""))
        {
            Debug.LogError("ArioGameService :  Invalid  APP_ID: " + APP_ID);
            return;
        }
        
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            androidClass.CallStatic("rateOnGame"); 
        } else {
            Debug.LogError("Ario Application not installed on this device");    
        }
        #endif   

        Debug.LogError("ArioGameService :  RateOnGame called");
    }

    //================================================================================================= Store Services

    public void ShowGamePage() {
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            androidClass.CallStatic("showGamePage"); 
        } else {
            Debug.LogError("Ario Application not installed on this device");    
        }
        #endif   

        Debug.LogError("ArioGameService :  ShowGamePage called");
    }

    public void ShowDeveloperPage(string developerId) {
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            androidClass.CallStatic("showDeveloperPage", developerId); 
        } else {
            Debug.LogError("Ario Application not installed on this device");    
        }
        #endif   

        Debug.LogError("ArioGameService :  ShowDeveloperPage called");
    }

    private Action<int> _OnCheckUpdate = null;
    public Action<int> onCheckUpdate
    {
        get {return _OnCheckUpdate;}
        set {_OnCheckUpdate = value;}
    }

    public void CheckUpdate() {
        #if (UNITY_ANDROID) && !UNITY_EDITOR
        if(IsStorePackageInstalled()) {
            androidClass.CallStatic("checkUpdateForGame", gameObject.name, "OnCheckUpdate"); 
        } else {
            Debug.LogError("Ario Application not installed on this device");    
        }
        #endif   

        Debug.LogError("ArioGameService :  Check Update called");
    }

    private void OnCheckUpdate(string result) {
        int version = 0;
        if (int.TryParse(result, out version)) {
            version = Int32.Parse(result);
        } else {
            version = -2;
        }
        _OnCheckUpdate(version);
    }
}
