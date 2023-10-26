using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopButton : MonoBehaviour {

    public void GoToShop( )
    {
        SceneManager.LoadScene("Shop");
    }
}
