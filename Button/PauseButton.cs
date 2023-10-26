using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }
    public void Pause( )
    {
        if(Time.timeScale == 1) {
            GM.GameSetUp.gameMode = GameMode.Pause;
            GM.UIManager.continueButton.gameObject.SetActive(true);
            GM.UIManager.restartButton.gameObject.SetActive(false);
            EventManager.pauseTime++;
            AdsReportingManager.LogEvent("Press Pause Button", (int )EventManager.pauseTime);
            Time.timeScale = 0;
        }
    }
}
