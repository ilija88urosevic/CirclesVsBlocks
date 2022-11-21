using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Animation block;
    public GameObject circlePrefab;
    public Transform circleHolder;
    public PlayerAttack player;
    public Button hireHelpers;
    public TextMeshProUGUI hireHelpersCost;
    public GameObject maxHelpers;
    public TextMeshProUGUI hireHelpersPriceText;
    public GameObject tutorialHolder;
    public CoinsManager coinsManager;
    public EffectsPool effectsPool = new EffectsPool();
    private ConfigurationSO config;
    private PlayerDataSO data;
    private List<HelperAttack> helpers;
    private void Awake()
    {
        //todo data
        config = GameManager.Instance.RequestConfig();
        data = GameManager.Instance.RequestData();
        player.Init(data.PlayerLevel, coinsManager.Subscribe, Attacked, config);
        foreach (var data in data.CircleLevels)
        {
            SpawnHelper(false, data);
        }

        coinsManager.SetCoins(data.Coins);
        hireHelpers.onClick.AddListener(BuyHelper);
        if (SaveManager.GetInt("FirstTutorial", 0) == 0)
        {
            tutorialHolder.SetActive(true);
            SaveManager.SetInt("FirstTutorial", 1);
        }
        hireHelpersCost.text = HelperCost() + "<sprite=0>";
        if (DisableHelpersButton())
        {
            hireHelpers.interactable = false;
            if (config.MaxCircles <= helpers.Count)
                maxHelpers.SetActive(true);
        }
        else
        {
            hireHelpers.interactable = true;
        }
    }

    private bool DisableHelpersButton()
    {
        return coinsManager.CurrentCoins < HelperCost() || config.MaxCircles <= helpers.Count;
    }
    private void BuyHelper()
    {
        SpawnHelper(true, 1);
        data.Save();
    }

    private void SpawnHelper(bool reduceCoins, int level)
    {
        if (helpers == null)
        {
            helpers = new List<HelperAttack>();
        }
        if (reduceCoins)
        {
            coinsManager.ChangeCoins(-HelperCost());
            data.SaveCoins();
        }
        var newHelper = (Instantiate(circlePrefab, circleHolder)).GetComponent<HelperAttack>();
        newHelper.Init(level, coinsManager.Subscribe, Attacked, config);
        helpers.Add(newHelper);
        if (DisableHelpersButton())
        {
            hireHelpers.interactable = false;
            if (config.MaxCircles <= helpers.Count)
                maxHelpers.SetActive(true);
        }
        else
        {
            hireHelpers.interactable = true;
        }
        hireHelpersCost.text = HelperCost() + "<sprite=0>";
    }

    private void Attacked(int amount, Transform position)
    {
        //TODO: create a sepparate attack and upgrade events
        Debug.Log(amount + " " + position.gameObject.name);
        if (amount > 0)
            block.Play();
        effectsPool.CoinsAnimations(amount, position);
        coinsManager.ChangeCoins(amount);
        if (DisableHelpersButton())
        {
            hireHelpers.interactable = false;
            if (config.MaxCircles <= helpers.Count)
                maxHelpers.SetActive(true);
        }
        else
        {
            hireHelpers.interactable = true;
        }
        data.SaveCoins();

    }

    private int HelperCost()
    {
        if (helpers == null)
        {
            helpers = new List<HelperAttack>();
        }
        return config.FirstCircleCost * (int)Math.Pow(10, helpers.Count);
    }
}

