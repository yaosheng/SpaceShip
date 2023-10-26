using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorSystem : MonoBehaviour {

    public Planet planet1;
    public Planet planet2;
    public int flyType;
    public Transform testTransform;
    public float meteorFrequency = 0.5f;
    private float timer = 0;
    public const float minX = -5.75f;
    public const float maxX = 5.75f;
    //public float tempFloat;
    private const float fixedDistance = 0.35f;

	void Update () {
        if(planet1 != null && planet2 != null) {
            if(Mathf.Abs(GM.PlanetManager.stepNumber - planet1.planetNumber) <= 1) {
                //SetMeteorTest( );
                if(timer > meteorFrequency) {
                    SetMeteor();
                    timer = 0;
                }
                else {
                    timer += Time.deltaTime;
                }
            }
            if(GM.PlanetManager.stepNumber - planet2.planetNumber > 0) {
                gameObject.SetActive(false);
            }
        }
    }

    void SetMeteor()
    {
        //yield return new WaitForSeconds(meteorFrequency);
        //Debug.Log("create meteor");
        Meteor mo;
        if(GM.MeteorManager.meteorPool.Count > 0) {
            mo = GM.MeteorManager.meteorPool.Dequeue( );
        }
        else {
            mo = Instantiate(GM.MeteorManager.meteor) as Meteor;
        }
        mo.gameObject.SetActive(true);
        mo.transform.parent = GM.MeteorManager.meteorParent;

        SpriteRenderer sr;
        if(GM.AeroliteManager.meteorspritePool.Count > 0) {
            sr = GM.AeroliteManager.meteorspritePool.Dequeue( );
        }
        else {
            sr = Instantiate(GM.AeroliteManager.meteor[Random.Range(0, GM.AeroliteManager.meteor.Length)]) as SpriteRenderer;
        }
        sr.gameObject.SetActive(true);
        sr.transform.parent = mo.transform;
        sr.transform.localPosition = Vector3.zero;
        mo.GetComponent<Meteor>( ).child = sr;
        sr.tag = "Meteor";

        Aerolite al = sr.GetComponent<Aerolite>( );
        al.isRotate = true;
        RandomMeteorType(mo, al);
    }

    void RandomMeteorType( Meteor mo, Aerolite al)
    {
        float testY = 0;
        switch(flyType) {
            case 0:
                testY = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + fixedDistance, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - fixedDistance);
                mo.transform.position = new Vector3(-5.5f, testY, 0);
                mo.transform.up = transform.right;

                break;
            case 1:
                testY = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + fixedDistance, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - fixedDistance);
                mo.transform.position = new Vector3(5.5f, testY, 0);
                mo.transform.up = -transform.right;
                break;
            case 2:
                int temp1 = Random.Range(0, 2);
                if(temp1 == 1) {
                    testY = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + fixedDistance, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - fixedDistance);
                    mo.transform.position = new Vector3(-5.5f, testY, 0);
                    mo.transform.up = transform.right;
                }
                else {
                    testY = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + fixedDistance, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - fixedDistance);
                    mo.transform.position = new Vector3(5.5f, testY, 0);
                    mo.transform.up = -transform.right;
                }
                break;
            case 3:
                mo.transform.position = SetStartPosition(0);
                mo.transform.up = SetRandomVector(0, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
            case 4:
                mo.transform.position = SetStartPosition(1);
                mo.transform.up = SetRandomVector(1, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
            case 5:
                int tempInt = Random.Range(0, 2);
                mo.transform.position = SetStartPosition(tempInt);
                mo.transform.up = SetRandomVector(tempInt, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
            case 6:
                SetStableVectorAndRandomPosition(0, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
            case 7:
                SetStableVectorAndRandomPosition(1, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
            case 8:
                int tempInt1 = Random.Range(0, 2);
                SetStableVectorAndRandomPosition(tempInt1, mo.transform, al.aeroliteRadius + fixedDistance);
                break;
        }
    }

    Vector3 SetRandomVector(int side, Transform trans , float disc)
    {
        Vector3 vector1 = planet1.transform.position - trans.position;
        Vector3 vector2 = planet2.transform.position - trans.position;
        float distance1 = Vector3.Distance(planet1.transform.position, trans.position);
        float distance2 = Vector3.Distance(planet2.transform.position, trans.position);
        float angle1 = 0, angle2 = 0, angle3 = 0;
        Vector3 v1, v2, v3;

        if(side == 0) {
            angle1 = - Mathf.Asin((planet1.pathHeight + disc) / distance1) * Mathf.Rad2Deg;
            angle2 = Mathf.Asin((planet2.pathHeight + disc) / distance2) * Mathf.Rad2Deg;
            v1 = Quaternion.AngleAxis(angle1, Vector3.forward) * vector1;
            v2 = Quaternion.AngleAxis(angle2, Vector3.forward) * vector2;
            angle3 = Vector3.Angle(vector1, vector2);
            float temp = Mathf.Abs(angle3) - Mathf.Abs(angle1) - Mathf.Abs(angle2);
            v3 = Quaternion.AngleAxis(-Random.Range(0, temp), Vector3.forward) * v1;
        }
        else {
            angle1 = Mathf.Asin((planet1.pathHeight + disc) / distance1) * Mathf.Rad2Deg;
            angle2 = - Mathf.Asin((planet2.pathHeight + disc) / distance2) * Mathf.Rad2Deg;
            v1 = Quaternion.AngleAxis(angle1, Vector3.forward) * vector1;
            v2 = Quaternion.AngleAxis(angle2, Vector3.forward) * vector2;
            angle3 = Vector3.Angle(vector1, vector2);
            float temp = Mathf.Abs(angle3) - Mathf.Abs(angle1) - Mathf.Abs(angle2);
            v3 = Quaternion.AngleAxis(Random.Range(0, temp), Vector3.forward) * v1;
        }
        return v3;
    }

    void SetStableVectorAndRandomPosition( int side, Transform trans, float disc )
    {
        Vector3 tempVector, randomVector, normalVector, randomPoint;
        float tempFloat = 0, tempDistance1 = 0, tempDistance2 = 0, finalDistance = 0, angle = 0;

        tempVector = (planet2.transform.position - planet1.transform.position).normalized;
        angle = 90 - Vector3.Angle(Vector3.right, tempVector);
        tempFloat = Random.Range(planet1.pathHeight + disc, Vector3.Distance(planet1.transform.position, planet2.transform.position) - planet2.pathHeight - disc);

        randomPoint = planet1.transform.position + (tempVector * tempFloat);
        randomVector = (planet2.transform.position - randomPoint).normalized;

        tempDistance1 = maxX - randomPoint.x;
        tempDistance2 = randomPoint.x - minX;

        if(side == 0) {
            normalVector = Quaternion.AngleAxis(90, Vector3.forward) * randomVector.normalized;
            trans.up = normalVector;
            finalDistance = Mathf.Abs(Mathf.Tan(Mathf.PI / (180 / angle))) * tempDistance1;
            if(planet2.transform.position.x >= planet1.transform.position.x) {
                trans.position = new Vector3(maxX, randomPoint.y - finalDistance, 0);
            }
            else {
                trans.position = new Vector3(maxX, randomPoint.y + finalDistance, 0);
            }
        }
        else {
            normalVector = Quaternion.AngleAxis(-90, Vector3.forward) * randomVector.normalized;
            trans.up = normalVector;
            finalDistance = Mathf.Abs(Mathf.Tan(Mathf.PI / (180 / angle))) * tempDistance2;
            if(planet2.transform.position.x >= planet1.transform.position.x) {
                trans.position = new Vector3(minX, randomPoint.y + finalDistance, 0);
            }
            else {
                trans.position = new Vector3(minX, randomPoint.y - finalDistance, 0);
            }
        }
    }

    Vector3 SetStartPosition(int side)
    {
        Vector3 finalPos;
        float minY = 0, maxY = 0;
        //int tempInt = Random.Range(0, 2);
        if(Mathf.Abs(planet1.transform.position.x - planet2.transform.position.x) <= 1.5f) {
            minY = planet1.transform.position.y;
            maxY = planet2.transform.position.y;
            if(side == 0) {
                finalPos = new Vector3(maxX, Random.Range(minY, maxY), 0);
            }
            else {
                finalPos = new Vector3(minX, Random.Range(minY, maxY), 0);
            }

        }
        else {
            if(side == 0) {
                if(planet1.transform.position.x >= planet2.transform.position.x) {
                    minY = planet1.transform.position.y + planet1.pathHeight;
                    maxY = planet2.transform.position.y + planet2.pathHeight;
                }
                else {
                    minY = planet1.transform.position.y - planet1.pathHeight;
                    maxY = planet2.transform.position.y - planet2.pathHeight;
                }
                finalPos = new Vector3(maxX, Random.Range(minY, maxY), 0);
            }
            else {
                if(planet1.transform.position.x >= planet2.transform.position.x) {
                    minY = planet1.transform.position.y - planet1.pathHeight;
                    maxY = planet2.transform.position.y - planet2.pathHeight;
                }
                else {
                    minY = planet1.transform.position.y + planet1.pathHeight;
                    maxY = planet2.transform.position.y + planet2.pathHeight;
                }
                finalPos = new Vector3(minX, Random.Range(minY, maxY), 0);
            }
        }
        return finalPos;
    }

    void SetMeteorTest( )
    {
        Debug.Log("create meteor");
        Meteor mo;
        if(GM.MeteorManager.meteorPool.Count > 0) {
            mo = GM.MeteorManager.meteorPool.Dequeue( );
        }
        else {
            mo = Instantiate(GM.MeteorManager.meteor) as Meteor;
        }
        mo.gameObject.SetActive(true);
        mo.transform.parent = GM.MeteorManager.meteorParent;

        SpriteRenderer sr;
        if(GM.AeroliteManager.aerolitePool.Count > 0) {
            sr = GM.AeroliteManager.aerolitePool.Dequeue( );
        }
        else {
            sr = Instantiate(GM.AeroliteManager.aerolite[Random.Range(0, GM.AeroliteManager.aerolite.Length)]) as SpriteRenderer;
        }
        sr.gameObject.SetActive(true);
        sr.transform.parent = mo.transform;
        sr.transform.localPosition = Vector3.zero;
        sr.tag = "Meteor";
        mo.GetComponent<Meteor>( ).child = sr;

        Aerolite al = sr.GetComponent<Aerolite>( );
        al.isRotate = true;

        //mo.transform.up = SetVector(0, testTransform, al.aeroliteRadius + 0.3f);
        mo.transform.up = SetVectorTest(0, testTransform);
    }

    Vector3 SetVectorTest( int side, Transform trans)
    {
        Vector3 vector1 = planet1.transform.position - trans.position;
        Vector3 vector2 = planet2.transform.position - trans.position;
        float distance1 = Vector3.Distance(planet1.transform.position, trans.position);
        float distance2 = Vector3.Distance(planet2.transform.position, trans.position);
        float angle1 = 0, angle2 = 0, angle3 = 0;
        Vector3 v1, v2, v3;

        //if(side == 0) { 
        if(trans.position.x >= 0) {
            angle1 = - Mathf.Asin((planet1.pathHeight) / distance1) * Mathf.Rad2Deg;
            angle2 = Mathf.Asin((planet2.pathHeight) / distance2) * Mathf.Rad2Deg;

            v1 = Quaternion.AngleAxis(angle1, Vector3.forward) * vector1;
            v2 = Quaternion.AngleAxis(angle2, Vector3.forward) * vector2;
            angle3 = Vector3.Angle(vector1, vector2);
            float temp = Mathf.Abs(angle3) - Mathf.Abs(angle1) - Mathf.Abs(angle2);
            v3 = Quaternion.AngleAxis(-Random.Range(0, temp), Vector3.forward) * v1;

        }
        else {
            angle1 = Mathf.Asin((planet1.pathHeight) / distance1) * Mathf.Rad2Deg;
            angle2 = - Mathf.Asin((planet2.pathHeight) / distance2) * Mathf.Rad2Deg;
            v1 = Quaternion.AngleAxis(angle1, Vector3.forward) * vector1;
            v2 = Quaternion.AngleAxis(angle2, Vector3.forward) * vector2;
            angle3 = Vector3.Angle(vector1, vector2);
            float temp = Mathf.Abs(angle3) - Mathf.Abs(angle1) - Mathf.Abs(angle2);
            v3 = Quaternion.AngleAxis(Random.Range(0, temp), Vector3.forward) * v1;

        }

        Debug.DrawRay(trans.position, v1 * 100, Color.red);
        Debug.DrawRay(trans.position, v2 * 100, Color.green);
        Debug.DrawRay(trans.position, v3 * 100, Color.yellow);
        Debug.Log("angle1 : " + angle1);
        Debug.Log("angle2 : " + angle2);
        Debug.Log("angle3 : " + angle3);
        return v3;
    }

    //switch(temp) {
    //    case 0:
    //        float test1Y = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + 0.35f, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - 0.35f);
    //        mo.transform.position = new Vector3(5.5f, test1Y, 0);
    //        mo.transform.up = transform.right;
    //        break;
    //    case 1:
    //        float test2Y = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + 0.35f, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - 0.35f);
    //        mo.transform.position = new Vector3(-5.5f, test2Y, 0);
    //        mo.transform.up = -transform.right;
    //        break;
    //    case 2:
    //        int temp1 = Random.Range(0, 2);
    //        if(temp1 == 1) {
    //            float test3Y = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + 0.35f, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - 0.35f);
    //            mo.transform.position = new Vector3(5.5f, test3Y, 0);
    //            mo.transform.up = transform.right;
    //        }
    //        else {
    //            float test4Y = Random.Range(planet1.transform.position.y + planet1.pathHeight + al.aeroliteRadius + 0.35f, planet2.transform.position.y - planet2.pathHeight - al.aeroliteRadius - 0.35f);
    //            mo.transform.position = new Vector3(-5.5f, test4Y, 0);
    //            mo.transform.up = -transform.right;
    //        }
    //        break;
    //    case 3:

    //        break;
    //    case 4:

    //        break;
    //    case 5:

    //        break;
    //    case 6:
    //        mo.transform.position = SetStartPosition(0);
    //        mo.transform.up = SetVector(mo.transform, al.aeroliteRadius + 0.3f);
    //        break;
    //    case 7:
    //        mo.transform.position = SetStartPosition(1);
    //        mo.transform.up = SetVector(mo.transform, al.aeroliteRadius + 0.3f);
    //        break;
    //    case 8:
    //        mo.transform.position = SetStartPosition(Random.Range(0, 2));
    //        mo.transform.up = SetVector(mo.transform, al.aeroliteRadius + 0.3f);
    //        break;
    //}

    //Debug.DrawRay(trans.position, v1 * 100, Color.red);
    //Debug.DrawRay(trans.position, v2 * 100, Color.green);
    //Debug.DrawRay(trans.position, v3 * 100, Color.yellow);
    //Vector3 v1 = Quaternion.AngleAxis(angle1, Vector3.forward) * vector1;
    //Vector3 v2 = Quaternion.AngleAxis(angle2, Vector3.forward) * vector2;
    //float temp = Random.Range(0.00f, Vector3.Angle(v1, v2));
    //angle3 = Vector3.Angle(v1, v2);

    //if(timer < meteorFrequency) {
    //    timer += Time.deltaTime;
    //    if(!isCreated) {
    //        Debug.Log("create meteor");
    //        Meteor mo;
    //        if(GM.MeteorManager.meteorPool.Count > 0) {
    //            mo = GM.MeteorManager.meteorPool.Dequeue( );
    //        }
    //        else {
    //            mo = Instantiate(GM.MeteorManager.meteor) as Meteor;
    //        }
    //        mo.gameObject.SetActive(true);
    //        mo.transform.parent = GM.MeteorManager.meteorParent;
    //        mo.transform.position = SetStartPosition( );

    //        SpriteRenderer sr;
    //        if(GM.AeroliteManager.aerolitePool.Count > 0) {
    //            sr = GM.AeroliteManager.aerolitePool.Dequeue( );
    //        }
    //        else {
    //            sr = Instantiate(GM.AeroliteManager.aerolite[Random.Range(0, GM.AeroliteManager.aerolite.Length)]) as SpriteRenderer;
    //        }
    //        sr.gameObject.SetActive(true);
    //        sr.transform.parent = mo.transform;
    //        sr.transform.localPosition = Vector3.zero;
    //        mo.GetComponent<Meteor>( ).child = sr;


    //        Aerolite al = sr.GetComponent<Aerolite>( );
    //        al.isMeteor = true;
    //        al.isRotate = true;
    //        mo.transform.up = SetRandomVector(mo.transform, al.aeroliteRadius + 0.3f);

    //        isCreated = true;
    //    }
    //}
    //else {
    //    timer = 0;
    //    isCreated = false;
    //}

    //float RandomSide( )
    //{
    //    int tempInt = Random.Range(0, 2);

    //}
    //int rotateDirection(Vector3 v, )
}
