using UnityEngine;
using System.Collections;

public class SatelliteManager : MonoBehaviour {

    public float fixedsatelliteDistance;
    public GameObject satelliteSystem;
    public Planet thisPlanet;

    private int _rotateDirection;
    private float _rotateSpeed;

    //private Satellite satelliteObject;
    void Awake( )
    {
        thisPlanet = GetComponent<Planet>( );
        _rotateDirection = Random.Range(0, 2);
        _rotateSpeed = thisPlanet.rotateSpeed * (1 / Random.Range(5.0f, 15.0f));
    }

    public void SetSatellite(Planet p, int sn)
    {
        int[ ] tempintArray = Tools.RandomInt(24);

        for(int i = 0; i < sn; i++) {
            GameObject go = Instantiate(satelliteSystem) as GameObject;
            go.transform.parent = p.transform;
            go.GetComponent<Satellite>( ).thisPlanet = p;
            go.GetComponent<Satellite>( ).rotateDirection = _rotateDirection;
            go.GetComponent<Satellite>( ).rotateSpeed = _rotateSpeed;
            go.transform.localPosition = Vector3.zero;
            go.transform.rotation = Quaternion.AngleAxis(tempintArray[i] * 15, Vector3.forward);

            SpriteRenderer sr;
            int temp = Random.Range(0, 10);
            if(GM.AeroliteManager.aerolitePool.Count > 0) {
                sr = GM.AeroliteManager.aerolitePool.Dequeue( );
            }
            else {
                if(temp >= 1) {
                    sr = Instantiate(GM.AeroliteManager.aerolite[Random.Range(0, GM.AeroliteManager.aerolite.Length - 3)]);
                }
                else {
                    sr = Instantiate(GM.AeroliteManager.aerolite[Random.Range(GM.AeroliteManager.aerolite.Length - 3, GM.AeroliteManager.aerolite.Length)]);
                }
            }

            sr.gameObject.SetActive(true);
            sr.transform.parent = go.transform;
            sr.transform.localPosition = new Vector3(p.pathHeight + fixedsatelliteDistance + sr.GetComponent<Aerolite>().aeroliteRadius, 0, 0);
            sr.transform.localScale = Vector3.one;
            sr.GetComponentInChildren<Aerolite>( ).isRotate = true;
            sr.tag = "Aerolite";
        }
    }

    public void SetSatelliteDifficulty(Planet p)
    {
        int[ ] tempArray = new int[GM.GameSetUp.createProportion.Length - 1];
        int temp = Random.Range(1, 11);
        for(int i = 0; i < tempArray.Length; i++) {
            tempArray[i] = GM.GameSetUp.createProportion[i + 1];
        }
        if(GM.PlanetManager.planetSumNumber < tempArray[0])
            return;
        else if(GM.PlanetManager.planetSumNumber == tempArray[0]) {
            SetSatellite(p, 1);
        }
        else if(GM.PlanetManager.planetSumNumber > tempArray[0] && GM.PlanetManager.planetSumNumber < tempArray[1]) {
            if(temp > 0 && temp <= 4) {
                return;
            }
            else {
                SetSatellite(p, 1);
            }
        }
        else if(GM.PlanetManager.planetSumNumber >= tempArray[1] && GM.PlanetManager.planetSumNumber < tempArray[2]) {
            if(temp > 0 && temp <= 2) {
                return;
            }
            else if(temp > 2 && temp <= 6) {
                SetSatellite(p, 1);
            }
            else {
                SetSatellite(p, 2);
            }
        }
        else if(GM.PlanetManager.planetSumNumber >= tempArray[2] && GM.PlanetManager.planetSumNumber < tempArray[3]) {
            if(temp > 0 && temp <= 1) {
                return;
            }
            else if(temp > 1 && temp <= 4) {
                SetSatellite(p, 1);
            }
            else if(temp > 4 && temp <= 7) {
                SetSatellite(p, 2);
            }
            else {
                SetSatellite(p, 3);
            }
        }
        else {
            if(temp > 0 && temp <= 2) {
                SetSatellite(p, 1);
            }
            else if(temp > 2 && temp <= 6) {
                SetSatellite(p, 2);
            }
            else {
                SetSatellite(p, 3);
            }
        }
    }
    //if(isSatellite) {
    //    //SatelliteRevolution( );
    //    //SatelliteRotation( );
    //}
    //fixedsatelliteDistance = 0.005f;
    //_rotateDirection = Random.Range(0, 2);
    //_rotateSpeed = thisPlanet.rotateSpeed * (1 / Random.Range(5.0f, 15.0f));

    //void SatelliteRevolution( )
    //{
    //    switch(_rotateDirection) {
    //        case 0:
    //            for(int i = 0; i < _satelliteSystem.Length; i++) {
    //                _satelliteSystem[i].transform.Rotate(Vector3.forward * Time.deltaTime * _rotateSpeed);
    //            }

    //            break;
    //        case 1:
    //            for(int i = 0; i < _satelliteSystem.Length; i++) {
    //                _satelliteSystem[i].transform.Rotate(Vector3.back * Time.deltaTime * _rotateSpeed);
    //            }
    //            break;
    //    }
    //}

    //_satelliteRotateSpeed = new int[sn];
    //_satellite = new Aerolite[sn];
    //_satelliterotateDirection = new int[sn];

    //sr.sprite = GM.AeroliteManager.aerolite[Random.Range(0, GM.AeroliteManager.aerolite.Length)].sprite;
    //_satelliteRotateSpeed[i] = Random.Range(10, 500);
    //_satelliterotateDirection[i] = Random.Range(0, 2);
    //_satellite[i] = go.GetComponentInChildren<Aerolite>();
    //_satellite[i].isRotate = true;

    //void SatelliteRotation( )
    //{
    //    for(int i = 0; i < _satellite.Length; i++) {
    //        if(_satellite[i].isRotate == true) {
    //            switch(_satelliterotateDirection[i]) {
    //                case 0:
    //                    _satellite[i].transform.Rotate(Vector3.forward * Time.deltaTime * _satelliteRotateSpeed[i]);
    //                    break;
    //                case 1:
    //                    _satellite[i].transform.Rotate(Vector3.back * Time.deltaTime * _satelliteRotateSpeed[i]);
    //                    break;
    //            }
    //        }
    //    }
    //}

    //satelliteSystem.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    //void OnTriggerEnter2D( Collider2D other )
    //{
    //    if(other.gameObject.tag == "Ship") {
    //        sr.gameObject.SetActive(false);
    //        SpaceShip.spaceShip.gameObject.SetActive(false);
    //        GM.UIManager.gameover.gameObject.SetActive(true);
    //    }
    //}
}
