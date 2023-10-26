using UnityEngine;
using System.Collections;

public class AppsflyerInit : MonoBehaviour {

    public string AppsFlyerKey;
    public string AndroidPackageName;
    public string AppleId;

    void Start( )
    {
        //AppsFlyer.setAppsFlyerKey(AppsFlyerKey);
#if UNITY_IOS
    AppsFlyer.setAppsFlyerKey(AppsFlyerKey);
    AppsFlyer.setAppID ("YOUR_APP_ID_HERE");
        
    // For detailed logging
    //AppsFlyer.setIsDebug (true); 
        
    // For getting the conversion data will be triggered on AppsFlyerTrackerCallbacks.cs file
    AppsFlyer.getConversionData (); 
        
    // For testing validate in app purchase (test against Apple's sandbox environment
    //AppsFlyer.setIsSandbox(true);         
     
    AppsFlyer.trackAppLaunch ();

#elif UNITY_ANDROID
        AppsFlyer.init(AppsFlyerKey);
        AppsFlyer.setAppID(AndroidPackageName);

        //AppsFlyer.setIsDebug(true);
        //AppsFlyer.createValidateInAppListener("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure");
        AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks", "didReceiveConversionData", "didReceiveConversionDataWithError");
        //AppsFlyer.trackAppLaunch( );
#endif
    }


}
