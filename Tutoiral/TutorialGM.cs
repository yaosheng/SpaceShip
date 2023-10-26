using UnityEngine;
using System.Collections;

public class TutorialGM : MonoBehaviour {

    private static TutorialUIManager gameui;
    public static TutorialUIManager UIManager
    {
        get {
            if(gameui == null) {
                gameui = GameObject.FindObjectOfType(typeof(TutorialUIManager)) as TutorialUIManager;
            }
             return gameui;
        }
    }

    private static TutorialGameSetUp setup;
    public static TutorialGameSetUp GameSetUp
    {
        get {
            if(setup == null) {
                setup = GameObject.FindObjectOfType(typeof(TutorialGameSetUp)) as TutorialGameSetUp;
            }
            return setup;
        }
    }

    private static TutorialBulletManager BM;
    public static TutorialBulletManager BulletManager
    {
        get {
            if(BM == null) {
                BM = GameObject.FindObjectOfType(typeof(TutorialBulletManager)) as TutorialBulletManager;
            }
            return BM;
        }
    }

    private static TutorialAeroliteManager am;
    public static TutorialAeroliteManager AeroliteManager
    {
        get {
            if(am == null) {
                am = GameObject.FindObjectOfType(typeof(TutorialAeroliteManager)) as TutorialAeroliteManager;
            }
            return am;
        }
    }

    private static TutorialSpriteManager sm;
    public static TutorialSpriteManager SpriteManager
    {
        get {
            if(sm == null) {
                sm = GameObject.FindObjectOfType(typeof(TutorialSpriteManager)) as TutorialSpriteManager;   
            }
            return sm;
        }
    }

    private static TutorialParticleManager pm;
    public static TutorialParticleManager ParticleManager
    {
        get {
            if(pm == null) {
                pm = GameObject.FindObjectOfType(typeof(TutorialParticleManager)) as TutorialParticleManager;
            }
            return pm;
        }
    }


    private static TutorialCameraManager cm;
    public static TutorialCameraManager CameraManager
    {
        get {
            if(cm == null) {
                cm = GameObject.FindObjectOfType(typeof(TutorialCameraManager)) as TutorialCameraManager;
            }
            return cm;
        }
    }

    private static TutorialPlanetManager plm;
    public static TutorialPlanetManager PlanetManager
    {
        get {
            if(plm == null) {
                plm = GameObject.FindObjectOfType(typeof(TutorialPlanetManager)) as TutorialPlanetManager;
            }
            return plm;
        }
    }

    private TutorialGM( )
    {

    }
}
