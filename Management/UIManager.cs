using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class UIManager : MonoBehaviour {

    public Text passNumber;
    public Text coinNumber;
    public Image gameover;
    public Image titleImage;
    public Image startButton;
    public Image dottedLine;
    public Transform DottedLineParent;

    public GameObject playingUI;
    public GameObject gameoverUI;
    public GameObject openingUi;
    public GameObject teachUI;
    public GameObject teachShootUI;
    public Text teachPlay;
    public Text teachShoot;
    public Text score;
    public Text best;

    public Button restartButton;
    public Button continueButton;

    public Button soundOpen;
    public Button soundClose;
    public Button soundOpen1;
    public Button soundClose1;
    public Button pauseButton;

    public Text scoreNumber;
    public Text bestNumber;
    public Text coinText;
    public int coin;
    public int coinSum;

    public SpriteRenderer rightFinger;
    public SpriteRenderer cricle;
    public bool rightDirection = false;

    private Image[ ] dottedlineArray = new Image[40];
    private bool isOvered = false;
    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    void Start( )
    {
        CreateDottedLine( );
        coinSum = GameSetUp.coinAmount;
        if(GM.GameSetUp.isSoundOn) {
            soundOpen.gameObject.SetActive(true);
            soundClose.gameObject.SetActive(false);
            soundOpen1.gameObject.SetActive(true);
            soundClose1.gameObject.SetActive(false);
        }
        else {
            soundOpen.gameObject.SetActive(false);
            soundClose.gameObject.SetActive(true);
            soundOpen1.gameObject.SetActive(false);
            soundClose1.gameObject.SetActive(true);
        }
        teachPlay.text = LanguagePack.teachPlay;
        teachShoot.text = LanguagePack.teachShoot;
        score.text = LanguagePack.score;
        best.text = LanguagePack.best;
        //GM.GameSetUp.gameMode = GameMode.Opening;

    }
    
    void Update( )
    {
        //Debug.Log(GM.GameSetUp.gameMode);
        switch(GM.GameSetUp.gameMode) {
            case GameMode.Playing:
                Playing( );
                ShowTeachShootUI( );
                break;
            case GameMode.Teaching:
                Playing( );
                if(GM.GameSetUp.motiomMode == MotionMode.Turn) {
                    if(!GameSetUp.isTutorial_1_Finished) {
                        ShowTeachPlayZone( );
                        if(GM.PlanetManager.stepNumber == 0) {
                            teachUI.gameObject.SetActive(true);
                        }
                        else {
                            teachUI.gameObject.SetActive(false);
                        }
                    }
                }
                else {
                    DottedLineParent.gameObject.SetActive(false);
                }
                break;

            case GameMode.Opening:
                //Debug.Log("opening");
                Opening( );
                break;
            case GameMode.GameOver:
                //Debug.Log("gameover");
                GameOver( );
                break;
            case GameMode.Pause:
                Pause( );
                break;
        }
    }

    public void Opening( )
    {
        openingUi.SetActive(true);
        playingUI.SetActive(false);
        gameoverUI.SetActive(false);
        DottedLineParent.gameObject.SetActive(false);
    }

    public void Playing( )
    {
        pauseButton.gameObject.SetActive(true);
        playingUI.SetActive(true);
        openingUi.SetActive(false);
        gameoverUI.SetActive(false);
        coinNumber.text = coinSum.ToString( );
        passNumber.text = GM.PlanetManager.stepNumber.ToString( );
    }

    void GameOver( )
    {
        if(!isOvered) {
            pauseButton.gameObject.SetActive(false);
            gameoverUI.SetActive(true);
            restartButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
            scoreNumber.text = GM.PlanetManager.stepNumber.ToString( );
            coinText.text = "+" + coin.ToString( );
            PlayerPrefs.SetInt("coinAmount", coinSum);
            //if(PlayerPrefs.GetInt("coinAmount") >= 10000) {
            //    GM.GooglePlayManager.AchievementGoldFarmer();
            //}
            if(int.Parse(scoreNumber.text.ToString( )) > GM.GameSetUp.bestScore) {
                bestNumber.text = scoreNumber.text.ToString( );
                PlayerPrefs.SetInt("bestScore", int.Parse(scoreNumber.text.ToString( )));
            }
            else {
                bestNumber.text = GM.GameSetUp.bestScore.ToString( );
            }
            Debug.Log("Score : " + GM.PlanetManager.stepNumber);
            Debug.Log("Best Score : " + PlayerPrefs.GetInt("bestScore"));
            AdsReportingManager.LogEvent("Score", GM.PlanetManager.stepNumber);
            AdsReportingManager.LogEvent("Best Score", PlayerPrefs.GetInt("bestScore"));
            isOvered = true;
        }

    }

    public void Pause( )
    {
        gameoverUI.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        scoreNumber.text = GM.PlanetManager.stepNumber.ToString( );
        coinText.text = "+" + coin.ToString( );
    }

    void ShowTeachPlayZone( )
    {
        if(!GameSetUp.isTutorial_1_Finished) {
            DottedLineFallowShip( );
            if(rightDirection) {
                rightFinger.gameObject.SetActive(true);
                cricle.gameObject.SetActive(true);
            }
            else {
                rightFinger.gameObject.SetActive(false);
                cricle.gameObject.SetActive(false);
            }
            if(GM.PlanetManager.rotateDirection == 0) {
                rightFinger.transform.localPosition = new Vector3(-120, -161, 0);
                cricle.transform.localPosition = new Vector3(-131.8f, -131, 0);
            }
            else {
                rightFinger.transform.localPosition = new Vector3(120, -161, 0);
                cricle.transform.localPosition = new Vector3(108.2f, -131, 0);
            }
        }
        else {
            DottedLineParent.gameObject.SetActive(false);
        }
    }

    void ShowTeachShootUI( )
    {
        if(GM.PlanetManager.stepNumber == GM.GameSetUp.createProportion[0] && GM.GameSetUp.motiomMode == MotionMode.TeachShoot) {
            Debug.Log("teach to shot");
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

    //public void CrossAnimaton( )
    //{
    //    iTween.ScaleTo(cross.gameObject, iTween.Hash("x", 1.35f, "y", 1.35f, "islocal", true, "time", 0.1f, "easeType", "easeOutSine"));
    //    iTween.ScaleTo(cross.gameObject, iTween.Hash("x", 1, "y", 1, "islocal", true, "time", 0.05f, "delay", 0.1f,  "easeType", "easeOutSine"));
    //}

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
            dottedlineArray[i].transform.up = SpaceShip.spaceShip.transform.up;
            dottedlineArray[i].transform.position = SpaceShip.spaceShip.transform.position + SpaceShip.spaceShip.transform.up * (0.5f + (0.2f * i));
        }
    }

    public void ControlDottedLineLength( Planet p )
    {
        float distance = Tools.FindStraightLineDistanceAboutOutSidePointToCircumference(SpaceShip.spaceShip.transform, p.transform, p.pathHeight);
        for(int i = 0; i < dottedlineArray.Length; i++) {
            if(0.5f + (0.2f * i) >= distance) {
                dottedlineArray[i].enabled = false;
            }
            else {
                dottedlineArray[i].enabled = true;
            }
        }
    }

    //public void ClearForRight( )
    //{
    //    //zoneLeft.color = new Color32(255, 255, 255, 255);
    //    //zoneRight.color = new Color32(255, 255, 255, 255);
    //}

    //public void ClearForWrong( )
    //{
    //    //zoneLeft.gameObject.SetActive(false);
    //    //zoneRight.gameObject.SetActive(false);
    //}

    //void ClearTeachText( )
    //{
    //    if(Input.GetButton("Fire1")) {
    //        teachUI.SetActive(false);
    //    }
    //}




    //void KeepLostEnergy( )
    //{
    //    if(nowEnergy > 0) {
    //        energyLine.transform.localScale = new Vector3(nowEnergy / 320.0f, 1, 0);
    //        nowEnergy -= lostEnergy * Time.deltaTime;
    //    }
    //    else
    //    {
    //        nowEnergy = 0;
    //        //SpaceShip.ship.gameObject.SetActive(false);
    //        //gameover.gameObject.SetActive(true);
    //    }
    //}

}
