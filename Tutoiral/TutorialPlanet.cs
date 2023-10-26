using UnityEngine;
using System.Collections;

public class TutorialPlanet : MonoBehaviour {

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

    void Start () {
        newPlanet = this.transform;
        rotation = this.gameObject.transform.FindChild("rotation");
        thisRender = this.GetComponent<SpriteRenderer>( );
        detail.sprite = TutorialGM.SpriteManager.planetDetail[Random.Range(0, TutorialGM.SpriteManager.planetDetail.Length)];
        detail.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        if(this == TutorialGM.PlanetManager.firstPlanet) {
            GetComponent<SpriteRenderer>( ).sprite = TutorialGM.PlanetManager.earth;
            detail.gameObject.SetActive(false);
        }
        SetRotateSpeed( );
    }
	
    void SetRotateSpeed( )
    {
        if(planetNumber <= TutorialGM.GameSetUp.createProportion[0]) {
            rotateSpeed = teachRotateSpeed + (planetNumber * 7.5f);
        }
        else {
            rotateSpeed = baseRotateSpeed;
        }
    }

	void Update () {
        switch(TutorialGM.GameSetUp.motiomMode) {
            case MotionMode.Drift:
                InToThePath( );
                break;
            case MotionMode.Turn:
                if(TutorialGM.GameSetUp.gameMode == GameMode.Teaching) {
                    if(TutorialGM.UIManager.rightDirection) {
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
        }

        if(!thisRender.isVisible && planetNumber < TutorialGM.PlanetManager.stepNumber) {
            newPlanet.gameObject.SetActive(false);
        }
        else {
            newPlanet.gameObject.SetActive(true);
        }
    }

    void InToThePath( )
    {
        if(Vector2.Distance(TutorialSpaceShip.spaceShip.transform.position, newPlanet.position) <= pathHeight && !isLeave) {
            Vector3 gravityDirection = TutorialSpaceShip.spaceShip.transform.position - newPlanet.position;
            TutorialSpaceShip.currentPlanet = this;
            TutorialGM.PlanetManager.stepNumber = TutorialSpaceShip.currentPlanet.planetNumber;
            //TutorialGM.UIManager.passNumber.text = TutorialSpaceShip.currentPlanet.planetNumber.ToString( );
            if(TutorialGM.PlanetManager.CheckShipRotateDirection(gravityDirection, TutorialSpaceShip.spaceShip.transform.right)) {
                rotateMode = TurnMode.Counterclockwise;
            }
            else {
                rotateMode = TurnMode.Clockwise;
            }
            TutorialGM.GameSetUp.motiomMode = MotionMode.ReadyToTurn;
        }
    }

    private void DecideTurnedDirection(float scaleFloat)
    {
        TutorialSpaceShip.spaceShip.transform.parent = TutorialSpaceShip.currentPlanet.rotation;
        if(rotateMode == TurnMode.Counterclockwise) {
            rotation.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed * scaleFloat);
        }
        if(rotateMode == TurnMode.Clockwise) {
            rotation.Rotate(Vector3.back * Time.deltaTime * rotateSpeed * scaleFloat);
        }
    }
}
