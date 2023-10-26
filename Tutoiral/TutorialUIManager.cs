using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class TutorialUIManager : MonoBehaviour {

    public Image returntoHome;
    public Image dottedLine;
    public Transform DottedLineParent;

    public GameObject playingUI;
    public GameObject teachUI;
    public GameObject teachShootUI;
    public Text passNumber;
    public Text teachPlay;
    public Text teachShoot;

    public SpriteRenderer rightFinger;
    public SpriteRenderer cricle;
    public bool rightDirection = false;

    private Image[ ] dottedlineArray = new Image[40];

    void Awake( )
    {

    }

    void Start( )
    {
        CreateDottedLine( );
        TutorialGM.GameSetUp.gameMode = GameMode.Teaching;
        teachPlay.text = LanguagePack.teachPlay;
        teachShoot.text = LanguagePack.teachShoot;
    }
    
    void Update( )
    {
        //Debug.Log(TutorialGM.GameSetUp.gameMode);
        //Debug.Log(TutorialGM.GameSetUp.motiomMode);
        //Debug.Log(TutorialGM.PlanetManager.stepNumber);
        //Debug.Log("rightDirection : " + rightDirection);
        //Debug.Log(TutorialGM.PlanetManager.planetList.Count);
        switch(TutorialGM.GameSetUp.gameMode) {
            case GameMode.Teaching:
                if(TutorialGM.GameSetUp.motiomMode == MotionMode.Turn) {
                    ShowTeachPlayZone( );
                    if(TutorialGM.PlanetManager.stepNumber == 0) {
                        teachUI.gameObject.SetActive(true);
                    }
                    else {
                        teachUI.gameObject.SetActive(false);
                    }
                }
                else {
                    ShowTeachShootUI( );
                    DottedLineParent.gameObject.SetActive(false);
                }
                break;
        }
    }

    void ShowTeachPlayZone( )
    {
        DottedLineFallowShip( );
        if(rightDirection) {
            rightFinger.gameObject.SetActive(true);
            cricle.gameObject.SetActive(true);
        }
        else {
            rightFinger.gameObject.SetActive(false);
            cricle.gameObject.SetActive(false);
        }
        if(TutorialGM.PlanetManager.rotateDirection == 0) {
            rightFinger.transform.localPosition = new Vector3(-120, -161, 0);
            cricle.transform.localPosition = new Vector3(-131.8f, -131, 0);
        }
        else {
            rightFinger.transform.localPosition = new Vector3(120, -161, 0);
            cricle.transform.localPosition = new Vector3(108.2f, -131, 0);
        }
    }

    void ShowTeachShootUI( )
    {
        if(TutorialGM.PlanetManager.stepNumber == TutorialGM.GameSetUp.createProportion[0] && TutorialGM.GameSetUp.motiomMode == MotionMode.TeachShoot) {
            //Debug.Log("teach to shot");
            StartCoroutine(ShowTeachToShoot( ));
        }
        else {
            teachShootUI.SetActive(false);
        }
    }

    IEnumerator ShowTeachToShoot( )
    {
        yield return new WaitForSeconds(0.25f);
        teachShootUI.SetActive(true);
    }

    public void CreateDottedLine( )
    {
        for(int i = 0; i < dottedlineArray.Length; i++) {
            dottedlineArray[i] = Instantiate(dottedLine) as Image;
            dottedlineArray[i].transform.SetParent(DottedLineParent);
            dottedlineArray[i].transform.localScale = new Vector3(0.8f, 0.8f, 0);
        }
    }

    public void DottedLineFallowShip( )
    {
        for(int i = 0; i < dottedlineArray.Length; i++) {
            dottedlineArray[i].transform.up = TutorialSpaceShip.spaceShip.transform.up;
            dottedlineArray[i].transform.position = TutorialSpaceShip.spaceShip.transform.position + TutorialSpaceShip.spaceShip.transform.up * (0.5f + (0.2f * i));
        }
    }

    public void ControlDottedLineLength( TutorialPlanet p )
    {
        float distance = Tools.FindStraightLineDistanceAboutOutSidePointToCircumference(TutorialSpaceShip.spaceShip.transform, p.transform, p.pathHeight);
        for(int i = 0; i < dottedlineArray.Length; i++) {
            if(0.5f + (0.2f * i) >= distance) {
                dottedlineArray[i].enabled = false;
            }
            else {
                dottedlineArray[i].enabled = true;
            }
        }
    }
}
