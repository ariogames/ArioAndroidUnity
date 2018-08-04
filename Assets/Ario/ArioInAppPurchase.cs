using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ArioInAppPurchase : MonoBehaviour {
	#if !UNITY_EDITOR && UNITY_ANDROID
    private AndroidJavaClass androidClass;
    #endif
	
    private string PUBLIC_KEY = "";
	
	static private ArioInAppPurchase _instance = null;
	static public ArioInAppPurchase Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ArioInAppPurchaseObject");
                obj.name = "ArioInAppPurchaseObject";
                _instance = obj.AddComponent<ArioInAppPurchase>();
            }

            return _instance;
        }
    }

    public void init(String rsa_key)
    {
        PUBLIC_KEY = rsa_key;
    }
    
    private string getKey()
    {
        return PUBLIC_KEY; 
    }

        void Awake()
    {
        DontDestroyOnLoad(gameObject);
#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJNI.AttachCurrentThread();
        androidClass = new AndroidJavaClass("com.arioclub.unity.sdk.android.ArioInAppPurchaseInterface");         
#endif
    }

    private Action<string> _onConsumeFailed = null; // argument : the result error 
    public System.Action<string> OnConsumeFailed
    {
        get { return _onConsumeFailed; }
        set { _onConsumeFailed = value; }
    }

    private Action<string, string> _onConsumeSucceed = null; // 1st argument : consumed sku      2nd argumnet : consume token  
    public System.Action<string, string> OnConsumeSucceed
    {
        get { return _onConsumeSucceed; }
        set { _onConsumeSucceed = value; }
    }

    private Action<string> _onPurchaseFailed = null; // argument : the result error
    public System.Action<string> OnPurchaseFailed
    {
        get { return _onPurchaseFailed; }
        set { _onPurchaseFailed = value; }
    }

    private Action<string, string> _onPurchseSucceed = null; // 1st argument : consumed sku      2nd argumnet : consume token  
    public System.Action<string, string> OnPurchseSucceed
    {
        get { return _onPurchseSucceed; }
        set { _onPurchseSucceed = value; }
    }

    private Action<string> onGetProductsInfo = null;
    public System.Action<string> OnGetProductsInfo
    {
        get { return onGetProductsInfo; }
        set { onGetProductsInfo = value; }
    }

    private List<string> SKUList = new List<string>();  // your sku list that should be added here

    public void SetSKUList(List<string> list)
    {
        SKUList.Clear();

        for (int i = 0; i < list.Count; i++)
            SKUList.Add(list[i]);
    }

    private string lastSku = null;

    public void PurchaseSucceed(string result)
    {
        Debug.Log("Ario: we are back in unity in function : PurchaseSucceed . with result : " + result );

        int t = result.IndexOf(",");

        string sku = result.Substring(0, t);
        string token = result.Substring(t + 1, result.Length - t - 1);

        for (int i = 0; i < SKUList.Count; i++)
        {
            if (sku.Equals(SKUList[i]))
            {
                if (_onPurchseSucceed != null)
                    _onPurchseSucceed(sku, token);

                lastSku = sku;
                Debug.Log("Ario: Purchase was successfull for sku " + lastSku );


                return;
            }
        }
    }

    public void PurchaseFailed(string result)
    {
        Debug.Log("Ario: Purchase failed with result " + result);

        if (_onPurchaseFailed != null)
            _onPurchaseFailed(result);
    }

    public void ConsumeSucceed(string result)
    {
        int t = result.IndexOf(",");

        string sku = result.Substring(0, t);
        string token = result.Substring(t + 1, result.Length - t - 1);

        Debug.Log("Ario: Consume was successfull for sku " + sku);

        if (_onConsumeSucceed != null)
            _onConsumeSucceed(sku, token);

        lastSku = null;
    }

    public void ConsumeFailed(string result)
    {
        Debug.Log("Ario: Consume Failed with result " + result);

        if (_onConsumeFailed != null)
            _onConsumeFailed(result);
    }

    public string GetSKUListString()
    {
        string skuString = "";

        for (int i = 0; i < SKUList.Count; i++)
        {
            skuString += SKUList[i] + ",";
        }

        return skuString;
    }

    public void GetProductsInfo(string info)
    {
        Debug.Log("Ario: GetProductsInfo is called with result :  " + info);


        if (onGetProductsInfo != null)
            onGetProductsInfo(info);
    }

    public bool Rate( )
    {
        bool canRate = false;
        string packageName = GetGamePackage(); // you could write any package name for test 

        Debug.Log("Ario: Rate function is called");

#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR

        canRate = androidClass .CallStatic<bool>( "Rate" , packageName  ); 
#endif

        return canRate; 
    }

    private void ConsumePurchase()
    {
        if (lastSku == null)
        {
            OnConsumeFailed("Ario: There is no purchase to consume");
            return;
        }
#if ( UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR

        androidClass .CallStatic(   "OpenActivityForPurchase",
                                    gameObject.name,
                                    "PurchaseSucceed" , 
                                    "PurchaseFailed" ,
                                    "ConsumeSucceed",
                                    "ConsumeFailed" , 
                                    lastSku,
                                    getKey(),
                                    true);
#endif
    }


    public void DoQueryInventory()
    {

#if ( UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR

        androidClass .CallStatic(   "QueryInventoryInfo",
                                    gameObject.name,
                                    "PurchaseSucceed" ,
                                    "GetProductsInfo",
                                    GetSKUListString(),
                                    getKey()                           
                                );
#endif
    }

    private void DoPurchase(string sku)
    {
        lastSku = sku;
        Debug.Log("Ario: SKU for purchase is  " + sku);

#if ( UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR

        androidClass.CallStatic(    "OpenActivityForPurchase",
                                    gameObject.name,
                                    "PurchaseSucceed" , 
                                    "PurchaseFailed" ,
                                    "ConsumeSucceed",
                                    "ConsumeFailed" , 
                                    lastSku,
                                    getKey(),
                                    false);
#endif
    }

    public bool IsSKUValid(string sku)
    {
        if (SKUList.Count == 0)
        {
            Debug.Log("SKU list is empty. Please add desired SKU List by calling --> SetSKUList(List<String> urSkuList).");
            return false;
        }

        for (int i = 0; i < SKUList.Count; i++)
        {
            if (sku == SKUList[i])
                return true;
        }

        return false;

    }

    // return false if sku does not exist in sku list 
    public bool Purchase(string sku)
    {
        if (IsSKUValid(sku))
        {
            DoPurchase(sku);
            return true;
        }

        return false;
    }

    public void Consume()
    {
        ConsumePurchase();
    }


    public bool QueryInventory()
    {
        if (SKUList.Count == 0)
        {
            Debug.Log("SKU list is empty. Please add desired SKU List by calling --> SetSKUList(List<String> urSkuList).");
            return false;
        }

        DoQueryInventory();
        return true;
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

    public string GetGamePackage()
    {
        string packageName = "";

        Debug.Log("Ario : GetGamePackage is called  ");

#if ( UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR

        packageName = (string)androidClass.CallStatic<string>( "GetPackageNameFromJava" );
#endif

        if (packageName != "")
            Debug.Log("Ario : Package name is : " + packageName);
        else
            Debug.Log("Ario : Store does not exist");


        return packageName;
    }
}
