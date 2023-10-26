using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SoundOff : MonoBehaviour, IPointerDownHandler
{
    public virtual void OnPointerDown( PointerEventData data )
    {
        Debug.Log("Sound Off");
        GM.SoundManager.isSoundTurnOn = false;
        GM.UIManager.soundOpen.gameObject.SetActive(false);
        GM.UIManager.soundClose.gameObject.SetActive(true);
        GM.UIManager.soundOpen1.gameObject.SetActive(false);
        GM.UIManager.soundClose1.gameObject.SetActive(true);
        PlayerPrefs.SetInt("SoundControl", 0);
        //GM.ADRManager.LogEvent("Sound Off");
        //GM.GameSetUp.AdsReportingManager.LogEvent("Sound Off");
    }
}
