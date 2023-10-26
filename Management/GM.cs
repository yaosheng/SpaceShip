using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour {

    private static UIManager gameui;
    public static UIManager UIManager;
    //{
    //    get {
    //        if(gameui == null) {
    //            gameui = GameObject.FindObjectOfType(typeof(UIManager)) as UIManager;
    //        }
    //         return gameui;
    //    }
    //}

    private static GameSetUp setup;
    public static GameSetUp GameSetUp;
    //{
    //    get {
    //        if(setup == null) {
    //            setup = GameObject.FindObjectOfType(typeof(GameSetUp)) as GameSetUp;
    //        }
    //        return setup;
    //    }
    //}

    private static BulletManager BM;
    public static BulletManager BulletManager;
    //{
    //    get {
    //        if(BM == null) {
    //            BM = GameObject.FindObjectOfType(typeof(BulletManager)) as BulletManager;
    //        }
    //        return BM;
    //    }
    //}

    private static TreasureManager tm;
    public static TreasureManager TreasureManager;
    //{
    //    get {
    //        if(tm == null) {
    //            tm = GameObject.FindObjectOfType(typeof(TreasureManager)) as TreasureManager;
    //        }
    //        return tm;
    //    }
    //}

    private static AeroliteManager am;
    public static AeroliteManager AeroliteManager;
    //{
    //    get {
    //        if(am == null) {
    //            am = GameObject.FindObjectOfType(typeof(AeroliteManager)) as AeroliteManager;
    //        }
    //        return am;
    //    }
    //}

    private static SpriteManager sm;
    public static SpriteManager SpriteManager;
    //{
    //    get {
    //        if(sm == null) {
    //            sm = GameObject.FindObjectOfType(typeof(SpriteManager)) as SpriteManager;   
    //        }
    //        return sm;
    //    }
    //}

    private static ParticleManager pm;
    public static ParticleManager ParticleManager;
    //{
    //    get {
    //        if(pm == null) {
    //            pm = GameObject.FindObjectOfType(typeof(ParticleManager)) as ParticleManager;
    //        }
    //        return pm;
    //    }
    //}

    private static SoundManager som;
    public static SoundManager SoundManager;
    //{
    //    get {
    //        if(som == null) {
    //            som = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
    //        }
    //        return som;
    //    }
    //}

    private static CameraManager cm;
    public static CameraManager CameraManager;
    //{
    //    get {
    //        if(cm == null) {
    //            cm = GameObject.FindObjectOfType(typeof(CameraManager)) as CameraManager;
    //        }
    //        return cm;
    //    }
    //}

    private static PlanetManager plm;
    public static PlanetManager PlanetManager;
    //{
    //    get {
    //        if(plm == null) {
    //            plm = GameObject.FindObjectOfType(typeof(PlanetManager)) as PlanetManager;
    //        }
    //        return plm;
    //    }
    //}

    private static MeteorManager mm;
    public static MeteorManager MeteorManager;
    //{
    //    get {
    //        if(mm == null) {
    //            mm = GameObject.FindObjectOfType(typeof(MeteorManager)) as MeteorManager;
    //        }
    //        return mm;
    //    }
    //}

    private static GooglePlayManager gpm;
    public static GooglePlayManager GooglePlayManager;
    //{
    //    get {
    //        if(gpm == null) {
    //            gpm = GameObject.FindObjectOfType(typeof(GooglePlayManager)) as GooglePlayManager;
    //        }
    //        return gpm;
    //    }
    //}

    private GM( )
    {

    }


    void Awake( )
    {
        GooglePlayManager = GameObject.FindObjectOfType(typeof(GooglePlayManager)) as GooglePlayManager;
        MeteorManager = GameObject.FindObjectOfType(typeof(MeteorManager)) as MeteorManager;
        PlanetManager = GameObject.FindObjectOfType(typeof(PlanetManager)) as PlanetManager;
        CameraManager = GameObject.FindObjectOfType(typeof(CameraManager)) as CameraManager;
        SoundManager = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
        ParticleManager = GameObject.FindObjectOfType(typeof(ParticleManager)) as ParticleManager;
        SpriteManager = GameObject.FindObjectOfType(typeof(SpriteManager)) as SpriteManager;
        AeroliteManager = GameObject.FindObjectOfType(typeof(AeroliteManager)) as AeroliteManager;
        TreasureManager = GameObject.FindObjectOfType(typeof(TreasureManager)) as TreasureManager;
        BulletManager = GameObject.FindObjectOfType(typeof(BulletManager)) as BulletManager;
        GameSetUp = GameObject.FindObjectOfType(typeof(GameSetUp)) as GameSetUp;
        UIManager = GameObject.FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    //public IEnumerator LateToGameOver( )
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    setup.gameMode = GameMode.GameOver;
    //}

    //void Start( )
    //{
    //    Debug.Log("run gm");
    //}

    //private static SpaceShip ss;
    //public static 


}
