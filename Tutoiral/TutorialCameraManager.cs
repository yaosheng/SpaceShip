using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialCameraManager : MonoBehaviour {

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
    private int pingpongColorV = -1;
    private float timer;

    void Awake () {
        mainCamera = GetComponent<Camera>( );
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
        CheckStarPosition( );
        ShowBackGroundColor( );
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
    }

    void ChangeBackGroundColor(float distance, float fixedTime )
    {
        TrunBackColor( );
        for(int i = 0; i < spaceBackGround.Length; i++) {
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

    public void MoveCamera( bool islocked, TutorialPlanet currentP, TutorialPlanet targetP, TutorialPlanet nextP )
    {
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
            ChangeBackGroundColor(distance, distanceScale);
        }
        else {
            Debug.Log("no match");
            distance = nextP.transform.position.y - (downPosition.y + targetP.pathHeight + fixedY);
            iTween.MoveTo(mainCamera.gameObject, iTween.Hash("y", mainCamera.transform.position.y + distance, "time", movecameraSlowly * movecameraTime, "easeType", "easeOutSine"));
            MoveStar(distance, movecameraSlowly);

        }
    }
}
