using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public Stopwatch sw;
    public Text text;
    public Text swText;
    public int max = 0;
    //private GameObject thisGo;

    void Start( )
    {
        //thisGo = this.gameObject;
        sw = new Stopwatch( );
    }

    void Update( )
    {
        //thisGo.SetActive(true);
        if(Time.time > 0.2f) {
            sw.Stop( );
            if((int)sw.ElapsedMilliseconds > max) {
                max = (int)sw.ElapsedMilliseconds;
            }
            text.text = max.ToString( );
            swText.text = sw.ElapsedMilliseconds.ToString();
            //print(sw.ElapsedMilliseconds);
            sw.Reset( );
            sw.Start( );
        }
    }
}
