using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {

    public SpriteRenderer bullet;
    public Transform bulletParent;
    public Queue<SpriteRenderer> bulletPool = new Queue<SpriteRenderer>( );
    private float timer = 0.1f;
    private int teachInt = 10;

    void Update( )
    {
        switch(GM.GameSetUp.motiomMode) {
            case MotionMode.Drift:
                Shooter( );
                break;
            case MotionMode.TeachShoot:
                TeachToShoot( );
                break;
        }
    }

    void Shooter( )
    {
        if(GM.PlanetManager.stepNumber >= GM.GameSetUp.createProportion[0]) {
            timer += Time.deltaTime;
            if(SpaceShip.isbuttonDown) {
                if(timer >= 0.1f) {
                    CreateBullet(0);
                    CreateBullet(1);
                    timer = 0;
                }
            }
        }

    }

    void TeachToShoot( )
    {
        timer += Time.deltaTime;
        if(Input.GetButton("Fire1") && SpaceShip.spaceShip.gameObject.activeSelf) {
            if(timer >= 0.1f) {
                CreateBullet(0);
                CreateBullet(1);
                timer = 0;
                teachInt--;
            }
        }
        if(teachInt == 0) {
            GM.GameSetUp.motiomMode = MotionMode.Drift;
            //GM.CameraManager.islockedcameraMoving = false;
            SpaceShip.spaceShip.GetComponent<SpaceShip>( ).FindTargetPlanet( );
        }
    }

    void CreateBullet(int temp)
    {
        SpriteRenderer sr;
        if(bulletPool.Count > 0) {
            sr = bulletPool.Dequeue( );
        }
        else {
            sr = Instantiate(bullet) as SpriteRenderer;
        }
        sr.gameObject.SetActive(true);
        if(temp == 0) {
            sr.transform.localPosition = SpaceShip.spaceShip.transform.position + SpaceShip.spaceShip.transform.right * (-0.15f) + SpaceShip.spaceShip.transform.up * 0.5f;
        }
        else {
            sr.transform.localPosition = SpaceShip.spaceShip.transform.position + SpaceShip.spaceShip.transform.right * (0.15f) + SpaceShip.spaceShip.transform.up * 0.5f;
        }

    }

    //if(GM.GameSetUp.motiomMode == MotionMode.Drift && GM.PlanetManager.stepNumber >= 10) {
    //    Shooter( );
    //}
    //if(GM.GameSetUp.motiomMode == MotionMode.Teach) {
    //    TeachToShoot( );
    //}

    //if(Input.GetButton("Fire1") && SpaceShip.spaceShip.gameObject.activeSelf) {

    //}

    //if(bulletPool.Count > 0) {
    //    SpriteRenderer sr1 = bulletPool.Dequeue( );
    //    sr1.gameObject.SetActive(true);
    //}
    //else {
    //    Debug.Log("no bullet");
    //    SpriteRenderer sr1 = Instantiate(bullet) as SpriteRenderer;
    //    sr1.gameObject.SetActive(true);
    //}




    //if(GM.UIManager.nowEnergy > 0) {
    //    if(bulletPool.Count > 0) {
    //        SpriteRenderer sr = bulletPool.Dequeue( );
    //        sr.gameObject.SetActive(true);
    //    }
    //    else {
    //        Debug.Log("no bullet");
    //        SpriteRenderer sr1 = Instantiate(bullet) as SpriteRenderer;
    //        sr1.gameObject.SetActive(true);
    //    }
    //    GM.UIManager.nowEnergy -= shootEnergy;
    //}

    //for(int i = 0; i < 5; i++) {
    //    SpriteRenderer sr = Instantiate(bullet) as SpriteRenderer;
    //    sr.gameObject.SetActive(false);
    //    bulletPool.Enqueue(sr);
    //}
}
