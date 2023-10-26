using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aerolite : MonoBehaviour {

    private SpriteRenderer sr;
    //private bool islocked = false;
    private int rotateDirection = 0;
    private float rotateSpeed = 0;

    public bool isRotate = false;
    //public bool isMeteor = false;
    public Vector3 forwardVector;
    public ParticleSystem meteroTrail;
    public float flySpeed;
    public float aeroliteRadius;

    void Awake( )
    {
        sr = GetComponent<SpriteRenderer>( );
    }

    void OnEnable( )
    {
        rotateSpeed = Random.Range(20, 500);
        flySpeed = Random.Range(0.05f, 0.20f);
        rotateDirection = Random.Range(0, 2);
        sr.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
    }

    void Update( )
    {
        GM.AeroliteManager.OutOfCamera(sr);
        RotateUpdate( );
    }

    void RotateUpdate( )
    {
        if(isRotate) {
            switch(rotateDirection) {
                case 0:
                    sr.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
                    break;
                case 1:
                    sr.transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
                    break;
            }
        }
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        switch(other.transform.tag) {
            case "Bullet":
                GM.ParticleManager.CreatExplosion(sr.transform.position);
                GM.AeroliteManager.RecoverAerolite(sr);
                break;
            case "Ship":
                Debug.Log("warming to hit ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Vector3 pos = SpaceShip.spaceShip.transform.position;
                GM.ParticleManager.CreatExplosion(pos);
                StartCoroutine(GM.CameraManager.LateToGameOver( ));
                break;
        }
    }

    //case "Aerolite":
    //    //Debug.Log("hit aerolite");
    //    if(sr.tag == "Meteor") {
    //        Debug.Log("Aerolite vs Meteor");
    //        GM.ParticleManager.CreatSmoke(sr.transform.position);
    //        Meteor mt1 = sr.transform.parent.GetComponent<Meteor>( );
    //        GM.MeteorManager.RecoverMeteor(mt1, sr);
    //    }
    //    if (sr.tag == "Aerolite") {
    //        break;
    //    }
    //    break;
    //case "Meteor":
    //    //Debug.Log("hit Meteor");
    //    //GM.ParticleManager.CreatSmoke(sr.transform.position);
    //    if(sr.tag == "Aerolite") {
    //        Debug.Log("Meteor vs Aerolite");
    //        GM.ParticleManager.CreatSmoke(sr.transform.position);
    //        GM.AeroliteManager.RecoverAerolite(sr.GetComponent<SpriteRenderer>( ));
    //    }
    //    if(sr.tag == "Meteor") {
    //        Debug.Log("Meteor vs Meteor");
    //        GM.ParticleManager.CreatSmoke(sr.transform.position);
    //        Meteor mt2 = sr.transform.parent.GetComponent<Meteor>( );
    //        GM.MeteorManager.RecoverMeteor(mt2, sr);
    //    }
    //    break;

    //GM.ParticleManager.CreatSmoke(other.transform.position);
    //GM.AeroliteManager.RecoverAerolite(other.GetComponent<SpriteRenderer>( ));
    //if(other.gameObject.tag == "Aerolite") {
    //    Debug.Log("bug？？？？？？？？？？？？？？？" + other.gameObject.tag);
    //    //
    //    //GM.ParticleManager.CreatSmoke(other.transform.position);
    //}

    //移到gamesetup
    //IEnumerator LateToGameOver( )
    //{
    //    SpaceShip.spaceShip.gameObject.SetActive(false);
    //    yield return new WaitForSeconds(0.75f);
    //    Debug.Log("game over");
    //    GM.GameSetUp.gameMode = GameMode.GameOver;
    //}

    //private void OutOfCamera( )
    //{
    //    distance = GM.CameraManager.mainCamera.transform.position.y - sr.transform.position.y;
    //    if(distance >= 10 && sr.transform.position.y < SpaceShip.spaceShip.transform.position.y) {
    //        //Debug.Log("C C");
    //        sr.gameObject.SetActive(false);
    //        GM.AeroliteManager.aerolitePool.Enqueue(sr);
    //    }
    //}

    //if(other.gameObject.tag == "Ship") {
    //    Debug.Log("warming to hit ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    //    Vector3 pos = rd.transform.position;
    //    GM.ParticleManager.CreatExplosion(pos);
    //    //sr.gameObject.SetActive(false);
    //    isRotate = false;
    //    SpaceShip.spaceShip.gameObject.SetActive(false);
    //    GM.UIManager.gameover.gameObject.SetActive(true);
    //    GM.AeroliteManager.aerolitePool.Enqueue(sr);
    //    StartCoroutine(LateToGameOver( ));
    //}

    //public IEnumerator HitAndChangeColor( )
    //{
    //    Debug.Log("change color for hit");
    //    if(sr.color == originalColor) {
    //        sr.color = hitColor;
    //    }
    //    yield return new WaitForSeconds(0.05f);
    //    Debug.Log("change color to original");
    //    if(sr.color == hitColor) {
    //        sr.color = originalColor;
    //    }
    //}
}
