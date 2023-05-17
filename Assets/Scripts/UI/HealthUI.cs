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

        if (health > 0)
        {
            text.text = health.ToString();
        }
        else
        {
            // Death Message
            text.text = "DEAD";
            //transform.position -= new Vector3(5.0f, 0.0f);

            // Remove UI
            GetComponentInChildren<Image>().enabled = false;
        }
    }
}
