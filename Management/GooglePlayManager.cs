using UnityEngine;
using System.Collections;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour {

    public string leaderboardID;
    public string achievement1_FirstStep;
    public string achievement2_SpaceBall;
    public string achievement3_Survivor;
    public string achievement4_GoldFarmer;
    public string achievement5_FleetCommander;

    void Awake( )
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate( );
    }

    void Start () {
        ReportScoreToLeaderboard( );
    }

	void Update () {
        if(GM.PlanetManager.stepNumber >= 50) {
            AchievementSurvivor( );
        }
        if(GM.UIManager.coinSum >= 10000) {
            AchievementGoldFarmer( );
        }
    }

    public void ShowLeaderBoardUI( )
    {
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate( );
        //ReportScoreToLeaderboard( );
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
#endif
    }

//    public void ShowAchievementsUI( )
//    {
//#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
//        Social.ShowAchievementsUI( );
//#endif
//    }

    public void ShowGamePage( )
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=atfone.orbit.path.android");
#elif UNITY_IPHONE
			Application.OpenURL("itms-apps://itunes.apple.com/app/id1044954812");
#endif
    }

    public void ReportScoreToLeaderboard( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                PlayGamesPlatform.Instance.ReportScore(GM.GameSetUp.bestScore, leaderboardID, ( bool success1 ) =>
                {
                    if(success1) {
                        Debug.Log("success to post bestScore");
                    }
                    else {
                        Debug.Log("failed to post bestScore");
                    }
                });
            }
        });
    }

    public void AchievementFirstStep( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                Social.ReportProgress(achievement1_FirstStep, 100.0f, ( bool success1 ) => {
                    // handle success or failure
                });
            }
        });
    }

    public void AchievementSpaceBall( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                Social.ReportProgress(achievement2_SpaceBall, 100.0f, ( bool success1 ) => {                    // handle success or failure
                    // handle success or failure
                });
            }
        });
    }
    public void AchievementSurvivor( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                Social.ReportProgress(achievement3_Survivor, 100.0f, ( bool success1 ) => {
                    // handle success or failure
                });
            }
        });
    }
    public void AchievementGoldFarmer( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                Social.ReportProgress(achievement4_GoldFarmer, 100.0f, ( bool success1 ) => {
                    // handle success or failure
                });
            }
        });
    }
    //public void AchievementFleetCommander( )
    //{
    //    Social.localUser.Authenticate(( bool success ) =>
    //    {
    //        if(success) {
    //            Social.ReportProgress(achievement5_FleetCommander, 100.0f, ( bool success1 ) => {
    //                // handle success or failure
    //            });
    //        }
    //    });
    //}
}
