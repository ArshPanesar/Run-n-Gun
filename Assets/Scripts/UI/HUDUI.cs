using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    public Text upgradeWeaponPrompt;

    private void Awake()
    {
        EventManager.GetInstance().AddListener(GameEvents.CanUpgradeWeapon, OpenUpgradeWeaponPrompt);
        EventManager.GetInstance().AddListener(GameEvents.WeaponUpgraded, CloseUpgradeWeaponPrompt);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.CanUpgradeWeapon, OpenUpgradeWeaponPrompt);
        EventManager.GetInstance().RemoveListener(GameEvents.WeaponUpgraded, CloseUpgradeWeaponPrompt);
    }

    void OpenUpgradeWeaponPrompt(Dictionary<string, object> args)
    {
        upgradeWeaponPrompt.gameObject.SetActive(true);
    }

    void CloseUpgradeWeaponPrompt(Dictionary<string, object> args)
    {
        upgradeWeaponPrompt.gameObject.SetActive(false);
    }
}
