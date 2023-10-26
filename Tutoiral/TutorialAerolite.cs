using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialAerolite : MonoBehaviour {

    private SpriteRenderer sr;
    private int rotateDirection = 0;
    private float rotateSpeed = 0;

    public bool isRotate = false;
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
                TutorialGM.ParticleManager.CreatExplosion(sr.transform.position);
                sr.gameObject.SetActive(false);
                break;
        }
    }
}
