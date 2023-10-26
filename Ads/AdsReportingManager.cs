using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

// Include Facebook namespace
using Facebook.Unity;

[DisallowMultipleComponent]
public class AdsReportingManager : MonoBehaviour {

    public GoogleAnalyticsV4 googleAnalytics;

    public GoogleAnalyticsV4 googlePrefab;

    private static AdsReportingManager _instance;

    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }

        if (googleAnalytics == null)
        {
            googleAnalytics = FindObjectOfType<GoogleAnalyticsV4>();
            if (googleAnalytics == null)
            {
                googleAnalytics = Instantiate<GoogleAnalyticsV4>(googlePrefab);
                if (googleAnalytics == null)
                {
                    Debug.LogError("Cannot load google analytics component!");
                }
            }
        }
    
        DontDestroyOnLoad(gameObject);
    }

    public void LogEvent(string action, int? value = null)
    {
        // Google Analytics

        //EventHitBuilder builder = new EventHitBuilder( );
        //builder.SetEventCategory("action");
        //builder.SetEventAction(action);
        //if(value != null) {
        //    builder.SetEventValue(value.Value);
        //}
        //googleAnalytics.LogEvent(builder);

        //Facebook

        if(FB.IsInitialized) {
            if(value != null) {
                FB.LogAppEvent("event1", 0);
                FB.LogAppEvent("event3", 0);
                FB.LogAppEvent(action, value.Value);
            }
            else {
                FB.LogAppEvent(action);
            }
        }
        else {
            Debug.LogError("Facebook component not initialized yet!");
        }
        
        //Appsflyer
        Dictionary<string, string> eventValue = new Dictionary<string, string>( );
        eventValue.Add(action, value.ToString( ));
        AppsFlyer.trackRichEvent(action, eventValue);

        Dictionary<string, string> eventValue2 = new Dictionary<string, string>( );
        eventValue2.Add("event1_1", "1");
        AppsFlyer.trackRichEvent("event1", eventValue2);

        Dictionary<string, string> eventValue3 = new Dictionary<string, string>( );
        eventValue3.Add("event2_1", "1");
        AppsFlyer.trackRichEvent("event2", eventValue3);
    }

    // Use this for initialization
    void Start ()
    {
        googleAnalytics.StartSession();
    }

    void OnLevelWasLoaded(int level)
    {
#if UNITY_ANDROID
        AndroidJNIHelper.debug = false;
#endif
        if (_instance != this) return;
        if (!FB.IsInitialized) return;
        //Scene scene = SceneManager.GetSceneAt(level);
        //googleAnalytics.LogScreen(scene.name);
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            LogEvent("Game Started");
            //EventManager.openingTime++;
            //GM.GameSetUp.AdsReportingManager.LogEvent("OpeningTime", EventManager.openingTime);
            //GM.ADRManager.LogEvent("OpeningTime", EventManager.openingTime);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
}
