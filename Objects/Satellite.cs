using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

    public float rotateSpeed;
    public int rotateDirection;
    public Planet thisPlanet;
	
	void Update () {
        SatelliteRevolution( );
    }

    void SatelliteRevolution( )
    {
        switch(rotateDirection) {
            case 0:
                transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
                break;
            case 1:
                transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
                break;
        }
    }
}
