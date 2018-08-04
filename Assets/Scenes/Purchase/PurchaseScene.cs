using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//
// Sample scene which uses Ario in-app purchase sdk
// please note that it's important to implement callbacks 
// and register them before calling ArioGameService.Instance methods
public class PurchaseScene : MonoBehaviour
{
    public Text message = null;

    public InputField input = null; 

    void Start()
    {
        ArioInAppPurchase.Instance.SetSKUList(new List<string>()  { "TEST_SKU", "YOUR-SKU-1","YOUR-SKU-2" });
        
        // Registering callback methods
        ArioInAppPurchase.Instance.OnGetProductsInfo = this.OnGetProductInfo;
        ArioInAppPurchase.Instance.OnConsumeFailed = this.OnConsumeFailed;
        ArioInAppPurchase.Instance.OnConsumeSucceed = this.OnConsumeSucceed;
        ArioInAppPurchase.Instance.OnPurchaseFailed = this.OnPurchaseFailed;
        ArioInAppPurchase.Instance.OnPurchseSucceed = this.OnPurchasedSucced;
    }


    public void OnPurchaseFailed(string error )
    {
        message.text = " Purachse failed \n" + error; 
    }

    public void OnPurchasedSucced(string sku , string token )
    {
        message.text = " Purchasing item with sku " + sku + " succeed with token " + token; 
    }

    public void OnConsumeFailed(string error)
    {
        message.text = " Consume failed \n" + error; 
    }

    public void OnConsumeSucceed(string sku, string token)
    { 
        message.text = " Consuming item with SKU " + sku + " succeeded with token " + token;
    }

    public void OnPurchaseClick()
    {
        if (input != null)
        {
            if(input.text.Length == 0)
            {
                message.text = " Please enter SKU";
            }
            ArioInAppPurchase.Instance.Purchase(input.text);
            Debug.Log(" The entered SKU is : " + input.text); 
        } else {
            Debug.Log(" Input is null"); 
        }
    }

    public void OnConsumeClick()
    {
        ArioInAppPurchase.Instance.Consume(); 
    }

    public void OnGetProductInfo(string info )
    {
        Debug.Log("Unity Ario -> The returned info is: " + info); 
    }

    public void OnInitializeArio()
    {
        ArioInAppPurchase.Instance.QueryInventory();
    }

    public void GetPackageName()
    {
        message.text = ArioInAppPurchase.Instance.GetGamePackage(); 
    }

    public void IsStoreInstalled()
    {
        if (ArioInAppPurchase.Instance.IsStorePackageInstalled())
            message.text = " The Store is Installed ";
        else
            message.text = " The Store is Not Installed"; 
    }

    public void onBackPressed() {
        SceneManager.LoadScene("MainMenu");
    }
}
