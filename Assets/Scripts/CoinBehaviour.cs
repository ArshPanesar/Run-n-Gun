using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private void Start()
    {
        EventManager.GetInstance().AddListener(GameEvents.CollectCoins, DestroyCollectedCoin);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.CollectCoins, DestroyCollectedCoin);
    }

    public void DestroyCollectedCoin(Dictionary<string, object> args)
    {
        GameObject coin = (GameObject)args["coin"];
        Destroy(coin, 0.1f);
    }
}
