using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public Text coinsCollectedText;

    private bool active = false;

    private void Awake()
    {
        EventManager.GetInstance().AddListener(GameEvents.FinishLevel, OnFinishLevel);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.FinishLevel, OnFinishLevel);
    }

    public void OnFinishLevel(Dictionary<string, object> args)
    {
        Time.timeScale = 0f;

        int coinsCollected = (int)args["coins"];
        coinsCollectedText.text = coinsCollected.ToString();

        active = !active;
        gameObject.SetActive(active);

        // Play Music
        FindObjectOfType<AudioManager>().Stop("Level 1 Background");
        FindObjectOfType<AudioManager>().Play("Finish Level");
    }

    public void OnNextLevel()
    {
        // Reload Current Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;

        FindObjectOfType<AudioManager>().Stop("Finish Level");
        FindObjectOfType<AudioManager>().Play("Level 1 Background");

        active = false;
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main Menu");
    }
}
