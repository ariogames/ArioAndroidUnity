using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SocialScript : MonoBehaviour {

	void Start () {
		
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

	public void rateOnGame() {
		ArioGameService.Instance.RateOnGame();
	}

	public void postScreenShot() {
		ArioGameService.Instance.ShowScreeenShotPage();
	}

}
