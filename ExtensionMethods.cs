using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {

    public static void ResetTransform( this Transform trans )
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetParent( this Transform g, Transform parent )
    {
        g.parent = parent;
    }

    //public static void CheckAndGetFloat( this PlayerPrefs player, string name)
    //{
    //    PlayerPrefs.
    //    if(player)
    //}
}
