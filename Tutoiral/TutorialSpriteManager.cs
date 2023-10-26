using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSpriteManager : MonoBehaviour {

    public Sprite[ ] planetSprite1;
    public Sprite[ ] planetSprite2;
    public Sprite[ ] planetDetail;

    void Awake( )
    {
        GetPlanetSprite( );
    }

    void GetPlanetSprite( )
    {
        List<Sprite> spriteList1 = new List<Sprite>( );
        List<Sprite> spriteList2 = new List<Sprite>( );
        Object[ ] testObject1 = (Object[ ])Resources.LoadAll("Planets1");
        Object[ ] testObject2 = (Object[ ])Resources.LoadAll("Planets2");

        for(int i = 0; i < testObject1.Length; i++) {
            if(testObject1[i] is Sprite) {
                Sprite s = (Sprite)testObject1[i];
                spriteList1.Add(s);
            }
        }
        for(int i = 0; i < testObject2.Length; i++) {
            if(testObject2[i] is Sprite) {
                Sprite s = (Sprite)testObject2[i];
                spriteList2.Add(s);
            }
        }
        planetSprite1 = spriteList1.ToArray( );
        planetSprite2 = spriteList2.ToArray( );
    }
}
