using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopToHome : MonoBehaviour {

    public void ReTurnToHome( )
    {
        SceneManager.LoadScene("GameStart");
        Time.timeScale = 1;
        GameSetUp.isOpened = false;
    }
}
