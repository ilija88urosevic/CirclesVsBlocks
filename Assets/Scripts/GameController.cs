using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Animation block;
    public GameObject circlePrefab;
    public Transform circleHolder;
    public PlayerAttack player;
    public Button hireHelpers;
    public Button resetGame;
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
        config = GameManager.Instance.RequestConfig();
        data = GameManager.Instance.RequestData();
        coinsManager.Init(data);
        helpers = new List<HelperAttack>();
        resetGame.onClick.AddListener(ResetAndDelete);
        player.Init(data, coinsManager.Subscribe, Attacked, config,0);
        foreach (var data in data.CircleLevels)
        {
            SpawnHelper(false, data, helpers.Count+1);
        }

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

    private void ResetAndDelete()
    {
        PlayerPrefs.DeleteAll();
        DestroyImmediate(GameManager.Instance);
        SceneManager.LoadScene(0);
    }

    private bool DisableHelpersButton()
    {
        return coinsManager.CurrentCoins < HelperCost() || config.MaxCircles <= helpers.Count;
    }
    private void BuyHelper()
    {
        data.playerData.tapUpgradeLevels.Add(1); 
        SpawnHelper(true, 1, helpers.Count+1);
        data.Save();
    }

    private void SpawnHelper(bool reduceCoins, int level, int id)
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
        newHelper.Init(data, coinsManager.Subscribe, Attacked, config,id);
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

