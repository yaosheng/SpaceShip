using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private SpriteRenderer sr;
    //private float distance;

    void Awake( )
    {
        sr = GetComponent<SpriteRenderer>( );
    }

    void OnEnable( )
    {
        sr.transform.parent = GM.TreasureManager.treasureParent;
        sr.transform.localScale = new Vector3(0.4f, 0.4f, 0);
    }

    void Update( )
    {
        GM.CameraManager.OutOfCamera(sr, GM.TreasureManager.coinPool);
    }

    void OnTriggerEnter2D( Collider2D other )
    {
        if(other.gameObject.tag == "Ship") {
            GM.UIManager.coinSum++;
            GM.UIManager.coin++;
            sr.gameObject.SetActive(false);
            GM.TreasureManager.coinPool.Enqueue(sr);
            if(GM.SoundManager.isSoundTurnOn) {
                GM.SoundManager.EatingCoin( );
            }
        }
    }
}