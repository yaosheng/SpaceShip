using UnityEngine;
using System.Collections;

public class Continue : MonoBehaviour {

    public void ContinueToPlay( )
    {
        //GM.ADRManager.LogEvent("Press Continue Button");
        //GM.GameSetUp.AdsReportingManager.LogEvent("Press Continue Button");
        StartCoroutine(WaitToPlay( ));
    }

    IEnumerator WaitToPlay( )
    {
        float timer = 0;
        while(timer < 0.3f) {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;
        GM.GameSetUp.gameMode = GameMode.Playing;
    }
}
