﻿using UnityEngine;
using System.Collections;

public class ParticleExplosion : MonoBehaviour {

    private ParticleSystem thisPm;

    void Awake( )
    {
        thisPm = GetComponent<ParticleSystem>();
    }

    void OnEnable( )
    {
        StartCoroutine(ReturnToPool( ));
    }

    IEnumerator ReturnToPool( )
    {
        yield return new WaitForSeconds(1.0f);
        thisPm.gameObject.SetActive(false);
        GM.ParticleManager.explosioinPool.Enqueue(thisPm);
    }
}
