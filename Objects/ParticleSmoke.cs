using UnityEngine;
using System.Collections;

public class ParticleSmoke : MonoBehaviour {

    private ParticleSystem thisPm;

    void Awake( )
    {
        thisPm = GetComponent<ParticleSystem>( );
    }

    void OnEnable( )
    {
        StartCoroutine(ReturnToPool( ));
    }

    IEnumerator ReturnToPool( )
    {
        yield return new WaitForSeconds(1.0f);
        thisPm.gameObject.SetActive(false);
        GM.ParticleManager.smokePool.Enqueue(thisPm);
    }
}
