using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialPlanetManager : MonoBehaviour {

    public List<TutorialPlanet> planetList = new List<TutorialPlanet>( );
    public SpriteRenderer[ ] Planets;
    public TutorialPlanet firstPlanet;
    public TutorialPlanet secondPlanet;
    public TutorialPlanet thirePlanet;
    public int rotateDirection;
    public Transform planetParent;
    public int planetSumNumber = 0;
    public int stepNumber;
    public Sprite earth;

    void Start () {
        SetFirstPlanet( );
        SetSecondPlanet( );
        SetThirdPlanet( );
    }
	
    public void SetFirstPlanet( )
    {
        TutorialGM.GameSetUp.motiomMode = MotionMode.Turn;
        TutorialSpaceShip.currentPlanet = firstPlanet;
        planetList.Add(firstPlanet);
        firstPlanet.rotation = firstPlanet.gameObject.transform.FindChild("rotation");
        firstPlanet.rotation.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        TutorialSpaceShip.spaceShip.transform.parent = firstPlanet.rotation;
        TutorialSpaceShip.spaceShip.transform.localPosition = new Vector3(firstPlanet.pathHeight, 0, 0);
        rotateDirection = Random.Range(0, 2);
        firstPlanet.planetNumber = 0;
        if(rotateDirection == 0) {
            firstPlanet.rotateMode = TutorialPlanet.TurnMode.Clockwise;
            Vector3 vectorToTarget = (TutorialSpaceShip.spaceShip.transform.position - firstPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
            TutorialSpaceShip.spaceShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else {
            firstPlanet.rotateMode = TutorialPlanet.TurnMode.Counterclockwise;
            Vector3 vectorToTarget = (TutorialSpaceShip.spaceShip.transform.position - firstPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            TutorialSpaceShip.spaceShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        Debug.Log("planetSumNumber : " + planetSumNumber);
    }

    public void SetSecondPlanet( )
    {
        secondPlanet.transform.parent = planetParent;
        secondPlanet.transform.localPosition = new Vector3(0, 3.06f, 0);
        secondPlanet.planetNumber = 1;
        planetList.Add(secondPlanet);
        Debug.Log("planetSumNumber : " + planetSumNumber);
    }

    public void SetThirdPlanet( )
    {
        thirePlanet.transform.parent = planetParent;
        thirePlanet.transform.localPosition = new Vector3(0, 10.5f, 0);
        thirePlanet.planetNumber = 2;
        planetList.Add(thirePlanet);
        Debug.Log("planetSumNumber : " + planetSumNumber);
    }

    void SetPathObject( TutorialPlanet p1, TutorialPlanet p2 )
    {
        Debug.Log("planetSumnumber : " + planetSumNumber);
        if(planetSumNumber <= TutorialGM.GameSetUp.practicePlanet + 1) {
            return;
        }
        else {
            //float distance = Vector3.Distance(p1.transform.position, p2.transform.position) - p1.pathHeight - p2.pathHeight;
            int obstacleNumber = Random.Range(1, 3);
            if(planetSumNumber == TutorialGM.GameSetUp.createProportion[0] + 1) {
                Debug.Log("create aerolites");
                TutorialGM.AeroliteManager.CreateAerolitePlus(p1, p2);
                return;
            }
            if(planetSumNumber < TutorialGM.GameSetUp.createProportion[0] + 1) {
                int[ ] tempArray = Tools.RandomInt(obstacleNumber);
                for(int j = 0; j < tempArray.Length; j++) {
                    //TutorialGM.TreasureManager.CreateTreasure(p1, p2, tempArray[j]);
                }
            }
            else {
                int tempInt = Random.Range(0, 2);
                int[ ] tempArray = Tools.RandomInt(obstacleNumber);
                for(int j = 0; j < tempArray.Length; j++) {
                    //TutorialGM.TreasureManager.CreateTreasure(p1, p2, tempArray[j]);
                }
                //GM.MeteorManager.CreateMeteor(p1, p2, 0.5f);
                if(tempInt == 0) {
                    TutorialGM.AeroliteManager.CreateAerolitePlus(p1, p2);
                }
            }
        }
    }

    public Sprite ChoicePlanetSprite( int temp )
    {
        Sprite sp;
        if(temp >= 0 && temp <= 1) {
            sp = TutorialGM.SpriteManager.planetSprite2[Random.Range(0, TutorialGM.SpriteManager.planetSprite2.Length - 1)];
        }
        else {
            sp = TutorialGM.SpriteManager.planetSprite1[Random.Range(0, TutorialGM.SpriteManager.planetSprite1.Length - 1)];
        }
        return sp;
    }

    public bool CheckShipRotateDirection( Vector2 v1, Vector2 v2 )
    {
        bool check = false;
        if(Vector2.Dot(v1, v2) < 0) {
            check = false;
        }
        else {
            check = true;
        }
        return check;
    }
}
