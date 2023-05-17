using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text text;

    void Start()
    {
        EventManager.GetInstance().AddListener(GameEvents.CollectCoins, UpdateScore);
    }

    void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.CollectCoins, UpdateScore);
    }

    // Events
    //
    // Change Score UI
    void UpdateScore(Dictionary<string, object> args)
    {
        int score = (int)args["score"];

        text.text = score.ToString();
    }
}
