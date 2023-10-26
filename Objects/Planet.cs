using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public enum TurnMode
    {
        Clockwise,
        Counterclockwise
    }
    public TurnMode rotateMode = TurnMode.Clockwise;
    public int planetNumber;
    public bool isPass = false;
    public bool isLeave = false;
    public float pathHeight = 1.6f;
    public float rotateSpeed;
    private float teachRotateSpeed = 75.0f;
    private float baseRotateSpeed = 150.0f;
    public Vector3 setPosition;
    public Transform pathPosition;

    public SpriteRenderer detail;
    public SpriteRenderer halo;

    public Renderer thisRender;
    private Transform newPlanet;
    public Transform rotation;

    //public bool isSatellite = false;

    void Start () {
        newPlanet = this.transform;
        rotation = this.gameObject.transform.FindChild("rotation");
        thisRender = this.GetComponent<SpriteRenderer>( );
        detail.sprite = GM.SpriteManager.planetDetail[Random.Range(0, GM.SpriteManager.planetDetail.Length)];
        detail.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        if(this == GM.PlanetManager.firstPlanet) {
            GetComponent<SpriteRenderer>( ).sprite = GM.PlanetManager.earth;
            detail.gameObject.SetActive(false);
        }
        SetRotateSpeed( );
    }
	
    void SetRotateSpeed( )
    {
        if(planetNumber <= GM.GameSetUp.createProportion[0]) {
            rotateSpeed = teachRotateSpeed + (planetNumber * 7.5f);
        }
        else {
            rotateSpeed = baseRotateSpeed;
        }
    }

	void Update () {
        switch(GM.GameSetUp.motiomMode) {
            case MotionMode.Drift:
                InToThePath( );
                break;
            case MotionMode.Turn:
                if(GM.GameSetUp.gameMode == GameMode.Teaching && !GameSetUp.isTutorial_1_Finished) {
                    if(GM.UIManager.rightDirection) {
                        DecideTurnedDirection(0.05f);
                    }
                    else {
                        DecideTurnedDirection(1.0f);
                    }
                }
                else {
                    DecideTurnedDirection(1.0f);
                }
                break;
            //case MotionMode.TeachPlay:
            //    if(GM.GameSetUp.gameMode == GameMode.Teaching) {
            //        if(GM.UIManager.rightDirection) {
            //            DecideTurnedDirection(0.05f);
            //        }
            //        else {
            //            DecideTurnedDirection(1.0f);
            //        }
            //    }
            //    break;
        }

        if(!thisRender.isVisible && planetNumber < GM.PlanetManager.stepNumber) {
            newPlanet.gameObject.SetActive(false);
        }
        else {
            newPlanet.gameObject.SetActive(true);
        }
    }

    void InToThePath( )
    {
        if(Vector2.Distance(SpaceShip.spaceShip.transform.position, newPlanet.position) <= pathHeight && !isLeave) {
            //Debug.Log("match");
            //Debug.Log("pathHeight : " + pathHeight);
            //Debug.Log(SpaceShip.spaceShip.transform.position);
            Vector3 gravityDirection = SpaceShip.spaceShip.transform.position - newPlanet.position;
            SpaceShip.currentPlanet = this;
            GM.PlanetManager.stepNumber = SpaceShip.currentPlanet.planetNumber;
            GM.UIManager.passNumber.text = SpaceShip.currentPlanet.planetNumber.ToString( );
            if(GM.PlanetManager.CheckShipRotateDirection(gravityDirection, SpaceShip.spaceShip.transform.right)) {
                rotateMode = TurnMode.Counterclockwise;
            }
            else {
                rotateMode = TurnMode.Clockwise;
            }
            //AddEnergy( );
            //GM.setUp.MoveCamera( );
            GM.GameSetUp.motiomMode = MotionMode.ReadyToTurn;

        }
    }

    private void DecideTurnedDirection(float scaleFloat)
    {
        //if(SpaceShip.spaceShip.transform.parent == null && SpaceShip.currentPlanet != null) {
        //    //Debug.Log("run circle");
        //}
        SpaceShip.spaceShip.transform.parent = SpaceShip.currentPlanet.rotation;
        //SpaceShip.spaceShip.transform.localPosition = Vector3.zero;

        if(rotateMode == TurnMode.Counterclockwise) {
            rotation.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed * scaleFloat);
        }
        if(rotateMode == TurnMode.Clockwise) {
            rotation.Rotate(Vector3.back * Time.deltaTime * rotateSpeed * scaleFloat);
        }
    }

    //void AddEnergy( )
    //{
    //    if(GM.UIManager.energy - GM.UIManager.nowEnergy >= GM.UIManager.addEnergy) {
    //        GM.UIManager.nowEnergy += GM.UIManager.addEnergy;
    //    }
    //    else {
    //        GM.UIManager.nowEnergy = GM.UIManager.energy;
    //    }
    //}   

    //public GameManager gameManager
    //{
    //    get {
    //        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
    //        return gm.GetComponent<GameManager>( );
    //    }
    //}

    //if(GameManager.CheckShipRotateDirection(gravityDirection, SpaceShip.ship.transform.right)) {
    //    rotation.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    //    //SpaceShip.ship.transform.right = gravityDirection;
    //}
    //else {
    //    rotation.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
    //    //SpaceShip.ship.transform.right = -gravityDirection;
    //}

    //float angle = 0;
    //if(CheckShipRotateDirection( )) {
    //    Vector3 vectorToTarget = gravityDirection - SpaceShip.ship.transform.right;
    //    angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    //    SpaceShip.ship.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    rotation.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    //}
    //else {
    //    Vector3 vectorToTarget = gravityDirection - SpaceShip.ship.transform.right;
    //    angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
    //    SpaceShip.ship.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    rotation.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
    //}

    //public bool CheckShipRotateDirection( )
    //{
    //    Debug.Log("check dot");
    //    bool check = false;
    //    if(Vector2.Dot(gravityDirection, SpaceShip.ship.transform.right) < 0) {
    //        check = false;
    //    }
    //    else {
    //        check = true;
    //    }
    //    return check;
    //}
}
