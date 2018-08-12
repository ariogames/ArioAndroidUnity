using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ario.Models;

public class AchievementScript : MonoBehaviour {

	public InputField inputAchievementIdUnlock;
	public InputField inputAchievementIdIncreament;
	public InputField inputAchievementNumStep;
	public Text textAchievementOne;
	public Text textAchievementTwo;
	public Text textAchievementThree;

	// Use this for initialization
	void Start () {
		
	}

    void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			SceneManager.LoadScene(0);
	}
	
	 public void UnlockAchievement()
    {
        // you can use 60, 61, 62 as achievement id for test, which is TEST_ACHIEVEMENT in Ario Run&Jump sample game
        // keep in mind that achievement is given to user if user has installed your app from Ario, 
        // otherwise this has no effect;

        if (inputAchievementIdUnlock != null && inputAchievementIdUnlock.text != null 	&& inputAchievementIdUnlock.text.Length > 0)
        {
            ArioGameService.Instance.UnlockAchievement(inputAchievementIdUnlock.text);
        }

    }

	
    public void IncrementAchievement()
    {   // you can use 60, 61, 62 as achievement id for test, which is TEST_ACHIEVEMENT in Ario Run&Jump sample game
		// each achievement has 10 step to unlock and you can increment these steps by this function.
        // keep in mind that achievement is given to user if user has installed your app from Ario, 
        // otherwise this has no effect; 
        if (inputAchievementIdIncreament != null && inputAchievementIdIncreament.text != null && inputAchievementIdIncreament.text.Length > 0
	        && inputAchievementNumStep != null && inputAchievementNumStep.text != null && inputAchievementNumStep.text.Length > 0)
        {
            ArioGameService.Instance.IncrementAchievement(inputAchievementIdIncreament.text, Int32.Parse(inputAchievementNumStep.text));
        }
    }

	public void ShowAllAchievements()
    {
        ArioGameService.Instance.ShowAllAchievements();
    }

    public void GetAchievementsInfo()
    {
        ArioGameService.Instance.LoadAllAchievement();
        ArioGameService.Instance.OnGetAchievementInfo = OnGetAchievementInfo;
    }

    private void OnGetAchievementInfo(AchievementList achievementList)
    {
        Debug.LogError("AriogameService achievement List size: " + achievementList.list.Length);
        if (achievementList.list.Length > 0) {
            AchievementList.Achievement achievementOne = achievementList.list[0];
            textAchievementOne.text = "\nname: "+achievementOne.name+"\ndescription: "+achievementOne.description+"\ntotalStep: "+achievementOne.totalStep.ToString()+"\ncurrentStep: "+achievementOne.currentStep.ToString()+"\nisUnlock: " + (achievementOne.isUnlock);
            AchievementList.Achievement achievementTwo = achievementList.list[1];
            textAchievementTwo.text = "\nname: "+achievementTwo.name+"\ndescription: "+achievementTwo.description+"\ntotalStep: "+achievementTwo.totalStep.ToString()+"\ncurrentStep: "+achievementTwo.currentStep.ToString()+"\nisUnlock: " + (achievementTwo.isUnlock);
            AchievementList.Achievement achievementThree = achievementList.list[2];
            textAchievementThree.text = "\nname: "+achievementThree.name+"\ndescription: "+achievementThree.description+"\ntotalStep: "+achievementThree.totalStep.ToString()+"\ncurrentStep: "+achievementThree.currentStep.ToString()+"\nisUnlock: " + (achievementThree.isUnlock);
        }
    }
}
