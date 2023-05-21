using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play("Main Menu Theme");
    }

    public void OnPlay()
    {
        FindObjectOfType<LevelLoader>().LoadNextLevel();

        // Stop Playing Music
        FindObjectOfType<AudioManager>().Stop("Main Menu Theme");
    }

    public void OnQuit()
    {
#if (UNITY_WEBGL)
        // For a WebGL Build, Go to an Empty Web Page
        Application.ExternalEval("window.open('about:blank','_self')");
#else
        Application.Quit();
#endif

        // Stop Playing Music
        FindObjectOfType<AudioManager>().Stop("Main Menu Theme");
    }
}
