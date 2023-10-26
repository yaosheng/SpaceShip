using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TutorialClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IScrollHandler, IDragHandler {

    public virtual void OnPointerDown( PointerEventData data )
    {
        Debug.Log("on point down");
        TutorialSpaceShip.isbuttonDown = true;
    }

    public virtual void OnPointerUp( PointerEventData data )
    {
        Debug.Log("on point up");
        TutorialSpaceShip.isbuttonDown = false;
    }

    public virtual void OnScroll( PointerEventData data )
    {
        Debug.Log("Scroll");
    }

    public virtual void OnDrag( PointerEventData data )
    {
        Debug.Log("Drag");
    }
}
