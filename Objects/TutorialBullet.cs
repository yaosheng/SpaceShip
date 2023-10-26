using UnityEngine;
using System.Collections;

public class TutorialBullet : MonoBehaviour {

    private float shootSpeed = 25.0f;
    private SpriteRenderer sr;

    void Awake( )
    {
        sr = GetComponent<SpriteRenderer>( );
    }

    void OnEnable( )
    {
        sr.transform.parent = TutorialGM.BulletManager.bulletParent;
        sr.transform.up = TutorialSpaceShip.spaceShip.transform.up;
        sr.transform.localScale = new Vector3(0.3f, 0.3f, 0);
    }

    void Update( )
    {
        OutOfCamera( );
        BulletMotion( );
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        switch(other.gameObject.tag) {
            case "Planet":
                TutorialGM.ParticleManager.CreatSmoke(sr.transform.position);
                sr.gameObject.SetActive(false);
                TutorialGM.BulletManager.bulletPool.Enqueue(sr);
                break;
        }
    }

    private void BulletMotion( )
    {
        sr.transform.Translate(Vector3.up * shootSpeed * Time.deltaTime);
    }

    private void OutOfCamera( )
    {
        if(!sr.isVisible) {
            sr.gameObject.SetActive(false);
            TutorialGM.BulletManager.bulletPool.Enqueue(sr);
        }
    }
}
