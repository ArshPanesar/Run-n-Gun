using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Text text;

    void Start()
    {
        EventManager.GetInstance().AddListener(GameEvents.UpdateHealth, UpdateHealth);
    }

    void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.UpdateHealth, UpdateHealth);
    }

    // Events
    //
    // Change Health UI
    void UpdateHealth(Dictionary<string, object> args)
    {
        int health = (int)args["health"];

        text.text = health.ToString();
    }
}
