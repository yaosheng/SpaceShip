using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn1 : MonoBehaviour {

    public float alpha;
    public Image project;
    public Image backtoHomeButton;
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
        Debug.Log("fade in 1");
        iTween.ValueTo(project.gameObject, iTween.Hash("from", 0, "to", 1, "time", timer, "easeType", "easeOutSine", "onupdatetarget", this.gameObject, "onupdate", "OnColorUpdated1"));
        StartCoroutine(WaitItween(timer));
    }

    private void OnColorUpdated1( float tempColor )
    {
        alpha = tempColor;
    }

    IEnumerator WaitItween(float temp)
    {
        yield return new WaitForSeconds(temp);
        backtoHomeButton.gameObject.SetActive(true);
    }
}
