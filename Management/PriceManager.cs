using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if(UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;


public class PriceManager : MonoBehaviour {

    public static int coinNumber;
    public Text coinUi;
    public Price[ ] priceArray = new Price[20];
    public int[ ] isbuyArray {
        get {
            int[ ] tempArray = new int[priceArray.Length];
            string tempString = "price";
            tempArray[0] = 1;
            for(int i = 1; i < tempArray.Length; i++) {
                string s = tempString + i.ToString( );
                if(PlayerPrefs.HasKey(s)) {
                    tempArray[i] = PlayerPrefs.GetInt(s);
                }
                else {
                    tempArray[i] = 0;
                }
            }
            return tempArray;
        }
    }
    public int criclePosition
    {
        get {
            int temp = 0;
            if(PlayerPrefs.HasKey("criclePosition")) {
                temp = PlayerPrefs.GetInt("criclePosition");
            }
            else {
                temp = 0;
            }
            return temp;
        }
    }
    public Color enough;
    public Color notEnough;
    public Image ringImage;
    public static Image ringSprite;
    public string achievement5_FleetCommander;
    private int temp = 0;


    void Awake( )
    {
        ShowPriceData( );
        SetPriceName( );
        CheckBuying( );
        coinNumber = GameSetUp.coinAmount;
        ringSprite = ringImage;
    }
	
    void Start( )
    {
        SetCriclePosition( );
        //coinNumber = 10000;
        //DeleteBuy( );
        //AddCoin( );
    }

    void SetCriclePosition( )
    {
        for(int i = 0; i < priceArray.Length; i++) {
            if(criclePosition == i) {
                ringSprite.rectTransform.localPosition = priceArray[i].gameObject.GetComponent<RectTransform>( ).localPosition;
            }
        }
    }
    
    void AddCoin( )
    {
        PlayerPrefs.SetInt("coinAmount", 5000);
    }

    void DeleteBuy( )
    {
        PlayerPrefs.DeleteKey("shipNumber");
        for(int i = 1; i < isbuyArray.Length; i++) {
            PlayerPrefs.DeleteKey("price" + i);
        }
    }

    void SetPriceName( )
    {
        for(int i = 0; i < priceArray.Length; i++) {
            priceArray[i].priceName = "price" + i.ToString( );
            priceArray[i].spriteNumber = i;
        }
    }

    void CheckBuying( )
    {
        for(int i = 1; i < isbuyArray.Length; i++) {
            if(isbuyArray[i] == 0) {
                priceArray[i].isBuy = false;
            }
            else {
                priceArray[i].isBuy = true;
            }
        }
    }

    void ShowPriceData( )
    {
        string temp = "";
        for(int i = 0; i < isbuyArray.Length; i++) {
            temp += "[" + "price" + i.ToString( ) + " : " + isbuyArray[i] + "]" + ",";
        }
        Debug.Log(temp);
    }

    void Update( )
    {
        coinUi.text = coinNumber.ToString( );

        for(int i = 1; i < priceArray.Length; i++) {
            if(coinNumber < priceArray[i].shipPrice) {
                priceArray[i].priceNumber.color = notEnough;
            }
            else {
                priceArray[i].priceNumber.color = enough;
            }
            if(i == 4 || i == 8 || i ==12 || i == 16) {
                if(!priceArray[i - 4].isBuy) {
                    priceArray[i].priceImage.gameObject.SetActive(false);
                }
                else {
                    priceArray[i].priceImage.gameObject.SetActive(true);
                }
            }
            else {
                if(!priceArray[i - 1].isBuy) {
                    priceArray[i].priceImage.gameObject.SetActive(false);
                }
                else {
                    priceArray[i].priceImage.gameObject.SetActive(true);
                }
            }
        }

        for(int i = 0; i < isbuyArray.Length; i++) {
            if(isbuyArray[i] == 1)
                temp++;
        }
        if(temp == isbuyArray.Length) {
            AchievementFleetCommander( );
            temp = 0;
        }
        else {
            temp = 0;
        }
    }

    public void AchievementFleetCommander( )
    {
        Social.localUser.Authenticate(( bool success ) =>
        {
            if(success) {
                Social.ReportProgress(achievement5_FleetCommander, 100.0f, ( bool success1 ) => {
                    // handle success or failure
                });
            }
        });
    }

    //public void MoveRing(RectTransform trans)
    //{
    //    ringImage.rectTransform.localPosition = trans.localPosition;
    //}
}
