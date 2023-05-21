using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeUI : MonoBehaviour
{
    public Text coinsCollectedText;
    public Text coinsRequiredText;
    public Text coinsCollectedLabel;
    public Text coinsRequiredLabel;

    public Text failText;
    public Text maxUpgradeReachedText;

    public Image bulletImage;

    public Button upgradeButton;

    public Animator imageAnimator;

    private bool active = false;
    private int coinsCollected = 0;
    private int coinsRequired = 0;

    private PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        EventManager.GetInstance().AddListener(GameEvents.UpgradeWeaponMenu, OnUpgradeWeapon);
        EventManager.GetInstance().AddListener(GameEvents.PlayerDead, Close);
        EventManager.GetInstance().AddListener(GameEvents.FinishLevel, Close);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.UpgradeWeaponMenu, OnUpgradeWeapon);
        EventManager.GetInstance().RemoveListener(GameEvents.PlayerDead, Close);
        EventManager.GetInstance().RemoveListener(GameEvents.FinishLevel, Close);
    }

    void OnUpgradeWeapon(Dictionary<string, object> args)
    {
        active = !active;
        gameObject.SetActive(active);

        if (active)
        {
            coinsCollected = (int)args["coinsCollected"];
            coinsRequired = (int)args["coinsRequired"];
            imageAnimator.runtimeAnimatorController = (RuntimeAnimatorController)args["bulletAnimatorCont"];
            playerBehaviour = (PlayerBehaviour)args["playerBehaviour"];
            bool maxUpgraded = (bool)args["maxUpgraded"];

            if (maxUpgraded)
            {
                MaxUpgraded();
            }
            else
            {
                // Update UI
                coinsCollectedText.text = coinsCollected.ToString();
                coinsRequiredText.text = coinsRequired.ToString();
            }

            // Play Music
            FindObjectOfType<AudioManager>().Play("Pause");
        }
        else
        {
            OnReset();
        }
    }

    void Close(Dictionary<string, object> args)
    {
        active = false;
        gameObject.SetActive(false);

        OnReset();
    }

    public void OnUpgrade()
    {
        if (coinsCollected < coinsRequired)
        {
            failText.gameObject.SetActive(true);
        }
        else
        {
            // Upgrade Weapon
            playerBehaviour.UpgradeWeapon();

            // Close the Menu
            EventManager.GetInstance().TriggerEvent(GameEvents.UpgradeWeaponMenu, null);
        }
    }

    void MaxUpgraded()
    {
        coinsCollectedText.gameObject.SetActive(false);
        coinsRequiredText.gameObject.SetActive(false);

        coinsCollectedLabel.gameObject.SetActive(false);
        coinsRequiredLabel.gameObject.SetActive(false);

        failText.gameObject.SetActive(false);

        bulletImage.gameObject.SetActive(false);

        upgradeButton.gameObject.SetActive(false);

        maxUpgradeReachedText.gameObject.SetActive(true);
    }

    private void OnReset()
    {
        coinsCollectedLabel.gameObject.SetActive(true);
        coinsRequiredLabel.gameObject.SetActive(true);

        coinsCollectedText.gameObject.SetActive(true);
        coinsRequiredText.gameObject.SetActive(true);

        bulletImage.gameObject.SetActive(true);

        upgradeButton.gameObject.SetActive(true);

        failText.gameObject.SetActive(false);
        maxUpgradeReachedText.gameObject.SetActive(false);
    }
}

