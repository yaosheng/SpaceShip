using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {

    private float runSpeed;
    public SpriteRenderer child;

    void Start( )
    {
        runSpeed = Random.Range(0.05f, 2.25f);
    }

    void OnEnable( )
    {

    }

    void Update () {
        OutOfCamera( );
        transform.Translate(Vector3.up * runSpeed * Time.deltaTime);
	}

    void OutOfCamera( )
    {
        if(transform.localPosition.x <= -6.5f || transform.localPosition.x >= 6.5f) {
            GM.MeteorManager.RecoverMeteor(this, child);
        }
    }


    //Debug.Log("out of camera");
    //if(child.gameObject.activeSelf && child != null) {

    //}
    //GM.AeroliteManager.aerolitePool.Enqueue(child);
    //GM.MeteorManager.meteorPool.Enqueue(this);
    //child.transform.parent = GM.AeroliteManager.aeroliteParent;
    //child.transform.gameObject.SetActive(false);
    //Debug.Log("GM.MeteorManager.meteorPool.count : " + GM.MeteorManager.meteorPool.Count);
    //gameObject.SetActive(false);
}
