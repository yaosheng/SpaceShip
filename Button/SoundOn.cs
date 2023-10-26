using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SoundOn : MonoBehaviour, IPointerDownHandler
{
    public virtual void OnPointerDown( PointerEventData data )
    {
        Debug.Log("Sound On");
        GM.SoundManager.isSoundTurnOn = true;
        GM.UIManager.soundOpen.gameObject.SetActive(true);
        GM.UIManager.soundClose.gameObject.SetActive(false);
        GM.UIManager.soundOpen1.gameObject.SetActive(true);
        GM.UIManager.soundClose1.gameObject.SetActive(false);
        PlayerPrefs.SetInt("SoundControl", 1);
        //GM.ADRManager.LogEvent("Sound On");
        //GM.GameSetUp.AdsReportingManager.LogEvent("Sound On");
    }
}
