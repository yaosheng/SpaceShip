using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour {

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    public void ReTurnToHome( )
    {
        SceneManager.LoadScene("GameStart");
        Time.timeScale = 1;
        GameSetUp.isOpened = false;
        GM.GameSetUp.gameMode = GameMode.Teaching;

        EventManager.openingTimes++;
        EventManager.replayTimes++;
        EventManager.SaveGameTime( );
        EventManager.SaveRePlayTimes( );
        EventManager.onceTime = 0;

        AdsReportingManager.LogEvent("Press Home Button");
        AdsReportingManager.LogEvent("Opening Time", (int)EventManager.openingTimes);
        AdsReportingManager.LogEvent("Play Time Once", (int)EventManager.onceTime);
        AdsReportingManager.LogEvent("Play Time Continue", (int)EventManager.continueTime);
        AdsReportingManager.LogEvent("Play Time", (int)EventManager.playTime);
    }

    //public IEnumerator IEnumeratorToHome( )
    //{
    //    var async = SceneManager.LoadSceneAsync("GameStart");
    //    GameSetUp.isOpened = false;
    //    //EventManager.isOpened = false;
    //    yield return async;
    //    Time.timeScale = 1;
    //    GM.GameSetUp.gameMode = GameMode.Teaching;
    //    EventManager.openingTime++;
    //    EventManager.SaveGameTime( );
    //    //EventManager.isOpened = true;
    //    AdsReportingManager.LogEvent("Press Home Button", (int)EventManager.gameTime);
    //    AdsReportingManager.LogEvent("Opening Time", (int)EventManager.openingTime);
    //}
}
