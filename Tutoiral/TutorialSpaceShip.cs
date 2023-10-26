using UnityEngine;
using System.Collections;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class TutorialSpaceShip : MonoBehaviour {

    public static SpriteRenderer spaceShip;
    public static TutorialPlanet targetPlanet;
    public static TutorialPlanet currentPlanet;
    public static bool isbuttonDown = false;

    public ParticleSystem originalFire;
    public ParticleSystem bigFire;

    private Ray forwardRay;
    private float rotateSpeed = 15f;
    private float runSpeed = 5.0f;
    private float timeforTurn = 0;
    private float timer;

    //private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        //AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
        spaceShip = GetComponent<SpriteRenderer>( );
        originalFire.gameObject.SetActive(true);
        bigFire.gameObject.SetActive(false);
    }

    void Update( )
    {

        //Debug.Log(currentPlanet);
        //Debug.Log(TutorialGM.GameSetUp.motiomMode);
        switch(TutorialGM.GameSetUp.gameMode) {
            case GameMode.Playing:
                RunTime( );
                break;
            case GameMode.Teaching:
                RunTime( );
                break;
            case GameMode.Opening:
                break;
        }
    }

    TutorialPlanet FindPlanet(int pn)
    {
        TutorialPlanet[ ] tempplanetArray = TutorialGM.PlanetManager.planetList.ToArray( );
        TutorialPlanet p = currentPlanet;
        for(int i = 1; i <tempplanetArray.Length; i++) {
            if(tempplanetArray[i].planetNumber == pn) {
                p = tempplanetArray[i];
            }
        }
        return p;
    }

    void RunTime( )
    {
        switch(TutorialGM.GameSetUp.motiomMode) {
            case MotionMode.Drift:
                RunForward( );
                break;
            case MotionMode.ReadyToTurn:
                FixedShipPosition( );
                FixedPassedPlanet( );
                //AdsReportingManager.LogEvent("arrive stage " + currentPlanet.planetNumber.ToString( ));
                CheckTutorial( );
                CheckToFadeIn( );
                TutorialGM.GameSetUp.motiomMode = MotionMode.Turn;
                break;
            case MotionMode.Turn:
                TurnToPath( );
                DirectionThePlanet(FindPlanet(TutorialGM.PlanetManager.stepNumber + 1));
                if(timeforTurn > 0.25f) {
                    if(isbuttonDown) {
                        Detach( );
                    }
                }
                break;
            case MotionMode.ReadyToDetach:
                break;
        }
    }

    void CheckToFadeIn( )
    {
        if(TutorialGM.PlanetManager.stepNumber == 2) {
            //TutorialGM.UIManager.ProjectFadeIn( );
            //FadeIn1.ProjectFadeIn( );
            //FadeIn2.ProjectFadeIn( );
            FadeIn1 tempfade1 = GameObject.FindObjectOfType(typeof(FadeIn1)) as FadeIn1;
            tempfade1.ProjectFadeIn( );
            FadeIn2 tempfade2 = GameObject.FindObjectOfType(typeof(FadeIn2)) as FadeIn2;
            tempfade2.ProjectFadeIn( );
        }
    }

    void CheckTutorial( )
    {
        if(TutorialGM.PlanetManager.stepNumber <= TutorialGM.GameSetUp.practicePlanet) {
            TutorialGM.GameSetUp.gameMode = GameMode.Teaching;
        }
    }

    public void Detach( )
    {
        Debug.Log("detach");
        if(TutorialGM.PlanetManager.stepNumber <= TutorialGM.GameSetUp.practicePlanet) {
            if(TutorialGM.UIManager.rightDirection) {
                TutorialGM.GameSetUp.motiomMode = MotionMode.ReadyToDetach;
                StartCoroutine(TouchToDetach( ));
                TutorialGM.UIManager.teachUI.SetActive(false);
                timeforTurn = 0;
            }
        }
        else {
            TutorialGM.GameSetUp.motiomMode = MotionMode.ReadyToDetach;
            StartCoroutine(TouchToDetach( ));
            TutorialGM.UIManager.teachUI.SetActive(false);
            timeforTurn = 0;
        }
    }

    void TurnToPath( )
    {
        timeforTurn += Time.deltaTime;
        if(currentPlanet.rotateMode == TutorialPlanet.TurnMode.Counterclockwise) {
            Vector3 vectorToTarget = (spaceShip.transform.position - currentPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
        }
        if(currentPlanet.rotateMode == TutorialPlanet.TurnMode.Clockwise) {
            Vector3 vectorToTarget = (spaceShip.transform.position - currentPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
        }
    }

    void RunForward( )
    {
        spaceShip.transform.Translate(Vector3.up * runSpeed * Time.deltaTime);
    }

    IEnumerator StartToRunForward( float time )
    {
        float timer = 0;
        while(timer < time) {
            timer += Time.deltaTime;
            spaceShip.transform.Translate(Vector3.up * runSpeed / 4 * Time.deltaTime);
            yield return null;
        }
    }

    float GetDistanceBetweenCameraAndShip( )
    {
        //float temp = 0;
        float temp = TutorialGM.CameraManager.mainCamera.transform.position.y - spaceShip.transform.position.y;
        return temp;
    }

    IEnumerator TouchToDetach( )
    {
        spaceShip.transform.parent = null;
        currentPlanet.isLeave = true;
        currentPlanet.isPass = true;
        StartCoroutine(StartToRunForward(0.3f));
        BoostFire( );
        yield return new WaitForSeconds(0.3f);
        OriginalFire( );
        StartCoroutine(ReadyToTeachShoot(0.15f));
    }

    void OriginalFire( )
    {
        originalFire.gameObject.SetActive(true);
        bigFire.gameObject.SetActive(false);
    }

    void BoostFire( )
    {
        originalFire.gameObject.SetActive(false);
        bigFire.gameObject.SetActive(true);
    }

    IEnumerator ReadyToTeachShoot(float delayTime)
    {
        float temp = 0;
        TutorialGM.GameSetUp.motiomMode = MotionMode.Drift;

        Debug.Log("createProportion[0] : " + TutorialGM.GameSetUp.createProportion[0]);
        if(TutorialGM.PlanetManager.stepNumber == TutorialGM.GameSetUp.createProportion[0]) {
            while(temp < delayTime) {
                temp += Time.deltaTime;
                //Debug.Log(temp);
                yield return null;
            }
            TutorialGM.GameSetUp.motiomMode = MotionMode.TeachShoot;
        }
        else {
            FindTargetPlanet( );
        }
    }

    public void FindTargetPlanet( )
    {
        forwardRay = new Ray(spaceShip.transform.position, spaceShip.transform.up * 10);
        TutorialPlanet[ ] tempArray = TutorialGM.PlanetManager.planetList.ToArray( );
        TutorialPlanet nextPlanet = currentPlanet;
        float distance = 0;
        int temp = 0;

        for(int i = 0; i < tempArray.Length; i++) {
            if(tempArray[i] == currentPlanet && i != tempArray.Length - 1) {
                nextPlanet = tempArray[i + 1];
                break;
            }
        }
        for(int i = 0; i < tempArray.Length; i++) {
            distance = Vector3.Cross(forwardRay.direction, tempArray[i].transform.position - forwardRay.origin).magnitude;
            Renderer sr = tempArray[i].GetComponent<SpriteRenderer>( );
            if(Vector3.Dot(Vector3.up, forwardRay.direction) > 0) {
                if(!tempArray[i].isPass && distance <= tempArray[i].pathHeight && sr.isVisible) {
                    targetPlanet = tempArray[i];
                    TutorialGM.CameraManager.MoveCamera(true, currentPlanet, targetPlanet, nextPlanet);
                    temp++;
                    break;
                }
            }
        }
        if(temp == 0) {
            targetPlanet = null;
            foreach(TutorialPlanet p in TutorialGM.PlanetManager.planetList) {
                Renderer r = p.GetComponent<SpriteRenderer>( );
                if(!r.isVisible && p.transform.position.y > spaceShip.transform.position.y) {
                    p.gameObject.SetActive(false);
                }
            }
            TutorialGM.CameraManager.MoveCamera(false, currentPlanet, currentPlanet, nextPlanet);
        }
    }

    public Vector3 IntersectsWith( TutorialPlanet p )
    {
        Vector3 lineBegin = transform.position;
        Vector3 lineEnd = lineBegin + transform.up * Vector3.Distance(lineBegin, p.transform.position);

        Vector2 intersection1, intersection2;

        if (MathUtils.LineIntersectsCircle(lineBegin, lineEnd, p.transform.position, p.pathHeight, out intersection1, out intersection2) != 0)
        {
            // only care for the point of entry
            return intersection1;
        }
        else
        {
            return MathUtils.INVALID;
        }
    }

    void DirectionThePlanet( TutorialPlanet p )
    {
        //Debug.Log("DirectionThePlanet planet");
        //Debug.Log("target p number : " + p.planetNumber);
        Vector3 intersect = IntersectsWith(p);
        if (intersect.isValid())
        {
            Debug.DrawLine(transform.position, intersect, Color.magenta);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * 20, Color.cyan);
        }
        forwardRay = new Ray(spaceShip.transform.position, spaceShip.transform.up * 10);
        float distance = Vector3.Cross(forwardRay.direction, p.transform.position - forwardRay.origin).magnitude;
        if(Vector3.Dot(Vector3.up, forwardRay.direction) > 0 && TutorialGM.PlanetManager.stepNumber <= TutorialGM.GameSetUp.practicePlanet) {
 
            if(distance <= p.pathHeight) {
                //Debug.Log("check direction right");
                TutorialGM.UIManager.DottedLineParent.gameObject.SetActive(true);
                TutorialGM.UIManager.ControlDottedLineLength(p);
                TutorialGM.UIManager.rightDirection = true;
            }
            else {
                //Debug.Log("check direction wrong");
                TutorialGM.UIManager.DottedLineParent.gameObject.SetActive(false);
                TutorialGM.UIManager.rightDirection = false;
            }
        }
        else {
            TutorialGM.UIManager.DottedLineParent.gameObject.SetActive(false);
            return;
        }
    }

    void FixedPassedPlanet( )
    {
        foreach(TutorialPlanet p in TutorialGM.PlanetManager.planetList) {
            if(currentPlanet != null) {
                if(p != currentPlanet && currentPlanet.transform.position.y > p.transform.position.y) {
                    p.isPass = true;
                }
            }
        }
    }

    void FixedShipPosition( )
    {
        Vector3 tempVector = (spaceShip.transform.position - currentPlanet.transform.position).normalized;
        spaceShip.transform.position = currentPlanet.transform.position + (tempVector * currentPlanet.pathHeight);
    }
}
