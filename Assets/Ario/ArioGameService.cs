using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class ArioGameService : MonoBehaviour
{

#if !UNITY_EDITOR && UNITY_ANDROID
    private AndroidJavaClass androidClass;
#endif

    private Action<bool> _onLoginStateListener = null;

    public System.Action<bool> OnLoginStateListener
    {
        get {return _onLoginStateListener;}
        set {_onLoginStateListener = value;}
    }

    private Action<bool> _onConnectListener = null;
    public System.Action<bool> OnConnectListener
    {
        get { return _onConnectListener; }
        set { _onConnectListener = value; }
    }

       private Action<string> _onGetAchievementInfo = null;
    public System.Action<string> OnGetAchievementInfo
    {
        get { return _onGetAchievementInfo; }
        set { _onGetAchievementInfo = value; }
    }

    private String APP_ID = "";
    private String SECRET_KEY = "";
    
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


    public void init(string app_id, string secret_key) {
        this.APP_ID = app_id;
        this.SECRET_KEY = secret_key;
    }

    public void setAutoStartSignInFlow(bool isAutoStartSignInFlow) {
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

        #if ( UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR
            isAppInstalled = (bool)androidClass.CallStatic<bool>( "isStorePackageInstalled" );
        #endif

        if (isAppInstalled)
            Debug.Log("Ario android app is installed");
        else
            Debug.Log("Ario android app is not installed");


        return isAppInstalled;
    }


    public void SignIn()
    {
        Debug.Log("ArioGameService :  SignIn() called"); 
        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("signIn",
                                    gameObject.name,
                                    "OnConnectedToArioListener" );
        #endif
    }

    public void SignOut()
    {
        Debug.Log("ArioGameService :  SignOut() is called");

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("signOut") ; 
        #endif
    }

    public void isLogin() {
        Debug.Log("ArioGameService :  isLogin() called");
        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("isLoginInArio",
                                    gameObject.name,
                                    "OnLoginStateInArioListener" );
        #endif
    }

    public bool IsConnected()
    {
        bool result = false;


        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            result =  (bool) androidClass.CallStatic<bool>("isConnetedToArio");
        #endif
        Debug.Log("ArioGameService :  IsConnected() is called , Ario is " + ( (result) ? "connected" : " not connected") );

        return result; 
    }


    private void OnLoginStateInArioListener(String response) {
        bool result = (response == "1") ? true : false;
        Debug.Log("ArioGameService : OnLoginStateInArioListener() is called , User in Ario" + ( (result)? "is Login" : "not Login") );
        if(_onLoginStateListener != null)
            _onLoginStateListener(result);
    }
    private void OnConnectedToArioListener(String response )
    {
        bool result = (response == "1") ? true : false;

        Debug.Log("ArioGameService : OnConnectSucceedJavaListener() is called , Ario is" + ( (!result)? "not connected" : "connected") );
        if (_onConnectListener != null )
            _onConnectListener( result); 
    }
    

    public void UnlockAchievement(string achievementID)
    {
        int temp;
        if(!int.TryParse(achievementID, out temp)){
            Debug.LogError("ArioGameService :  Invalid achivement id: " + achievementID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("unlockAchievement", achievementID, SECRET_KEY) ; 
        #endif

        Debug.Log("ArioGameService :  UnlockAchievement() is called");
    }

    public void IncrementAchievement(string achievementID, int incrementNumber)
    {
        int temp;
        if(!int.TryParse(achievementID, out temp)) {
            Debug.LogError("ArioGameService :  Invalid achivementId: " + achievementID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("incrementAchievement", achievementID, incrementNumber, SECRET_KEY) ; 
        #endif
        Debug.Log("ArioGameService :  IncrementAchievement() is called");

    }

    public void ShowAllAchievements()
    {
        int temp;
        if(!(int.TryParse(APP_ID, out temp))) {
            Debug.LogError("ArioGameService :  Invalid APP_ID: " + APP_ID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("showAllAchievements", APP_ID) ; 
        #endif
        Debug.Log("ArioGameService :  ShowAllAchievements() is called");

    }

    public void SubmitScoreToLeaderboard(string leaderboardID, long score)
    {
        int temp;
        if(!(int.TryParse(leaderboardID, out temp) && int.TryParse(APP_ID, out temp))) {
            Debug.LogError("ArioGameService :  Invalid leaderboardID or APP_ID: " + leaderboardID + ", " + APP_ID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("submitScore", leaderboardID, score.ToString(), APP_ID, SECRET_KEY) ; 
        #endif
        Debug.Log("ArioGameService :  SubmitScoreToLeaderboard() is called");
    }

    public void ShowLeaderboard(string leaderboardID)
    {
        int temp;
        if(!(int.TryParse(leaderboardID, out temp) && int.TryParse(APP_ID, out temp))) {
            Debug.LogError("ArioGameService :  Invalid leaderboardID or APP_ID: " + leaderboardID +", "+APP_ID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("showLeaderboard", leaderboardID, APP_ID) ; 
        #endif
        Debug.Log("ArioGameService :  ShowLeaderboard() is  called");
    }

    public void ShowAllLeaderboards()
    {
        int temp;
        if(!int.TryParse(APP_ID, out temp)) {
            Debug.LogError("ArioGameService :  Invalid APP_ID: "+APP_ID);    
            return;
        }
        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("showAllLeaderboards", APP_ID) ; 
        #endif
        Debug.Log("ArioGameService :  ShowAllLeaderboards() is  called");

    }

    public void LoadAchievement(string achievementId)
    {
        Debug.Log("ArioGameService :  LoadAchievement() is called");

        int temp;
        if(!(int.TryParse(achievementId, out temp) && int.TryParse(APP_ID, out temp))) {
            Debug.LogError("ArioGameService :  Invalid achievementId or APP_ID: " + achievementId +", "+APP_ID);    
            return;
        }

        #if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
            androidClass.CallStatic("loadAchievement",
                                    gameObject.name,
                                    "OnGetAchievement",
                                    achievementId,
                                    APP_ID) ; 
        #endif
    }

    private void OnGetAchievement(string achievement) {
        if (_onGetAchievementInfo!=null) {
            _onGetAchievementInfo(achievement);
        }
    }
}
