using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AuthenticationScript : MonoBehaviour {

	public Text message = null;
	private int DEFAULT_FONT_SIZE = 24;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        ArioGameService.Instance.SignOut();
        message.text = "Sign out called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void IsConnected()
    {
        message.text = (ArioGameService.Instance.IsConnected()) ? " Ario is connected " : " Ario is not connected";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    public void IsLogin() 
    {
        ArioGameService.Instance.OnLoginStateListener = OnLoginListener;
        ArioGameService.Instance.isLogin();
        message.text = "isLogin called";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    private void OnConnectListener(bool isConnected)
    {
        message.text = isConnected ? " Ario SignIn successfully" : " Ario SignIn failed";
        message.fontSize = DEFAULT_FONT_SIZE;
    }

    private void OnLoginListener(bool isLogin)
    {
        message.text = isLogin ? " User in Ario is Login" : "User in Ario is not Logged In";
        message.fontSize = DEFAULT_FONT_SIZE;
    }
}
