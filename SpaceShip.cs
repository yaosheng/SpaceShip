using UnityEngine;
using System.Collections;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;

public class SpaceShip : MonoBehaviour {

    public static SpriteRenderer spaceShip;
    public static Planet targetPlanet;
    public static Planet currentPlanet;
    public static bool isbuttonDown = false;

    public ParticleSystem originalFire;
    public ParticleSystem bigFire;

    private Ray forwardRay;
    private float rotateSpeed = 15f;
    private float runSpeed = 5.0f;
    private float timeforTurn = 0;
    private float timer;

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
        spaceShip = GetComponent<SpriteRenderer>( );
        originalFire.gameObject.SetActive(true);
        bigFire.gameObject.SetActive(false);
    }

    void Update( )
    {
        switch(GM.GameSetUp.gameMode) {
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

    Planet FindPlanet(int pn)
    {
        Planet[ ] tempplanetArray = GM.PlanetManager.planetList.ToArray( );
        Planet p = currentPlanet;
        for(int i = 1; i <tempplanetArray.Length; i++) {
            if(tempplanetArray[i].planetNumber == pn) {
                p = tempplanetArray[i];
            }
        }
        return p;
    }

    void RunTime( )
    {
        switch(GM.GameSetUp.motiomMode) {
            case MotionMode.Drift:
                RunForward( );
                break;
            case MotionMode.ReadyToTurn:
                FixedShipPosition( );
                FixedPassedPlanet( );
                AdsReportingManager.LogEvent("arrive stage " + currentPlanet.planetNumber.ToString( ));
                CheckTutorial( );
                GM.GameSetUp.motiomMode = MotionMode.Turn;
                break;
            case MotionMode.Turn:
                TurnToPath( );
                DirectionThePlanet(FindPlanet(GM.PlanetManager.stepNumber + 1));
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

    void CheckTutorial( )
    {
        //Debug.Log(GameSetUp.isTutorial_1_Finished);
        if(!GameSetUp.isTutorial_1_Finished) {
            if(GM.PlanetManager.stepNumber <= GM.GameSetUp.practicePlanet) {
                GM.GameSetUp.gameMode = GameMode.Teaching;
            }
            else {
                GM.GooglePlayManager.AchievementFirstStep( );
                PlayerPrefs.SetInt("isTutorial_1_Finished", 1);
                GM.GameSetUp.gameMode = GameMode.Playing;
            }
        }
        if(!GameSetUp.isTutorial_2_Finished) {
            if(GM.PlanetManager.stepNumber == GM.GameSetUp.createProportion[0] + 1) {
                GM.GooglePlayManager.AchievementSpaceBall( );
                PlayerPrefs.SetInt("isTutorial_2_Finished", 1);
            }
        }

    }

    public void Detach( )
    {
        Debug.Log("detach");
        if(GM.PlanetManager.stepNumber <= GM.GameSetUp.practicePlanet && !GameSetUp.isTutorial_1_Finished) {
            if(GM.UIManager.rightDirection) {
                GM.GameSetUp.motiomMode = MotionMode.ReadyToDetach;
                StartCoroutine(TouchToDetach( ));
                GM.UIManager.teachUI.SetActive(false);
                timeforTurn = 0;
            }
            //else {
            //    //GM.UIManager.CrossAnimaton( );
            //    Debug.Log("click at wrong time");
            //}
        }
        else {
            GM.GameSetUp.motiomMode = MotionMode.ReadyToDetach;
            StartCoroutine(TouchToDetach( ));
            GM.UIManager.teachUI.SetActive(false);
            timeforTurn = 0;
        }
    }

    void TurnToPath( )
    {
        timeforTurn += Time.deltaTime;
        if(currentPlanet.rotateMode == Planet.TurnMode.Counterclockwise) {
            Vector3 vectorToTarget = (spaceShip.transform.position - currentPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
        }
        if(currentPlanet.rotateMode == Planet.TurnMode.Clockwise) {
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
        float temp = GM.CameraManager.mainCamera.transform.position.y - spaceShip.transform.position.y;
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
        GM.GameSetUp.motiomMode = MotionMode.Drift;
        if(GM.PlanetManager.stepNumber == GM.GameSetUp.createProportion[0] && !GameSetUp.isTutorial_2_Finished) {
            while(temp < delayTime) {
                temp += Time.deltaTime;
                Debug.Log(temp);
                yield return null;
            }
            GM.GameSetUp.motiomMode = MotionMode.TeachShoot;
        }
        else {
            FindTargetPlanet( );
        }
    }

    public void FindTargetPlanet( )
    {
        forwardRay = new Ray(spaceShip.transform.position, spaceShip.transform.up * 10);
        Planet[ ] tempArray = GM.PlanetManager.planetList.ToArray( );
        Planet nextPlanet = currentPlanet;
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
            //Renderer[ ] srArray = tempArray[i].GetComponentsInChildren<SpriteRenderer>( );
            //int temp1 = 0;
            //for(int j = 0; j < srArray.Length; j++) {
            //    if(srArray[j].isVisible) {
            //        temp1++;
            //    }
            //}
            if(Vector3.Dot(Vector3.up, forwardRay.direction) > 0) {
                if(!tempArray[i].isPass && distance <= tempArray[i].pathHeight && sr.isVisible) {
                    //Debug.Log("direction a Planet");
                    targetPlanet = tempArray[i];
                    GM.CameraManager.MoveCamera(true, currentPlanet, targetPlanet, nextPlanet);
                    GM.PlanetManager.AddNewPlanet(targetPlanet);
                    temp++;
                    break;
                }
            }
        }
        if(temp == 0) {
            //Debug.Log("direction nothing");
            targetPlanet = null;
            foreach(Planet p in GM.PlanetManager.planetList) {
                Renderer r = p.GetComponent<SpriteRenderer>( );
                if(!r.isVisible && p.transform.position.y > spaceShip.transform.position.y) {
                    p.gameObject.SetActive(false);
                }
            }
            //StartCoroutine(GM.CameraManager.MoveCamera(false, currentPlanet, currentPlanet, nextPlanet));
            GM.CameraManager.MoveCamera(false, currentPlanet, currentPlanet, nextPlanet);
        }
    }

    public Vector3 IntersectsWith(Planet p)
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

    void DirectionThePlanet(Planet p)
    {
        // Find the point of intersection with the planet.
        Vector3 intersect = IntersectsWith(p);
        if (intersect.isValid())
        {
            // intersection found
            Debug.DrawLine(transform.position, intersect, Color.magenta);
        }
        else
        {
            // miss the planet
            Debug.DrawRay(transform.position, transform.up * 20, Color.cyan);
        }

        //Ray2D forwardRay2D = new Ray2D(spaceShip.transform.position, spaceShip.transform.up);
        forwardRay = new Ray(spaceShip.transform.position, spaceShip.transform.up * 10);
        float distance = Vector3.Cross(forwardRay.direction, p.transform.position - forwardRay.origin).magnitude;
        if(Vector3.Dot(Vector3.up, forwardRay.direction) > 0 && GM.PlanetManager.stepNumber <= GM.GameSetUp.practicePlanet) {
            if(distance <= p.pathHeight) {
                //Debug.Log("hit");
                GM.UIManager.DottedLineParent.gameObject.SetActive(true);
                GM.UIManager.ControlDottedLineLength(p);
                GM.UIManager.rightDirection = true;
                //GM.UIManager.ClearForRight( );
            }
            else {
                GM.UIManager.DottedLineParent.gameObject.SetActive(false);
                GM.UIManager.rightDirection = false;
                //GM.UIManager.ClearForWrong( );
            }
        }
        else {
            GM.UIManager.DottedLineParent.gameObject.SetActive(false);
            return;
            //Debug.Log("direction down");
        }

        //RaycastHit2D hitPlanet = Physics2D.Raycast(spaceShip.transform.position + spaceShip.transform.up * 1.2f, forwardRay2D.direction * 100);
        //if(hitPlanet.collider != null) {
        //    if(hitPlanet.collider.GetComponent<Planet>( ) == p) {
        //        GM.UIManager.rightDirection = true;
        //        GM.UIManager.ClearForRight( );
        //    }
        //}
        //else {
        //    GM.UIManager.rightDirection = false;
        //    GM.UIManager.ClearForWrong( );
        //}
    }

    void FixedPassedPlanet( )
    {
        foreach(Planet p in GM.PlanetManager.planetList) {
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
        //currentPlanet.pathPosition.localPosition = currentPlanet.pathPosition.InverseTransformPoint(spaceShip.transform.position);
    }

    //void OutOfCamera( )
    //{
    //    if(timer > 3.0f) {
    //        if(!spaceShip.isVisible || spaceShip.transform.position.x < -5.0f || spaceShip.transform.position.x > 5.0f) {
    //            StartCoroutine(GM.CameraManager.LateToGameOver( ));
    //        }
    //    }
    //    else {
    //        timer += Time.deltaTime;
    //    }
    //}

    //IEnumerator LateToGameOver( )
    //{
    //    yield return new WaitForSeconds(0.75f);
    //    GM.GameSetUp.gameMode = GameMode.GameOver;
    //    spaceShip.gameObject.SetActive(false);
    //}

    //if(!GM.CameraManager.islockedcameraMoving) {
    //    //StartCoroutine(GM.CameraManager.MoveCamera(true, currentPlanet, targetPlanet, nextPlanet));
    //    GM.CameraManager.MoveCamera(true, currentPlanet, targetPlanet, nextPlanet);
    //}
    //StartCoroutine(GM.CameraManager.MoveCamera(true, currentPlanet, targetPlanet, nextPlanet));

    //}
    //if(GM.PlanetManager.stepNumber == 10) {
    //    temp += Time.deltaTime;
    //    Debug.Log(temp);
    //    yield return new WaitForSeconds(2.25f);
    //    GM.GameSetUp.motiomMode = MotionMode.Teach;
    //}
    //else {
    //    GM.GameSetUp.motiomMode = MotionMode.Drift;
    //}

    //yield return new WaitForSeconds(0.25f);
    //if(GM.PlanetManager.stepNumber == 10) {
    //    GM.GameSetUp.motiomMode = MotionMode.Teach;
    //}
    //else {
    //    GM.GameSetUp.motiomMode = MotionMode.Drift;
    //}

    //if(GM.PlanetManager.stepNumber == 10) {
    //    if(Input.GetButton("Fire1")) {
    //        GM.CameraManager.CameraFallowSpaceShip(tempDistance);
    //        spaceShip.transform.Translate(Vector3.up * runSpeed * Time.deltaTime);
    //    }
    //}
    //else {
    //    spaceShip.transform.Translate(Vector3.up * runSpeed * Time.deltaTime);
    //}

    //void LateUpdate () {
    //    //Debug.Log("GM.setUp.mode : " + GM.setUp.mode);
    //    if(GM.GameSetUp.motiomMode == MotionMode.ReadyToTurn) {
    //        FixedShipPosition( );
    //        FixedPassedPlanet( );
    //        GM.GameSetUp.motiomMode = MotionMode.Turn;
    //    }
    //}

    //void DoingBeforeInToOrbit( )
    //{
    //    //int temp = int.Parse(UIManager.UI.passNumber.ToString( ));
    //    //if(temp < targetPlanet.planetNumber) {
    //    //    Debug.Log("Right Direction");
    //    //}
    //}

    //Vector3 movement = transform.up * 20 * Time.deltaTime;
    //spaceShip.MovePosition(spaceShip.transform.position + movement);

    //void FixedUpdate()
    //{
    //    switch(GM.GameSetUp.gameMode) {
    //        case GameMode.Playing:
    //            //Debug.Log("game playing");
    //            RunTimeMethod( );
    //            break;
    //        case GameMode.Opening:
    //            //Debug.Log("game opening");
    //            break;
    //    }
    //}

    //IEnumerator Start () {
    //    GM.GameSetUp.motiomMode = MotionMode.Idle;
    //    Reset( );
    //    FindFirstPlanet( );
    //    FaceToFirstPlanet( );
    //    yield return new WaitForSeconds(1.0f);
    //    GM.GameSetUp.motiomMode = MotionMode.Drift;
    //}

    //void Reset( )
    //{

    //}

    //void FindFirstPlanet( )
    //{
    //    foreach(Planet p1 in GM.GameSetUp.planetList) {
    //        currentPlanet = p1;
    //        break;
    //    }
    //}

    //void FaceToFirstPlanet( )
    //{
    //    Vector2 vectorToTarget = currentPlanet.transform.position - spaceShip.transform.position;
    //    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
    //    spaceShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //}

    //void Shooter( )
    //{
    //    if(Input.GetButton("Fire1")) {
    //        if(BulletManager.bulletPool.Count > 0) {
    //            SpriteRenderer sr = BulletManager.bulletPool.Dequeue( );
    //            sr.gameObject.SetActive(true);
    //            GM.UI.nowEnergy -= shootEnergy;
    //        }
    //        else {
    //            Debug.Log("no bullet");
    //            SpriteRenderer sr1 = Instantiate(bulletSprite) as SpriteRenderer;
    //            BulletManager.bulletPool.Enqueue(sr1);
    //        }
    //    }
    //}

    //sr.transform.parent = bulletParent;
    //sr.transform.up = ship.transform.up;
    //sr.transform.position = ship.transform.position + ship.transform.up * 1.25f;
    //sr.transform.localScale = new Vector3(1.2f, 4, 0);
    //GM.UI.nowEnergy -= shootEnergy;

    //if(sr.gameObject.activeSelf) {
    //    sr.transform.parent = bulletParent;
    //    sr.transform.up = ship.transform.up;
    //    sr.transform.position = ship.transform.position + ship.transform.up * 1.25f;
    //    sr.transform.localScale = new Vector3(1.2f, 4, 0);
    //}

    //foreach(Planet p in GM.setUp.planetList) {
    //    distance = Vector3.Cross(forwardRay.direction, p.transform.position - forwardRay.origin).magnitude;
    //    if(!p.isPass && distance <= p.pathHeight) {
    //        tempP = p;
    //        targetPlanet = p;
    //        break;
    //    }
    //}
    //return tempP;
    //Debug.DrawRay(transform.position, forwardRay.direction, Color.green);

    //for(int i = 0; i < tempArray.Length; i++) {
    //    distance = Vector3.Cross(forwardRay.direction, tempArray[i].transform.position - forwardRay.origin).magnitude;
    //    if(!tempArray[i].isPass && distance <= tempArray[i].pathHeight) {
    //        targetPlanet = tempArray[i];
    //        GM.setUp.MoveCamera(currentPlanet, targetPlanet);
    //        temp++;
    //        break;
    //    }
    //}

    //IEnumerator TurnToPath( )
    //{
    //    if(targetPlanet.rotateMode == Planet.TurnMode.Counterclockwise) {
    //        Vector3 vectorToTarget = (ship.transform.position - targetPlanet.transform.position) - transform.right * 0.1f;
    //        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    //        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

    //        Debug.Log("Planet.gravityDirection : " + targetPlanet.gravityDirection);
    //        Debug.Log("transform.right : " + transform.right);
    //        Debug.Log("angle : " + angle);
    //    }
    //    if(targetPlanet.rotateMode == Planet.TurnMode.Clockwise) {
    //        Vector3 vectorToTarget = (ship.transform.position - targetPlanet.transform.position) - transform.right * 0.1f;
    //        float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
    //        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    //    }
    //    yield return new WaitForSeconds(0.2f);
    //    GameManager.GM.mode = MotionMode.Turn;
    //}

    //void HitTargetPlanetAndChangeColor( )
    //{

    //}

    //if(targetPlanet != null) {
    //    //Vector3 direction = this.transform.up * 25;
    //    forwardRay = new Ray(transform.position, this.transform.up * 25);
    //    float distence = Vector3.Cross(forwardRay.direction, targetPlanet.transform.position - forwardRay.origin).magnitude;
    //    Debug.Log("distence : " + distence);
    //    //Debug.DrawRay(transform.position, direction, Color.green);
    //}

    //IEnumerator TouchToDetach( )
    //{
    //    Debug.Log("detach !");
    //    ship.transform.parent = null;
    //    targetPlanet.isLeave = true;
    //    targetPlanet = null;
    //    yield return new WaitForSeconds(0.25f);
    //    GameManager.mode = MotionMode.Drift;
    //}

    //public static bool isTurned = false;
    //public static bool isDetached = false;

    //Debug.Log("start to trun : " + ship.transform.right);
    //Debug.Log("gravityDirection : " + Planet.gravityDirection);
    //Vector3 vectorToTarget = Planet.gravityDirection - transform.right;
    //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

    //ship.transform.right = Planet.gravityDirection;
    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(ship.transform.up, Planet.gravityDirection), rotateSpeed * Time.deltaTime);

    //Vector3 vectorToTarget = Planet.newPlanet.transform.position - transform.position;
    //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
    //Quaternion q = Quaternion.AngleAxis(angle, -Vector3.forward);
    //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

    //mode = MotionMode.motion;

    //Vector3 v = Planet.gravityDirection - ship.transform.up;
    //float angle = 90 + Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    //Debug.Log("angle : " + angle);


    //rightVector2 = new Vector2(ship.transform.position.x, 0);
    //Quaternion rotation = Quaternion.LookRotation(Planet.gravityDirection - ship.transform.right, Vector3.up);
    //rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

    //ship.transform.rotation = rotation;

    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.right, Planet.gravityDirection - rightVector2), rotateSpeed * Time.deltaTime);

    //ship.MoveRotation(rotation);

    //rotation = Quaternion.LookRotation(pillers[startPoint].localPosition - playerRigid.transform.localPosition, Vector3.up);
    //rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed* Time.deltaTime);
    //playerRigid.MoveRotation(rotation);
}
