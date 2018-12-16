using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour {

	public InputField saveInput = null;
	public Text loadText = null;


	// Use this for initialization
	void Start () {
		
	}

	void Update()

	{
		if (Input.GetKeyDown (KeyCode.Escape))
			SceneManager.LoadScene(0);
	}
	
	public void saveGame() {
		ArioGameService.Instance.SaveGame(saveInput.text);
	}

	public void loadGame() {
		ArioGameService.Instance.LoadGame();
		ArioGameService.Instance.OnLoadSnapshot = onGameLoaded;
	}

	public void deleteSaveGame() {
		ArioGameService.Instance.DeleteSaveGame();
		loadText.text = "save was deleted!";
		loadText.color = Color.yellow;
	}

	public void onGameLoaded(string data) {
		if (data.Equals("")) {
			loadText.text = "There is no save data";
			loadText.color = Color.red;
		} else {
			loadText.text = data;
			loadText.color = Color.green;
		}
	}
	
}
