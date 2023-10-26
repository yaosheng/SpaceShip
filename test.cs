using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start( )
    {
        sr = GetComponent<SpriteRenderer>( );
    }

    void Update( )
    {
        showVertex( );
    }

    void showVertex( )
    {

        Debug.Log("max x : " + sr.bounds.max.x);
        Debug.Log("min x : " + sr.bounds.min.x);
        Debug.Log("max y : " + sr.bounds.max.y);
        Debug.Log("min y : " + sr.bounds.min.y);
    }
}
