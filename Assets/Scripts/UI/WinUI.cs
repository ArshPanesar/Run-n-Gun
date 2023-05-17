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
    }

    public void OnNextLevel()
    {
        // Reload Current Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;

        active = false;
        gameObject.SetActive(false);
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
