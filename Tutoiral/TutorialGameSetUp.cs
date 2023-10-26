using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialGameSetUp : MonoBehaviour {

    public MotionMode motiomMode = MotionMode.Drift;
    public GameMode gameMode;
    public bool isSoundOn
    {
        get {
            bool issoundon = true;
            if(PlayerPrefs.HasKey("SoundControl")) {
                if(PlayerPrefs.GetInt("SoundControl") == 0) {
                    issoundon = false;
                }
                else {
                    issoundon = true;
                }
            }
            else {
                issoundon = true;
            }
            return issoundon;
        }
    }
    //第一次出現新的障礙的關卡點
    public int[] createProportion = new int[]{ 1, 2, 3, 4, 5};
    public int practicePlanet = 2;
    private AdsReportingManager AdsReportingManager;
    public bool isTutorial = false;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    void Update( )
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            AdsReportingManager.LogEvent("Press Back To Quit Game");
            EventManager.SaveGameTime( );
            Application.Quit( );
        }
    }
}
