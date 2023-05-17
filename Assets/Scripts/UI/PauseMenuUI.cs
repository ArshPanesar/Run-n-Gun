using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    private bool active = false;

    private void Awake()
    {
        EventManager.GetInstance().AddListener(GameEvents.Paused, OnPause);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.Paused, OnPause);
    }

    public void OnPause(Dictionary<string, object> args)
    {
        active = !active;
        gameObject.SetActive(active);

        if (active)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void OnResume()
    {
        Time.timeScale = 1f;

        active = false;
        gameObject.SetActive(false);
    }

    public void OnUpgradeWeapon()
    {

    }

    public void OnQuit()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Main Menu");
    }
}
