using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPosition : MonoBehaviour {

    public Transform targetTransform;
    public RectTransform thisTransform;
	// Use this for initialization
	void Start () {
        thisTransform = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        //transform.position = UIToWorld(thisTransform, targetTransform.position);
    }

    void ShowCameraPosition( )
    {

    }

    static public Vector3 UIToWorld( RectTransform r, Vector3 uiPos )
    {
        float width = r.rect.width / 2; //UI一半的寬
        float height = r.rect.height / 2; //UI一半的高
        Vector3 screenPos = new Vector3(((uiPos.x / width) + 1f) / 2, ((uiPos.y / height) + 1f) / 2, uiPos.z); //須小心Z座標的位置
        return Camera.main.ViewportToWorldPoint(screenPos);
    }
}
