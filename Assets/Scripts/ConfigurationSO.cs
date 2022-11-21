using com.spacepuppy.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Configuration
{
    public int maxCircles = 5;
    public int firstCircleCost = 100;
    public string goldPerTapFormula = "5*(upgradeLevel^2.1)";
    public string upgradeCostFormula = "5*(1.08^upgradeLevel)";
    public int defaultCoins = 50;
    public string defaultData = "1";

    public Configuration(string data)
    {
        string[] dataArray = data.Split('#');
        maxCircles = int.Parse(dataArray[0]);
        firstCircleCost = int.Parse(dataArray[1]);
        goldPerTapFormula = dataArray[2];
        upgradeCostFormula = dataArray[3];
    }
    public override string ToString()
    {
        return maxCircles+"#"+ firstCircleCost + "#" + goldPerTapFormula + "#" + upgradeCostFormula;
    }
}
[CreateAssetMenu(fileName = "ConfigurationSO", menuName = "ScriptableObjects/ConfigurationSO", order = 1)]
public class ConfigurationSO : ScriptableObject
{
    [SerializeField] 
    private Configuration configuration;
    internal int DefaultCoins => configuration.defaultCoins;
    internal string DefaultData => configuration.defaultData;

    internal int FirstCircleCost => configuration.firstCircleCost;
    internal int MaxCircles => configuration.maxCircles;

    internal void LoadData(string data)
    {
        configuration = new Configuration(data);
    }

    internal string GetStringData() => configuration.ToString();

    internal int GetUpgradeCost(int upgradeLevel)
    {
        float evaluatedValue = float.Parse(Evaluator.EvalString(GetUpgradeString(upgradeLevel), null));
        return Mathf.RoundToInt(evaluatedValue);
    }

    private string GetUpgradeString(int upgradeLevel)
    {
        return configuration.upgradeCostFormula.Replace("upgradeLevel", upgradeLevel.ToString());
    }
    private string GetTapString(int upgradeLevel)
    {
        return configuration.goldPerTapFormula.Replace("upgradeLevel", upgradeLevel.ToString());
    }

    internal int GetCoinsOnTap(int upgradeLevel)
    {
        float evaluatedValue = float.Parse(Evaluator.EvalString(GetTapString(upgradeLevel), null));
        return Mathf.RoundToInt(5 * Mathf.Pow(upgradeLevel,2.1f)); 
    }
}
