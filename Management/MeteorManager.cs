using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorManager : MonoBehaviour {

    public Queue<MeteorSystem> meteorSystemPool = new Queue<MeteorSystem>( );
    public Queue<Meteor> meteorPool = new Queue<Meteor>( );
    public MeteorSystem meteorSystem;
    public Meteor meteor;
    public Transform meteorParent;

    public void CreateMeteor( Planet p1, Planet p2, float timer )
    {
        MeteorSystem ms;
        if(meteorSystemPool.Count > 0) {
            ms = meteorSystemPool.Dequeue( );
        }
        else {
            ms = Instantiate(meteorSystem) as MeteorSystem;
        }
        ms.gameObject.SetActive(true);
        ms.transform.parent = meteorParent;
        ms.planet1 = p1;
        ms.planet2 = p2;
        ms.flyType = Random.Range(0, 9);
        ms.meteorFrequency = timer;
    }

    public void RecoverMeteor( Meteor mt, SpriteRenderer sr)
    {
        if(sr.tag == "Meteor") {
            sr.transform.parent = null;
            meteorPool.Enqueue(mt);
            GM.AeroliteManager.meteorspritePool.Enqueue(sr);
            sr.transform.parent = GM.AeroliteManager.aeroliteParent;

            sr.GetComponent<Aerolite>( ).isRotate = false;
            mt.gameObject.SetActive(false);
            sr.gameObject.SetActive(false);
            //sr.tag = "Aerolite";
        }
    }
}
