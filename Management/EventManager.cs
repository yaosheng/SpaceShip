using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    public static float playTime = 0;
    public static float continueTime = 0;
    public static int restartTimes = 0;
    public static int openingTimes = 0;
    public static int pauseTime = 0;
    public static float onceTime = 0;
    public static float averageScore = 0;
    public static int scoreSum = 0;
    public static int replayTimes = 0;

    void Awake () {

    }
	
    void Start( )
    {
        onceTime = 0;
        if(PlayerPrefs.HasKey("playTime")) {
            playTime = PlayerPrefs.GetFloat("playTime");
        }
        if(PlayerPrefs.HasKey("scoreSum")) {
            scoreSum = PlayerPrefs.GetInt("scoreSum");
        }
        if(PlayerPrefs.HasKey("replayTimes")) {
            replayTimes = PlayerPrefs.GetInt("replayTimes");
        }
    }

    int GetIntOfPlayerPrefs(string name )
    {
        if(PlayerPrefs.HasKey(name)) {
            return PlayerPrefs.GetInt(name);
        }
        else {
            return 0;
        }
    }

    float GetFloatOfPlayerPrefs( string name )
    {
        if(PlayerPrefs.HasKey(name)) {
            return PlayerPrefs.GetFloat(name);
        }
        else {
            return 0;
        }
    }

    void Update () {
        continueTime = Time.time;
        onceTime += Time.deltaTime;
        //Debug.Log("oncetime : " + onceTime);
        //Debug.Log("continueTime : " + continueTime);
        //Debug.Log("playTime : " + playTime);
        //Debug.Log("opening times : " + openingTimes);
        //Debug.Log("restart times : " + restartTimes);
    }

    public static void SaveGameTime( )
    {
        PlayerPrefs.SetFloat("playTime", continueTime + playTime);
    }

    public static void SaveScoreSum( )
    {
        PlayerPrefs.SetInt("scoreSum", scoreSum);
    }

    public static void SaveRePlayTimes( )
    {
        PlayerPrefs.SetInt("replayTimes", replayTimes);
    }
}
