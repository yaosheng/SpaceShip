using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }
    public void ClickStartButton( )
    {
        Debug.Log("start button");
        GM.GameSetUp.gameMode = GameMode.Teaching;
        //GM.UIManager.teachUI.SetActive(true);
        GM.PlanetManager.ActiveStartPlanet( );
        AdsReportingManager.LogEvent("Press Start Button");
    }
}
