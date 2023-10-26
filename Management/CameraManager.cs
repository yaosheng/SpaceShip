using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

    public Camera mainCamera;
    public GameObject[ ] starBackGround;
    public SpriteRenderer[ ] spaceBackGround;
    private const float movecameraTime = 1.75f;
    private const float movecameraSlowly = 25.0f;
    private float colorH;
    private float colorS;
    private float colorV;
    private Color originalColor;
    private int pingpongColorH = -1;
    //private int pingpongColorS = -1;
    private int pingpongColorV = -1;
    private float timer;
    private bool isDeaded = false;
    //public bool islockedcameraMoving = false;
    private AdsReportingManager AdsReportingManager;

    void Awake () {
        mainCamera = GetComponent<Camera>( );
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }
	
    void Start( )
    {
        FindOriginalBackGroundColor( );
    }

    void FindOriginalBackGroundColor( )
    {
        originalColor = spaceBackGround[0].color;
        Color.RGBToHSV(originalColor, out colorH, out colorS, out colorV);
        Debug.Log("colorH : " + colorH + " , " + colorS + " , " + colorV);
    }

	void Update () {
        OutOfCamera( );
        CheckStarPosition( );
        ShowBackGroundColor( );
        //TrunBackColor( );
        //Debug.Log("colorH : " + colorH + " , " + colorS + " , " + colorV);
    }

    void ShowBackGroundColor( )
    {
        for(int i = 0; i < spaceBackGround.Length; i++) {
            spaceBackGround[i].color = Color.HSVToRGB(colorH, colorS, colorV);
        }
    }

    void CheckStarPosition( )
    {
        for(int i = 0; i < starBackGround.Length; i++) {
            if(starBackGround[i].transform.localPosition.y <= -18) {
                starBackGround[i].transform.localPosition = new Vector3(0, 18, 0);
            }
        }
    }
    
    void TrunBackColor( )
    {
        if(colorH <= 0.48f) {
            pingpongColorH = 1;
        }
        if(colorH >= 0.64f) {
            pingpongColorH = -1;
        }
        if(colorV <= 0.15f) {
            pingpongColorV = 1;
        }
        if(colorV >= 0.9f) {
            pingpongColorV = -1;
        }
        //Debug.Log("colorH : " + colorH + " , " + colorS + " , " + colorV);
    }

    void ChangeBackGroundColor(float distance, float fixedTime )
    {
        TrunBackColor( );
        for(int i = 0; i < spaceBackGround.Length; i++) {
            //Color tempColor = spaceBackGround[i].color;
            iTween.ValueTo(spaceBackGround[i].gameObject, iTween.Hash("from", colorH, "to", colorH + (pingpongColorH * distance * 0.001f), 
                            "time", fixedTime * movecameraTime, "easeType", "easeOutSine", "onupdatetarget", this.gameObject, "onupdate", "OnColorUpdated1"));
            iTween.ValueTo(spaceBackGround[i].gameObject, iTween.Hash("from", colorV, "to", colorV + (pingpongColorV * distance * 0.01f),
                            "time", fixedTime * movecameraTime, "easeType", "easeOutSine", "onupdatetarget", this.gameObject, "onupdate", "OnColorUpdated2"));
        }
    }

    private void OnColorUpdated1( float tempColor )
    {
        colorH = tempColor;
    }

    private void OnColorUpdated2(float tempColor )
    {
        colorV = tempColor;
    }

    public void MoveStar( float distance, float fixedTime )
    {
        for(int i = 0; i < starBackGround.Length; i++) {
            if(i <= 1) {
                iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 5), "islocal", true, "time", fixedTime * movecameraTime, "easeType", "easeOutSine"));
            }
            else {
                iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 10), "islocal", true, "time", fixedTime * movecameraTime, "easeType", "easeOutSine"));
            }
        }
    }

    public void MoveCamera( bool islocked, Planet currentP, Planet targetP, Planet nextP )
    //public IEnumerator MoveCamera( bool islocked, Planet currentP, Planet targetP, Planet nextP )
    {
        //yield return new WaitForSeconds(0.25f);
        Vector3 downPosition = mainCamera.ScreenToWorldPoint(Vector3.zero);
        float distance = 0;
        float unitDistance = Vector3.Distance(currentP.transform.position, nextP.transform.position);
        float distanceScale = Vector3.Distance(currentP.transform.position, targetP.transform.position) / unitDistance;
        float fixedY = 2.5f;
        if(islocked) {
            Debug.Log("next Planet or over");
            distance = targetP.transform.position.y - (downPosition.y + targetP.pathHeight + fixedY);
            iTween.MoveTo(mainCamera.gameObject, iTween.Hash("y", mainCamera.transform.position.y + distance, "time", distanceScale * movecameraTime, "easeType", "easeOutSine"));
            MoveStar(distance, distanceScale);
            //StartCoroutine(ChangeBackGroundColor(distance, distanceScale));
            ChangeBackGroundColor(distance, distanceScale);
        }
        else {
            Debug.Log("no match");
            distance = nextP.transform.position.y - (downPosition.y + targetP.pathHeight + fixedY);
            iTween.MoveTo(mainCamera.gameObject, iTween.Hash("y", mainCamera.transform.position.y + distance, "time", movecameraSlowly * movecameraTime, "easeType", "easeOutSine"));
            MoveStar(distance, movecameraSlowly);

        }
    }

    public void OutOfCamera( SpriteRenderer sr, Queue<SpriteRenderer> pool )
    {
        float distance = mainCamera.transform.position.y - sr.transform.position.y;
        if(distance >= 10 && sr.transform.position.y < SpaceShip.spaceShip.transform.position.y) {
            sr.transform.gameObject.SetActive(false);
            pool.Enqueue(sr);
        }
    }
    void OutOfCamera( )
    {
        if(timer > 3.0f) {
            if(!SpaceShip.spaceShip.isVisible || SpaceShip.spaceShip.transform.position.x < -5.25f || SpaceShip.spaceShip.transform.position.x > 5.25f) {
                StartCoroutine(LateToGameOver( ));
            }
        }
        else {
            timer += Time.deltaTime;
        }
    }

    public IEnumerator LateToGameOver( )
    {
        SpaceShip.spaceShip.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        GM.GameSetUp.gameMode = GameMode.GameOver;
        if(!isDeaded) {
            Debug.Log("stepNumber : " + GM.PlanetManager.stepNumber);
            EventManager.scoreSum += GM.PlanetManager.stepNumber;
            EventManager.SaveScoreSum( );
            Debug.Log("EventManager.scoreSum : " + EventManager.scoreSum);
            Debug.Log("EventManager.replayTimes : " + EventManager.replayTimes);
            if(EventManager.replayTimes > 0) {
                Debug.Log("Average Score : " + EventManager.scoreSum / EventManager.replayTimes);
                AdsReportingManager.LogEvent("Average Score", (int)(EventManager.scoreSum / EventManager.replayTimes));
            }
            isDeaded = true;
        }
    }

    //public void CameraFallowSpaceShip(float distance)
    //{
    //    //iTween.MoveTo(mainCamera.gameObject, iTween.Hash("y", SpaceShip.spaceShip.transform.position.y + distance, "time", 0.2f, "easeType", "easeOutSine"));
    //    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
    //                                                SpaceShip.spaceShip.transform.position.y + distance, 0);
    //}

    //IEnumerator ChangeBackGroundColor(float distance, float fixedTime )
    //{
    //    for(int i = 0; i < spaceBackGround.Length; i++) {
    //        Hashtable tweenParams = new Hashtable( );
    //        tweenParams.Add("from", spaceBackGround[i].color);
    //        tweenParams.Add("to", targetColorH);
    //        tweenParams.Add("time", fixedTime * movecameraTime);
    //        tweenParams.Add("onupdate", "OnColorUpdated");
    //        tweenParams.Add("easeType", "easeOutSine");
    //        iTween.ValueTo(spaceBackGround[i].gameObject, tweenParams);
    //    }
    //    //for(int i = 0; i < spaceBackGround.Length; i++) {
    //    //    //Debug.Log(spaceBackGround[i].color.a - (distance * 0.04f));
    //    //    iTween.ColorTo(spaceBackGround[i].gameObject, iTween.Hash(, Color.HSVToRGB(1 - (distance * 0.01f), 1, 1 - (distance * 0.01f)), "time", fixedTime * movecameraTime, "easeType", "easeOutSine"));
    //    //}
    //    yield return new WaitForSeconds(fixedTime * movecameraTime);
    //    //for(int i = 0; i < spaceBackGround.Length; i++) {
    //    //    spaceBackGround[i].color = new Color(spaceBackGround[i].color.r, spaceBackGround[i].color.g - (distance * 0.02f), spaceBackGround[i].color.b, spaceBackGround[i].color.a);
    //    //}
    //}

    //  for(int i = 0; i<starBackGround.Length; i++) {
    //      if(i <= 1) {
    //          iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 5), "islocal", true, "time", distanceScale* movecameraTime, "easeType", "easeOutSine"));
    //      }
    //      else {
    //          iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 10), "islocal", true, "time", distanceScale* movecameraTime, "easeType", "easeOutSine"));
    //      }
    //   }

    //for(int i = 0; i < starBackGround.Length; i++) {
    //    if(i <= 1) {
    //        iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 5), "islocal", true, "time", movecameraSlowly * movecameraTime, "easeType", "easeOutSine"));
    //    }
    //    else {
    //        iTween.MoveTo(starBackGround[i], iTween.Hash("y", starBackGround[i].transform.localPosition.y - (distance / 10), "islocal", true, "time", movecameraSlowly * movecameraTime, "easeType", "easeOutSine"));
    //    }
    //}
    //CheckStarPosition( );
    //yield return new WaitForSeconds(movecameraSlowly * movecameraTime);
}
