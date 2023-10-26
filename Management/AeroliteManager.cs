using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AeroliteManager : MonoBehaviour {

    public Queue<SpriteRenderer> aerolitePool = new Queue<SpriteRenderer>( );
    public Queue<SpriteRenderer> meteorspritePool = new Queue<SpriteRenderer>( );
    public Transform aeroliteParent;
    public SpriteRenderer[ ] aerolite;
    public SpriteRenderer[ ] meteor;

    public const float left = -4.5f;
    public const float right = 4.5f;

    public void CreateAerolitePlus( Planet p1, Planet p2) {
        Debug.Log("CreateAerolitePlus");
        Vector3 centerPoint, tempVector, centerVector, normalVector, tempPoint1, tempPoint2, randomPosition1, randomPosition2;
        float fixedDistance = 0.6f, distance1 = 0, distance2 = 0;

        tempVector = (p2.transform.position - p1.transform.position).normalized;
        tempPoint1 = p1.transform.position + (tempVector * p1.pathHeight);
        tempPoint2 = p1.transform.position + (tempVector * (Vector3.Distance(p1.transform.position, p2.transform.position) - p2.pathHeight));

        centerPoint = (tempPoint1 + tempPoint2) / 2;
        centerVector = (p2.transform.position - centerPoint).normalized;
        normalVector = Quaternion.AngleAxis(90, Vector3.forward) * centerVector.normalized;

        for(int i = 0; i < AeroliteDifficulty( ); i++) {
            SpriteRenderer sr;
            int temp = Random.Range(0, 10);
            if(aerolitePool.Count > 0) {
                sr = aerolitePool.Dequeue( );
            }
            else {
                if(temp >= 1) {
                    sr = Instantiate(aerolite[Random.Range(0, aerolite.Length - 3)]);
                }
                else {
                    sr = Instantiate(aerolite[Random.Range(aerolite.Length - 3, aerolite.Length)]);
                }
            }

            distance1 = (Vector3.Distance(p1.transform.position, p2.transform.position) - p1.pathHeight - p2.pathHeight) / 2 - fixedDistance - sr.GetComponent<Aerolite>().aeroliteRadius;
            distance2 = Vector3.Distance(p1.transform.position, p2.transform.position) / 2;

            sr.gameObject.SetActive(true);
            sr.transform.parent = GM.AeroliteManager.aeroliteParent;
            sr.transform.localScale = new Vector3(1, 1, 0);
            randomPosition1 = centerPoint + (centerVector * Random.Range(-distance1, distance1));
            randomPosition2 = randomPosition1 + (normalVector * Random.Range(-distance2, distance2));
            sr.transform.position = randomPosition2;
            sr.tag = "Aerolite";
        }
    }

    private int AeroliteDifficulty( )
    {
        int temp = 0;
        int[ ] intArray = new int[2];

        if(GM.PlanetManager.planetSumNumber >= GM.GameSetUp.createProportion[0] && GM.PlanetManager.planetSumNumber < GM.GameSetUp.createProportion[1]) {
            intArray = GM.GameSetUp.aeroliteLevel[0];
        }
        else if(GM.PlanetManager.planetSumNumber >= GM.GameSetUp.createProportion[1] && GM.PlanetManager.planetSumNumber < GM.GameSetUp.createProportion[2]) {
            intArray = GM.GameSetUp.aeroliteLevel[1];
        }
        else if(GM.PlanetManager.planetSumNumber >= GM.GameSetUp.createProportion[2] && GM.PlanetManager.planetSumNumber < GM.GameSetUp.createProportion[3]) {
            intArray = GM.GameSetUp.aeroliteLevel[2];
        }
        else if(GM.PlanetManager.planetSumNumber >= GM.GameSetUp.createProportion[3] && GM.PlanetManager.planetSumNumber < GM.GameSetUp.createProportion[4]) {
            intArray = GM.GameSetUp.aeroliteLevel[3];
        }
        else {
            intArray = GM.GameSetUp.aeroliteLevel[4];
        }
        temp = Random.Range(intArray[0], intArray[1]);
        return temp;
    }

    public void RecoverAerolite(SpriteRenderer sr)
    {
        if(sr.tag == "Aerolite") {
            aerolitePool.Enqueue(sr);
            sr.GetComponent<Aerolite>( ).isRotate = false;
            sr.gameObject.SetActive(false);
        }
        if(sr.tag == "Meteor") {
            //meteorspritePool.Enqueue(sr);
            //sr.GetComponent<Aerolite>( ).isRotate = false;
            //sr.gameObject.SetActive(false);
            GM.MeteorManager.RecoverMeteor(sr.GetComponentInParent<Meteor>(), sr);
        }
    }

    public void OutOfCamera( SpriteRenderer sr )
    {
        if(sr.tag == "Aerolite") {
            float distance = GM.CameraManager.mainCamera.transform.position.y - sr.transform.position.y;
            if(distance >= 10 && sr.transform.position.y < SpaceShip.spaceShip.transform.position.y) {
                Aerolite al = sr.GetComponent<Aerolite>( );
                al.isRotate = false;
                sr.gameObject.SetActive(false);
                aerolitePool.Enqueue(sr);
            }
        }
        //if(sr.tag == "ShipWreckage") {
        //    float distance = GM.CameraManager.mainCamera.transform.position.y - sr.transform.position.y;
        //    if(distance >= 10 && sr.transform.position.y < SpaceShip.spaceShip.transform.position.y) {
        //        sr.gameObject.SetActive(false);
        //        shipWreckagePool.Enqueue(sr);
        //    }
        //}
    }

    //public void CreateAeroliteIntegrate( Planet p1, Planet p2, int randomNumber, float timer )
    //{
    //    int temp = Random.Range(0, 2);
    //    if(temp == 1) {
    //        CreateAerolitePlus(p1, p2);
    //    }
    //    else {
    //        CreateMeteor(p1, p2, randomNumber, timer);
    //    }
    //}

    //public void CreateAerolite(Planet p1, Planet p2, int randomNumber, float concentration )
    //{
    //    //Debug.Log("CreateAerolite");
    //    float fixedY = 0.65f,
    //        minY = p1.transform.position.y + p1.pathHeight + fixedY,
    //        maxY = p2.transform.position.y - p2.pathHeight - fixedY,
    //        minX = left + concentration,
    //        maxX = right - concentration,
    //        center = Random.Range(left + 1.75f, right - 1.75f);
    //    //if(p1.GetComponent<SatelliteManager>( ).isSatellite) {
    //    //    minY = minY + p1.GetComponent<SatelliteManager>( ).fixedsatelliteDistance;
    //    //}
    //    //if(p2.GetComponent<SatelliteManager>( ).isSatellite) {
    //    //    maxY = maxY - p2.GetComponent<SatelliteManager>( ).fixedsatelliteDistance;
    //    //}
    //    for(int i = 0; i < randomNumber; i++) {
    //        float tempX = Random.Range(minX, maxX),
    //              tempY = Random.Range(minY, maxY);
    //        SpriteRenderer sr;
    //        if(aerolitePool.Count > 0) {
    //            sr = aerolitePool.Dequeue( );
    //        }
    //        else {
    //            sr = Instantiate(aerolite[Random.Range(0, aerolite.Length)]);
    //        }
    //        sr.gameObject.SetActive(true);
    //        sr.transform.parent = GM.AeroliteManager.aeroliteParent;
    //        sr.transform.localScale = new Vector3(1, 1, 0);
    //        sr.transform.position = new Vector3(center + tempX, tempY, 0);
    //    }
    //}

    //public void CreateMeteor( Planet p1, Planet p2, int randomNumber, float timer )
    //{
    //    Debug.Log("CreatMeteor");
    //    MeteorSystem ms;
    //    if(meteorPool.Count > 0) {
    //        ms = meteorPool.Dequeue( );
    //    }
    //    else {
    //        ms = Instantiate(meteorSystem) as MeteorSystem;
    //    }
    //    ms.transform.parent = meteorParent;
    //    ms.planet1 = p1;
    //    ms.planet2 = p2;
    //    ms.meteorNumber = randomNumber;
    //    ms.meteorFrequency = timer;
    //}

    //float x = Mathf.Abs(p2.transform.position.x - p1.transform.position.x);
    //float y = Mathf.Abs(p2.transform.position.y - p1.transform.position.y);
    //if(y > x) {
    //    CreateAerolite(p1, p2, randomNumber, concentration);
    //}
    //else {
    //    CreateAerolitePlus(p1, p2, randomNumber);
    //}

    //public void CreatExplosion( Vector3 pos )
    //{
    //    ParticleSystem ps = Instantiate(explosion) as ParticleSystem;
    //    ps.transform.position = pos;
    //    ps.transform.parent = particleParent;
    //}

    //float distance = Vector3.Distance(p1.transform.position, p2.transform.position);
    //randomNumber = Random.Range(15, 35);
    //if(distance < 7.5) {
    //    Debug.Log("too short and don't create aerolite");
    //    return;
    //}
    //else {
    //}
}
