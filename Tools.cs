using UnityEngine;
using System.Collections;

public class Tools : MonoBehaviour {

#region abc
    public static int[ ] RandomInt( int i )
    {
        int[ ] ranNum = new int[i];
        int[ ] ranArray = new int[i];
        int temp = 0;
        for(int j = 0; j < i; j++) {
            ranNum[j] = j;
        }
        for(int k = 0, m = i; k < i; k++, m--) {
            temp = Random.Range(0, m);
            ranArray[k] = ranNum[temp];
            ranNum[temp] = ranNum[m - 1];
        }
        return ranArray;
    }
#endregion
    public static float FindStraightLineDistanceAboutOutSidePointToCircumference( Transform point, Transform cricle, float radius)
    {
        float angle1 = 0, angle2 = 0, distance1 = 0, distance2 = 0, distance3 = 0, distance4 = 0, distance5 = 0;
        Ray forwardRay = new Ray(point.position, point.up * 10);
        //Vector3 originalVector = cricle.position - point.position;
        //distance1：distance between point and circle center
        distance1 = Vector3.Distance(point.position, cricle.position);
        //distance2 : cross with two vector
        distance2 = Vector3.Cross(forwardRay.direction, cricle.position - forwardRay.origin).magnitude;
        angle1 = Mathf.Asin(distance2 / distance1) * Mathf.Rad2Deg;
        distance3 = Mathf.Abs(Mathf.Cos(Mathf.PI / (180 / angle1))) * distance1;
        angle2 = Mathf.Acos(distance2 / radius) * Mathf.Rad2Deg;
        distance4 = Mathf.Abs(Mathf.Sin(Mathf.PI / (180 / angle2))) * radius;
        distance5 = distance3 - distance4;

        return distance5;
    }
}


