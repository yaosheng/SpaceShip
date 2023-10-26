using UnityEngine;
using System.Collections;

public class TutorialParticleSmoke : MonoBehaviour {

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
        TutorialGM.ParticleManager.smokePool.Enqueue(thisPm);
    }
}
