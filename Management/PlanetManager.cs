using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour {

    public const float leftWidth = -4.5f;
    public const float rightWidth = 4.5f;
    public List<Planet> planetList = new List<Planet>( );
    public SpriteRenderer[ ] Planets;
    public Transform planetParent;
    public int stepNumber;
    public int planetSumNumber;

    public Planet firstPlanet;
    public Planet secondPlanet;
    public Sprite earth;
    private SpriteRenderer[ ] startPlanet;

    private const float fixedSpace = 0.1f;
    private const int bufferPlanet = 3;
    public int rotateDirection;

    public void SetFirstPlanet( )
    {
        GM.GameSetUp.motiomMode = MotionMode.Turn;
        SpaceShip.currentPlanet = firstPlanet;
        planetList.Add(firstPlanet);
        //firstPlanet.pathPosition.localPosition = new Vector3(firstPlanet.pathHeight, 0, 0);
        //SpaceShip.spaceShip.transform.parent = firstPlanet.pathPosition;

        firstPlanet.rotation = firstPlanet.gameObject.transform.FindChild("rotation");
        firstPlanet.rotation.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);

        SpaceShip.spaceShip.transform.parent = firstPlanet.rotation;
        SpaceShip.spaceShip.transform.localPosition = new Vector3(firstPlanet.pathHeight, 0, 0);

        rotateDirection = Random.Range(0, 2);

        if(rotateDirection == 0) {
            firstPlanet.rotateMode = Planet.TurnMode.Clockwise;
            Vector3 vectorToTarget = (SpaceShip.spaceShip.transform.position - firstPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
            SpaceShip.spaceShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else {
            firstPlanet.rotateMode = Planet.TurnMode.Counterclockwise;
            Vector3 vectorToTarget = (SpaceShip.spaceShip.transform.position - firstPlanet.transform.position) - transform.right * 0.1f;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            SpaceShip.spaceShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void SetSecondPlanet( )
    {
        SpriteRenderer sr = Instantiate(Planets[4]) as SpriteRenderer;
        //srArray[i].sprite = planetSprite[Random.Range(0, planetSprite.Length)];
        secondPlanet = sr.GetComponent<Planet>( );
        sr.transform.parent = planetParent;
        sr.transform.localPosition = new Vector3(0, 3.06f, 0);
        //p.satelliteObject.SetActive(false);
        planetSumNumber++;
        secondPlanet.planetNumber = planetSumNumber;
        planetList.Add(secondPlanet);
    }

    void Start( )
    {
        //    SetFirstPlanet( );
        //    SetSecondPlanet( );
    }
    public void PrepareToStartGame( int pn )
    {
        //Debug.Log("prepare to start");
        SpriteRenderer[ ] srArray = new SpriteRenderer[pn];
        for(int i = 0; i < pn; i++) {
            if(i >= 0) {
                srArray[i] = Instantiate(SetPlanetDifficulty(GM.GameSetUp.createProportion)) as SpriteRenderer;
                Planet p1;
                Planet p2 = srArray[i].GetComponent<Planet>( );
                srArray[i].transform.parent = planetParent;
                if(i == 0) {
                    p1 = secondPlanet;
                }
                else {
                    p1 = srArray[i - 1].GetComponent<Planet>( );
                }
                srArray[i].transform.localPosition = new Vector3(Random.Range(leftWidth + fixedSpace + p2.pathHeight, rightWidth - fixedSpace - p2.pathHeight),
                             p1.transform.localPosition.y + p2.pathHeight + p1.pathHeight + 1.25f, 0);
                //srArray[i].transform.localPosition = new Vector3(Random.Range(leftWidth + fixedSpace + p2.pathHeight, rightWidth - fixedSpace - p2.pathHeight), 
                //                                                 srArray[i - 1].transform.localPosition.y + p2.pathHeight + p1.pathHeight + 2.0f, 0);
                //Random.Range(0.85f, 3.50f)
                planetSumNumber++;
                p2.planetNumber = planetSumNumber;
                planetList.Add(p2);
                SetPathObject(p1, p2);

                if(!GameSetUp.isOpened) {
                    srArray[i].gameObject.SetActive(false);
                }
            }
        }
        startPlanet = srArray;
    }

    public void ActiveStartPlanet( )
    {
        Debug.Log("startPlanet.Length : " + startPlanet.Length);
        for(int i = 0; i < startPlanet.Length; i++) {
            startPlanet[i].gameObject.SetActive(true);
        }
    }

    void SetPathObject( Planet p1, Planet p2 )
    {
        Debug.Log("planetSumnumber : " + planetSumNumber);
        if(planetSumNumber <= GM.GameSetUp.practicePlanet + 1) {
            return;
        }
        else {
            //float distance = Vector3.Distance(p1.transform.position, p2.transform.position) - p1.pathHeight - p2.pathHeight;
            int obstacleNumber = Random.Range(1, 3);
            if(planetSumNumber == GM.GameSetUp.createProportion[0] + 1) {
                Debug.Log("create aerolites");
                GM.AeroliteManager.CreateAerolitePlus(p1, p2);
                return;
            }
            if(planetSumNumber < GM.GameSetUp.createProportion[0] + 1) {
                int[ ] tempArray = Tools.RandomInt(obstacleNumber);
                for(int j = 0; j < tempArray.Length; j++) {
                    GM.TreasureManager.CreateTreasure(p1, p2, tempArray[j]);
                }
            }
            else {
                int tempInt = Random.Range(0, 2);
                int[ ] tempArray = Tools.RandomInt(obstacleNumber);
                for(int j = 0; j < tempArray.Length; j++) {
                    GM.TreasureManager.CreateTreasure(p1, p2, tempArray[j]);
                }
                float tempFloat = Random.Range(1.5f, 2.5f);
                //GM.MeteorManager.CreateMeteor(p1, p2, 0.5f);
                if(tempInt == 0) {
                    GM.AeroliteManager.CreateAerolitePlus(p1, p2);
                }
                else {
                    GM.MeteorManager.CreateMeteor(p1, p2, tempFloat);
                }
            }
        }
    }

    public int SetPlanetVolume( int[ ] intArray )
    {
        int intTemp = 0, intSum = 0, temp = 0;
        for(int i = 0; i < intArray.Length; i++) {
            intSum += intArray[i];
        }
        temp = Random.Range(0, intSum);
        for(int i = 0; i < intArray.Length; i++) {
            int step1 = 0, step2 = 0;

            for(int j = 1; j < i + 1; j++) {
                step1 += intArray[j - 1];
            }
            for(int j = 0; j < i + 1; j++) {
                step2 += intArray[j];
            }

            if(temp >= step1 && temp < step2) {
                intTemp = i;
                //Debug.Log("temp step1, step2 : " + temp + " , " + step1 + " , " + step2);
                //Debug.Log("i : " + i);
            }
            else {
                continue;
            }
        }
        return intTemp;
    }

    public SpriteRenderer SetPlanetDifficulty( int[ ] intArray )
    {
        SpriteRenderer sr = Planets[0];
        int temp = 0;

        for(int i = 0; i < intArray.Length; i++) {
            int temp1 = 0, temp2 = intArray[i];
            if(i > 0) {
                temp1 = intArray[i - 1];
            }
            else {
                temp1 = 0;
            }

            if(planetSumNumber >= temp1 && planetSumNumber < temp2) {
                int tempInt = SetPlanetVolume(GM.GameSetUp.stepLevel[i]);
                sr = Planets[tempInt];
                sr.sprite = ChoicePlanetSprite(tempInt);
                temp++;
            }
            else {
                continue;
            }
        }
        //Debug.Log("intArray : " + intArray.Length);
        if(temp == 0 && planetSumNumber >= intArray[intArray.Length - 1]) {
            Debug.Log("over 50");
            int tempInt = SetPlanetVolume(GM.GameSetUp.stepLevel[intArray.Length]);
            sr = Planets[tempInt];
            sr.sprite = ChoicePlanetSprite(tempInt);
            //sr = Planets[SetPlanetVolume(stepLevel[intArray.Length])];
        }

        return sr;
    }

    public Sprite ChoicePlanetSprite( int temp )
    {
        Sprite sp;
        if(temp >= 0 && temp <= 1) {
            sp = GM.SpriteManager.planetSprite2[Random.Range(0, GM.SpriteManager.planetSprite2.Length - 1)];
        }
        else {
            sp = GM.SpriteManager.planetSprite1[Random.Range(0, GM.SpriteManager.planetSprite1.Length - 1)];
        }
        return sp;
    }

    public void AddNewPlanet( Planet targetP )
    {
        int temp = 0;
        if(planetSumNumber - targetP.planetNumber < bufferPlanet) {
            temp = bufferPlanet - (planetSumNumber - targetP.planetNumber);
        }
        for(int i = 0; i < temp; i++) {
            Planet[ ] tempArray = planetList.ToArray( );
            SpriteRenderer sr = Instantiate(SetPlanetDifficulty(GM.GameSetUp.createProportion)) as SpriteRenderer;
            //sr.sprite = planetSprite[Random.Range(0, planetSprite.Length)];
            Planet p1 = tempArray[tempArray.Length - 1];
            Planet p2 = sr.GetComponent<Planet>( );
            SatelliteManager sm = sr.GetComponent<SatelliteManager>( );
            sm.SetSatelliteDifficulty(p2);
            //SetSatelliteDifficulty(p2);
            sr.transform.parent = planetParent;
            sr.transform.localPosition = new Vector3(Random.Range(leftWidth + fixedSpace + p2.pathHeight, rightWidth - fixedSpace - p2.pathHeight),
                                                     tempArray[tempArray.Length - 1].transform.localPosition.y + p1.pathHeight + p2.pathHeight + SetDistanceDifficulty( ), 0);
            planetSumNumber++;
            p2.planetNumber = planetSumNumber;
            planetList.Add(p2);
            SetPathObject(tempArray[tempArray.Length - 1], p2);
        }
    }

    public float SetDistanceDifficulty( )
    {
        float temp;
        float[ ] tempFloat;
        if(planetSumNumber < GM.GameSetUp.createProportion[GM.GameSetUp.createProportion.Length - 1]) {
            tempFloat = new float[ ] { 3.50f, 5.00f };
        }
        else {
            tempFloat = new float[ ] { 5.25f, 6.25f };
        }
        temp = Random.Range(tempFloat[0], tempFloat[1]);
        return temp;
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


//private int AeroliteDifficulty( )
//{
//    int temp = 0;
//    int[ ] intArray = new int[2];
//    if(planetSumNumber >= GM.GameSetUp.createProportion[0] && planetSumNumber < GM.GameSetUp.createProportion[1]) {
//        intArray = new int[2] { 9, 15 };
//    }
//    else if(planetSumNumber >= GM.GameSetUp.createProportion[1] && planetSumNumber < GM.GameSetUp.createProportion[2]) {
//        intArray = new int[2] { 9, 18 };
//    }
//    else if(planetSumNumber >= GM.GameSetUp.createProportion[2] && planetSumNumber < GM.GameSetUp.createProportion[3]) {
//        intArray = new int[2] { 12, 21 };
//    }
//    else if(planetSumNumber >= GM.GameSetUp.createProportion[3] && planetSumNumber < GM.GameSetUp.createProportion[4]) {
//        intArray = new int[2] { 12, 24 };
//    }
//    else {
//        intArray = new int[2] { 15, 30 };
//    }
//    temp = Random.Range(intArray[0], intArray[1]);
//    //Debug.Log("aerolite number : " + temp);
//    return temp;
//}

//void SetSatelliteDifficulty( Planet p )
//{
//    int[ ] tempArray = new int[GM.GameSetUp.createProportion.Length - 1];
//    int temp = Random.Range(1, 11);
//    SatelliteManager sl = p.GetComponent<SatelliteManager>( );

//    for(int i = 0; i < tempArray.Length; i++) {
//        tempArray[i] = GM.GameSetUp.createProportion[i + 1];
//    }
//    if(planetSumNumber < tempArray[0])
//        return;
//    else if(planetSumNumber == tempArray[0]) {
//        sl.SetSatellite(1, p);
//    }
//    else if(planetSumNumber > tempArray[0] && planetSumNumber < tempArray[1]) {
//        if(temp > 0 && temp <= 4) {
//            return;
//        }
//        else {
//            sl.SetSatellite(1, p);
//        }
//    }
//    else if(planetSumNumber >= tempArray[1] && planetSumNumber < tempArray[2]) {
//        if(temp > 0 && temp <= 2) {
//            return;
//        }
//        else if(temp > 2 && temp <= 6) {
//            sl.SetSatellite(1, p);
//        }
//        else {
//            sl.SetSatellite(2, p);
//        }
//    }
//    else if(planetSumNumber >= tempArray[2] && planetSumNumber < tempArray[3]) {
//        if(temp > 0 && temp <= 1) {
//            return;
//        }
//        else if(temp > 1 && temp <= 4) {
//            sl.SetSatellite(1, p);
//        }
//        else if(temp > 4 && temp <= 7) {
//            sl.SetSatellite(2, p);
//        }
//        else {
//            sl.SetSatellite(3, p);
//        }
//    }
//    else {
//        if(temp > 0 && temp <= 2) {
//            sl.SetSatellite(1, p);
//        }
//        else if(temp > 2 && temp <= 6) {
//            sl.SetSatellite(2, p);
//        }
//        else {
//            sl.SetSatellite(3, p);
//        }
//    }
//}
