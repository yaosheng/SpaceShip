using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{

    public void GoToTutorial( )
    {
        SceneManager.LoadScene("TutorialScene");
    }
}

