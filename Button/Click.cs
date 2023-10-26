using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IDragHandler {

    private AdsReportingManager AdsReportingManager;

    void Awake( )
    {
        AdsReportingManager = GameObject.FindObjectOfType(typeof(AdsReportingManager)) as AdsReportingManager;
    }

    public virtual void OnPointerDown( PointerEventData data )
    {
        Debug.Log("on point down");
        SpaceShip.isbuttonDown = true;
    }

    public virtual void OnPointerUp( PointerEventData data )
    {
        Debug.Log("on point up");
        SpaceShip.isbuttonDown = false;
    }

    public virtual void OnScroll( PointerEventData data )
    {
        Debug.Log("Scroll");
        AdsReportingManager.LogEvent("Scroll On The Screen");
    }

    public virtual void OnDrag( PointerEventData data )
    {
        Debug.Log("Drag");
        AdsReportingManager.LogEvent("Drag On The Screen");
    }
}
