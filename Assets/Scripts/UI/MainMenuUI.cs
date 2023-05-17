using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
