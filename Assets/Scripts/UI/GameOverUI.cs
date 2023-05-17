using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private bool active = false;

    private void Awake()
    {
        EventManager.GetInstance().AddListener(GameEvents.PlayerDead, OnGameOver);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.PlayerDead, OnGameOver);
    }

    public void OnGameOver(Dictionary<string, object> args)
    {
        active = !active;
        gameObject.SetActive(active);
    }

    public void OnRestart()
    {
        // Reload Current Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<AudioManager>().Play("Level 1 Background");

        active = false;
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("Main Menu");

        FindObjectOfType<AudioManager>().Stop("Level 1 Background");
    }
}
