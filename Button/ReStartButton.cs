using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReStartButton : MonoBehaviour {

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("GameStart");
        Time.timeScale = 1;
        GameSetUp.isOpened = true;
        GM.GameSetUp.gameMode = GameMode.Playing;

        EventManager.restartTimes++;
        EventManager.replayTimes++;
        EventManager.SaveGameTime( );
        EventManager.SaveRePlayTimes( );
        EventManager.onceTime = 0;

        AdsReportingManager.LogEvent("Press Restart Button");
        AdsReportingManager.LogEvent("Restart Times", (int)EventManager.restartTimes);
        AdsReportingManager.LogEvent("Play Time Once", (int)EventManager.onceTime);
        AdsReportingManager.LogEvent("Play Time Continue", (int)EventManager.continueTime);
        AdsReportingManager.LogEvent("Play Time", (int)EventManager.playTime);
    }

    //public IEnumerator IEnumeratorReStart( )
    //{
    //    var async = SceneManager.LoadSceneAsync("GameStart");
    //    GameSetUp.isOpened = true;

    //    Time.timeScale = 1;
    //    GM.GameSetUp.gameMode = GameMode.Playing;

    //    Debug.Log("restartbutton");
    //    EventManager.restartTime++;

    //    AdsReportingManager.LogEvent("Press Restart Button", (int)EventManager.gameTime);
    //    AdsReportingManager.LogEvent("Restart Time", (int)EventManager.restartTime);
    //    yield return async;
    //    EventManager.isOpened = true;
    //}
}
