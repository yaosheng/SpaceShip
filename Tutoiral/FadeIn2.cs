using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn2 : MonoBehaviour {

    public float alpha;
    public Image project;
    private float timer = 2.0f;

    void Start( )
    {
        project = gameObject.GetComponent<Image>( );
    }

    void Update( )
    {
        project.color = new Color(1, 1, 1, alpha);
    }

    public void ProjectFadeIn( )
    {
        Debug.Log("fade in 2");
        iTween.ValueTo(project.gameObject, iTween.Hash("from", 0, "to", 1, "time", timer, "easeType", "easeOutSine", "onupdatetarget", this.gameObject, "onupdate", "OnColorUpdated1"));
    }

    private void OnColorUpdated1( float tempColor )
    {
        alpha = tempColor;
    }
}
