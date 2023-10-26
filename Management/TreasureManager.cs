using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureManager : MonoBehaviour {

    public Queue<SpriteRenderer> coinPool = new Queue<SpriteRenderer>( );
    public SpriteRenderer coinSprite;
    public Transform treasureParent;

    public Vector3[] RandomPathBetweenTwoPlanets( Planet p1, Planet p2, int temp)
    {
        float distance1 = Vector3.Distance(p1.transform.position, p2.transform.position),
              distance2 = p1.pathHeight + p2.pathHeight,
              distance3 = Mathf.Abs(p1.pathHeight - p2.pathHeight),
              angel = 0,
              fixedFloat1 = (Random.Range(0, 3) * 0.20f) + 0.50f,
              fixedFloat2 = 3.5f;
                
        Vector3 finalVector = Vector3.one, 
                tempVector = Vector3.one;
        Vector3[ ] pointandVector = new Vector3[2];

        switch(temp) {
            case 0:
                angel = 360 - (Mathf.Asin(distance2 / distance1) * Mathf.Rad2Deg * fixedFloat1);
                finalVector = Quaternion.AngleAxis(angel, Vector3.forward) * (p2.transform.position - p1.transform.position);
                tempVector = Quaternion.AngleAxis(90, Vector3.forward) * finalVector.normalized * p1.pathHeight;
                break;
            case 1:
                angel = Mathf.Asin(distance2 / distance1) * Mathf.Rad2Deg * fixedFloat1;
                finalVector = Quaternion.AngleAxis(angel, Vector3.forward) * (p2.transform.position - p1.transform.position);
                tempVector = Quaternion.AngleAxis(-90, Vector3.forward) * finalVector.normalized * p1.pathHeight;
                break;
            case 2:
                if(p2.pathHeight > p1.pathHeight) {
                    angel = 360 - (Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg) + fixedFloat2;
                }
                else {
                    angel = (Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg) + fixedFloat2;
                }
                finalVector = Quaternion.AngleAxis(angel, Vector3.forward) * (p2.transform.position - p1.transform.position);
                tempVector = Quaternion.AngleAxis(-90, Vector3.forward) * finalVector.normalized * p1.pathHeight;
                break;
            case 3:
                if(p2.pathHeight > p1.pathHeight) {
                    angel = (Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg) - fixedFloat2;
                }
                else {
                    angel = 360 - (Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg) - fixedFloat2;
                }
                finalVector = Quaternion.AngleAxis(angel, Vector3.forward) * (p2.transform.position - p1.transform.position);
                tempVector = Quaternion.AngleAxis(90, Vector3.forward) * finalVector.normalized * p1.pathHeight;
                break;
        }
        pointandVector[0] = p1.transform.position + tempVector;
        pointandVector[1] = finalVector.normalized;
        //Debug.DrawRay(pointandVector[0], pointandVector[1] * 7, Color.yellow);
        return pointandVector;
    }

    public void CreateTreasure( Planet p1, Planet p2, int temp )
    {
        Vector3[ ] rp = RandomPathBetweenTwoPlanets(p1, p2, temp);
        float firstcoinPosition = 1.5f,
              intervalPosition = 1.25f;

        int number = 20;
        Vector3[ ] coinPosition = new Vector3[number];
        List<Vector3> coinvectorList = new List<Vector3>( );
        float fixedFloat = 0.1f;
        for(int i = 0; i < number; i++) {
            coinPosition[i] = rp[0] + rp[1] * (firstcoinPosition + (i * intervalPosition));
            if(Vector3.Distance(p2.transform.position, coinPosition[i]) <= p2.pathHeight + fixedFloat) {
                break;
            }
            else {
                coinvectorList.Add(coinPosition[i]);
            }
        }
        coinPosition = coinvectorList.ToArray( );
        for(int i = 0; i < coinPosition.Length; i++) {
            SpriteRenderer sr;
            if(coinPool.Count > 0) {
                sr = coinPool.Dequeue( );
            }
            else {
                sr = Instantiate(coinSprite) as SpriteRenderer;
            }
            sr.gameObject.SetActive(true);
            sr.transform.position = coinPosition[i];
        }
    }


    //int obTemp = 0;
    //if(Vector3.Distance(p1.transform.position, p2.transform.position) < 2.5) {
    //    Debug.Log("only coin");
    //    obTemp = 0;
    //}
    //else {
    //    obTemp = Random.Range(0, 2);
    //}

    //switch(obTemp) {
    //    case 0:
    //        int number = 20;
    //        Vector3[ ] coinPosition = new Vector3[number];
    //        List<Vector3> coinvectorList = new List<Vector3>( );
    //        float fixedFloat = 0.2f;
    //        for(int i = 0; i < number; i++) {
    //            coinPosition[i] = rp[0] + rp[1] * (firstcoinPosition + (i * intervalPosition));
    //            if(Vector3.Distance(p2.transform.position, coinPosition[i]) <= p2.pathHeight + fixedFloat) {
    //                break;
    //            }
    //            else {
    //                coinvectorList.Add(coinPosition[i]);
    //            }
    //        }
    //        coinPosition = coinvectorList.ToArray( );
    //        for(int i = 0; i < coinPosition.Length; i++) {
    //            SpriteRenderer sr;
    //            if(coinPool.Count > 0) {
    //                sr = coinPool.Dequeue( );
    //            }
    //            else {
    //                sr = Instantiate(coinSprite) as SpriteRenderer;
    //            }
    //            sr.gameObject.SetActive(true);
    //            sr.transform.position = coinPosition[i];
    //        }
    //        break;
    //    case 1:
    //        int aeroliteNumber = Random.Range(10, 15);
    //        Vector3[ ] aerolitePosition = new Vector3[aeroliteNumber];
    //        List<Vector3> aeroliteList = new List<Vector3>( );
    //        float fixedaeroliteDistance = 1.55f;
    //        for(int i = 0; i < aeroliteNumber; i++) {
    //            Vector3 newVector = Quaternion.AngleAxis(Random.Range(-30, 30), Vector3.forward) * rp[1];
    //            aerolitePosition[i] = rp[0] + newVector * (firstaerolitePosition + (i * intervalPosition));

    //        }
    //        for(int i = 0;i < aeroliteNumber; i++) {
    //            if(Vector3.Distance(p1.transform.position, aerolitePosition[i]) > p1.pathHeight + fixedaeroliteDistance &&
    //               Vector3.Distance(p2.transform.position, aerolitePosition[i]) > p2.pathHeight + fixedaeroliteDistance) {
    //                aeroliteList.Add(aerolitePosition[i]);
    //            }
    //        }

    //        Vector3[] tempArray = aeroliteList.ToArray( );

    //        for(int i = 0; i < tempArray.Length; i++) {
    //            SpriteRenderer sr;
    //            if(aerolitePool.Count > 0) {
    //                sr = aerolitePool.Dequeue( );
    //            }
    //            else {
    //                sr = Instantiate(aerolite[Random.Range(0, aerolite.Length)]) as SpriteRenderer;
    //            }
    //            sr.gameObject.SetActive(true);
    //            sr.transform.position = tempArray[i];
    //        }

    //        break;
    //}

    //for(int i = 0; i < 5; i++) {
    //    SpriteRenderer sr = Instantiate(coinSprite) as SpriteRenderer;
    //    sr.gameObject.SetActive(false);
    //    coinPool.Enqueue(sr);
    //}
    //Debug.Log("coinPool.Count : " + coinPool.Count);

    //foreach(Vector3 v in coinvectorList) {
    //    SpriteRenderer sr = Instantiate(pathObject[0]) as SpriteRenderer;
    //    sr.gameObject.SetActive(true);
    //    sr.transform.position = v;
    //}

    //foreach(Vector3 v in coinvectorList) {
    //if(coinPool.Count > 0) {
    //    Debug.Log("use it");
    //    SpriteRenderer sr = coinPool.Dequeue( );
    //    sr.gameObject.SetActive(true);
    //    sr.transform.position = v;
    //}
    //else {
    //    Debug.Log("create it");
    //    SpriteRenderer sr = Instantiate(pathObject[0]) as SpriteRenderer;
    //    sr.gameObject.SetActive(true);
    //    sr.transform.position = v;
    //}


    //}
    //for(int i = 0; i < number; i++) {
    //    SpriteRenderer sr = Instantiate(pathObject[obTemp]) as SpriteRenderer;
    //    sr.transform.position = rp[0] + rp[1] * (firstcoinPosition + (i * intervalPosition));
    //    sr.transform.parent = pathObjectParent;
    //}

    //public void TwoPlanetSetting( Planet p1, Planet p2 )
    //{
    //    float distance1 = Vector3.Distance(p1.transform.position, p2.transform.position);
    //    float distance2 = p1.runpathHeight + p2.runpathHeight;
    //    float distance3 = Mathf.Abs(p1.runpathHeight - p2.runpathHeight);

    //    float angle1 = 0, angle2 = 0, angle3 = 0, angle4 = 0;

    //    angle1 = 360 - Mathf.Asin(distance2 / distance1) * Mathf.Rad2Deg;
    //    Vector3 v11 = Quaternion.AngleAxis(angle1, Vector3.forward) * (p2.transform.position - p1.transform.position);
    //    Vector3 v12 = Quaternion.AngleAxis(90, Vector3.forward) * v11.normalized;
    //    Debug.DrawRay(p1.transform.position + (v12 * p1.runpathHeight), v11, Color.red);
    //    Debug.DrawRay(p1.transform.position + (v12 * p1.runpathHeight), v12, Color.blue);

    //    angle2 = Mathf.Asin(distance2 / distance1) * Mathf.Rad2Deg;
    //    Vector3 v21 = Quaternion.AngleAxis(angle2, Vector3.forward) * (p2.transform.position - p1.transform.position);
    //    Vector3 v22 = Quaternion.AngleAxis(-90, Vector3.forward) * v21.normalized;
    //    Debug.DrawRay(p1.transform.position + (v22 * p1.runpathHeight), v21, Color.red);
    //    Debug.DrawRay(p1.transform.position + (v22 * p1.runpathHeight), v22, Color.blue);

    //    if(p2.runpathHeight > p1.runpathHeight) {
    //        angle3 = 360 - Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg;
    //    }
    //    else {
    //        angle3 = Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg;
    //    }
    //    Vector3 v31 = Quaternion.AngleAxis(angle3, Vector3.forward) * (p2.transform.position - p1.transform.position);
    //    Vector3 v32 = Quaternion.AngleAxis(-90, Vector3.forward) * v31.normalized;
    //    Debug.DrawRay(p1.transform.position + (v32 * p1.runpathHeight), v31, Color.yellow);
    //    Debug.DrawRay(p1.transform.position + (v32 * p1.runpathHeight), v32, Color.blue);


    //    if(p2.runpathHeight > p1.runpathHeight) {
    //        angle4 = Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg;
    //    }
    //    else {
    //        angle4 = 360 - Mathf.Asin(distance3 / distance1) * Mathf.Rad2Deg;
    //    }
    //    Vector3 v41 = Quaternion.AngleAxis(angle4, Vector3.forward) * (p2.transform.position - p1.transform.position);
    //    Vector3 v42 = Quaternion.AngleAxis(90, Vector3.forward) * v41.normalized;
    //    Debug.DrawRay(p1.transform.position + (v42 * p1.runpathHeight), v41, Color.yellow);
    //    Debug.DrawRay(p1.transform.position + (v42 * p1.runpathHeight), v42, Color.blue);
    //}
}
