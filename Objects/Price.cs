using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Price : MonoBehaviour {

    public Image shipImage;
    public int shipPrice;
    public Text priceNumber;
    public Image priceImage;
    public bool isBuy = false;
    public string priceName;
    public int spriteNumber;

    public void BuySpaceShip( )
    {
        if(!isBuy) {
            Debug.Log("buy it");
            if(PriceManager.coinNumber > shipPrice) {
                PriceManager.coinNumber -= shipPrice;
                PlayerPrefs.SetInt("coinAmount", PriceManager.coinNumber);
                PlayerPrefs.SetInt(priceName, 1);
                PlayerPrefs.SetInt("shipNumber", spriteNumber);
                PlayerPrefs.SetInt("criclePosition", spriteNumber);
                shipImage.enabled = true;
                priceImage.enabled = false;
                isBuy = true;
                PriceManager.ringSprite.rectTransform.localPosition = this.gameObject.GetComponent<RectTransform>( ).localPosition;
            }
        }
        else {
            PriceManager.ringSprite.rectTransform.localPosition = this.gameObject.GetComponent<RectTransform>( ).localPosition;
            PlayerPrefs.SetInt("shipNumber", spriteNumber);
            PlayerPrefs.SetInt("criclePosition", spriteNumber);
        }
    }

    void Update( )
    {
        if(priceImage != null) {
            if(!isBuy) {
                priceImage.enabled = true;
                priceNumber.enabled = true;
                shipImage.enabled = false;
            }
            else {
                shipImage.enabled = true;
                priceImage.enabled = false;
                priceNumber.enabled = false;
            }
        }
    }
}
