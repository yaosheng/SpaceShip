using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem planetSmoke;
    public Queue<ParticleSystem> explosioinPool = new Queue<ParticleSystem>( );
    public Queue<ParticleSystem> smokePool = new Queue<ParticleSystem>( );
    public Transform particleParent;

    public void CreatExplosion( Vector3 pos )
    {
        if(explosioinPool.Count > 0) {
            ParticleSystem ps = explosioinPool.Dequeue();
            ps.gameObject.SetActive(true);
            ps.transform.position = pos;
            ps.transform.parent = particleParent;
        }
        else {
            ParticleSystem ps = Instantiate(explosion) as ParticleSystem;
            ps.gameObject.SetActive(true);
            ps.transform.position = pos;
            ps.transform.parent = particleParent;
        }
    }

    public void CreatSmoke(Vector3 pos )
    {
        if(smokePool.Count > 0) {
            ParticleSystem ps = smokePool.Dequeue( );
            ps.gameObject.SetActive(true);
            ps.transform.position = pos;
            ps.transform.parent = particleParent;
        }
        else {
            ParticleSystem ps = Instantiate(planetSmoke) as ParticleSystem;
            ps.gameObject.SetActive(true);
            ps.transform.position = pos;
            ps.transform.parent = particleParent;
        }
    }
}
