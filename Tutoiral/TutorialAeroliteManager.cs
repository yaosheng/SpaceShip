using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TutorialAeroliteManager : MonoBehaviour {

    public Queue<SpriteRenderer> aerolitePool = new Queue<SpriteRenderer>( );
    public Transform aeroliteParent;
    public SpriteRenderer[ ] aerolite;
    public const float left = -4.5f;
    public const float right = 4.5f;

    public void CreateAerolitePlus( TutorialPlanet p1, TutorialPlanet p2 ) {
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
            if(aerolitePool.Count > 0) {
                sr = aerolitePool.Dequeue( );
            }
            else {
                sr = Instantiate(aerolite[Random.Range(0, aerolite.Length)]);
            }
            distance1 = (Vector3.Distance(p1.transform.position, p2.transform.position) - p1.pathHeight - p2.pathHeight) / 2 - fixedDistance - sr.GetComponent<Aerolite>().aeroliteRadius;
            distance2 = Vector3.Distance(p1.transform.position, p2.transform.position) / 2;

            sr.gameObject.SetActive(true);
            sr.transform.parent = TutorialGM.AeroliteManager.aeroliteParent;
            sr.transform.localScale = new Vector3(1, 1, 0);
            randomPosition1 = centerPoint + (centerVector * Random.Range(-distance1, distance1));
            randomPosition2 = randomPosition1 + (normalVector * Random.Range(-distance2, distance2));
            sr.transform.position = randomPosition2;
        }
    }

    private int AeroliteDifficulty( )
    {
        int temp = Random.Range(9, 15);
        return temp;
    }
 }
