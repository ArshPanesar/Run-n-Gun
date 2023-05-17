using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void Start()
    {
        // Play Main Menu Music
        FindObjectOfType<AudioManager>().Play("Main Menu Theme");
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Level 1");

        // Stop Playing Music
        FindObjectOfType<AudioManager>().Stop("Main Menu Theme");

        // Play First Level Music
        FindObjectOfType<AudioManager>().Play("Level 1 Background");
    }

    public void OnQuit()
    {
#if (UNITY_WEBGL)
        // For a WebGL Build, Go to an Empty Web Page
        Application.ExternalEval("window.open('about:blank','_self')");
        //Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
        // Stop Playing Music
        FindObjectOfType<AudioManager>().Stop("Main Menu Theme");
    }
}
