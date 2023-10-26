using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialBulletManager : MonoBehaviour {

    public SpriteRenderer bullet;
    public Transform bulletParent;
    public Queue<SpriteRenderer> bulletPool = new Queue<SpriteRenderer>( );
    private float timer = 0.1f;
    private int teachInt = 10;

    void Update( )
    {
        switch(TutorialGM.GameSetUp.motiomMode) {
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
        if(TutorialGM.PlanetManager.stepNumber == TutorialGM.GameSetUp.createProportion[0]) {
            timer += Time.deltaTime;
            if(TutorialSpaceShip.isbuttonDown) {
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
        if(Input.GetButton("Fire1") && TutorialSpaceShip.spaceShip.gameObject.activeSelf) {
            if(timer >= 0.1f) {
                CreateBullet(0);
                CreateBullet(1);
                timer = 0;
                teachInt--;
            }
        }
        if(teachInt == 0) {
            TutorialGM.GameSetUp.motiomMode = MotionMode.Drift;
            TutorialSpaceShip.spaceShip.GetComponent<TutorialSpaceShip>( ).FindTargetPlanet( );
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
            sr.transform.localPosition = TutorialSpaceShip.spaceShip.transform.position + TutorialSpaceShip.spaceShip.transform.right * (-0.15f) + TutorialSpaceShip.spaceShip.transform.up * 0.5f;
        }
        else {
            sr.transform.localPosition = TutorialSpaceShip.spaceShip.transform.position + TutorialSpaceShip.spaceShip.transform.right * (0.15f) + TutorialSpaceShip.spaceShip.transform.up * 0.5f;
        }

    }
}
