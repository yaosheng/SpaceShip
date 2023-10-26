using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MotionMode
{
    Idle,
    Drift,
    Turn,
    ReadyToTurn,
    ReadyToDetach,
    TeachShoot,
    //TeachPlay
}

public enum GameMode
{
    Opening,
    Playing,
    Teaching,
    GameOver,
    Pause
}

public class GameSetUp : MonoBehaviour {

    public MotionMode motiomMode = MotionMode.Drift;
    public GameMode gameMode;
    public static int shipNumber
    {
        get {
            int temp = 0;
            if(PlayerPrefs.HasKey("shipNumber")) {
                temp = PlayerPrefs.GetInt("shipNumber");
            }
            else {
                temp = 0;
            }
            return temp;
        }
    }
    public Sprite[ ] shipSprites;
    public static bool isTutorial_1_Finished
    {
        get {
            bool temp = false;
            if(PlayerPrefs.HasKey("isTutorial_1_Finished")){
                if(PlayerPrefs.GetInt("isTutorial_1_Finished") == 1) {
                    temp = true;
                }
                else {
                    temp = false;
                }
            }
            else {
                temp = false;
            }
            return temp;
        }
    }
    public static bool isTutorial_2_Finished
    {
        get {
            bool temp = false;
            if(PlayerPrefs.HasKey("isTutorial_2_Finished")) {
                if(PlayerPrefs.GetInt("isTutorial_2_Finished") == 1) {
                    temp = true;
                }
                else {
                    temp = false;
                }
            }
            else {
                temp = false;
            }
            return temp;
        }
    }

    public int bestScore
    {
        get {
            int savebestScore = 0;
            if(PlayerPrefs.HasKey("bestScore")) {
                savebestScore = PlayerPrefs.GetInt("bestScore");
            }
            else {
                savebestScore = 0;
            }

            return savebestScore;
        }
    }
    public static int coinAmount
    {
        get {
            int cn = 0;
            if(PlayerPrefs.HasKey("coinAmount")) {
                cn = PlayerPrefs.GetInt("coinAmount");
            }
            else {
                cn = 0;
            }
            return cn;
        }
    }
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
    //每種星球大小出現比例(6)
    public int[ ][ ] stepLevel = new int[ ][ ]
    {
        new int[]{ 0, 10, 15, 20, 55 },
        new int[]{ 10, 15 ,20 ,25, 40 },
        new int[]{ 15, 20, 20, 20, 25 },
        new int[]{ 20, 20, 20, 20, 20 },
        new int[]{ 30, 25, 20, 15, 10 },
        new int[]{ 35, 30, 25, 10, 0 }
    };
    //衛星出現頻率及數量(4)
    public int[ ][ ] satelliteLevel = new int[ ][ ]
    {
        new int[] {40, 60, 0, 0},
        new int[] {20, 40, 30, 10},
        new int[] {10, 30, 30, 30},
        new int[] {0, 20, 40, 40}
    };
    //隕石數量(5)
    public int[ ][ ] aeroliteLevel = new int[ ][ ]
    {
        new int[] { 9, 15 },
        new int[] { 9, 18 },
        new int[] { 12, 21 },
        new int[] { 12, 24 },
        new int[] { 15, 30 },
    };
    //第一次出現新的障礙的關卡點
    public int[] createProportion = new int[]{ 10, 20, 30, 40, 50};
    public static bool isOpened = false;
    public int practicePlanet = 2;
    private AdsReportingManager AdsReportingManager;
    public bool isTutorial = false;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    void Start( )
    {
        //PlayerPrefs.DeleteAll( );
        if(!isOpened) {
            gameMode = GameMode.Opening;
        }
        GM.PlanetManager.SetFirstPlanet( );
        GM.PlanetManager.SetSecondPlanet( );
        GM.PlanetManager.PrepareToStartGame(practicePlanet);
        SpaceShip.spaceShip.sprite = shipSprites[shipNumber];
    }

    void Update( )
    {
        //Debug.Log("isOpened : " + isOpened);
        if(Input.GetKeyDown(KeyCode.Escape)) {
            AdsReportingManager.LogEvent("Press Back To Quit Game");
            EventManager.SaveGameTime( );
            Application.Quit( );
        }
        //string tempString = StackTraceUtility.ExtractStackTrace( );
        //Debug.Log("stacktrace : " + tempString);
    }
}
