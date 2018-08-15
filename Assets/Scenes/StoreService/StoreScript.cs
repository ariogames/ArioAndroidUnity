using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour {

	public Text textCheckUpdate = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

	public void checkUpdate() {
		ArioGameService.Instance.CheckUpdate();
		ArioGameService.Instance.onCheckUpdate = OnCheckUpdateResult;
	}

	private void OnCheckUpdateResult(int version) {
		if (version == -2) {
			textCheckUpdate.text = "invalid result";
		} else if (version == -1) {
			textCheckUpdate.text = "game is on last version (currently up to date)";
		} else {
			textCheckUpdate.text = "new version available. version_code: " + version;
		}
	}

	public void showGamePage() {
		ArioGameService.Instance.ShowGamePage();
	}

	public void showDeveloperPage() {
		ArioGameService.Instance.ShowDeveloperPage("12"); //you can see your developerId in developer panel at http://developers.ariogames.ir/profile
	}
}
	