using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ; 

public class MainMenuScene : MonoBehaviour 
{
		
	void Start()
	{
		ArioGameService.Instance.init(
			/*ENTER_YOUR_ARIO_APP_ID=*/ "772", 
			/*ENTER_YOUR_ARIO_APP_SECRET_KEY=*/ "644d2aa9-9cb8-4157-b1e2-5b0a064729e3"
		);

		ArioInAppPurchase.Instance.init( /* 
		ENTER_YOUR_ARIO_APP_RSA_KEY */
			"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDjXdFlwmrJDowfI7NjXtD0/U1uG7i567S9QgrhZDJRS52+sSDc7IcVFrTCIzl/QeE8yKaJjQdr9ZjmCD22ZYBUkSwkOrbtAqMnmPk13AOy9A+95q1PI0SnZeGPLejFLQNxPUEaSbKMKCwqeqTCCvh4toMby87ZtlvlVHs3KDS1+QIDAQAB");
	}
	
	public void GoToPurchase()
	{
			SceneManager.LoadScene("Purchase") ; 
	}

	public void GoToGameService()
	{
		SceneManager.LoadScene("Services") ; 
	}
}
