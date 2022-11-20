using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Configuration
{
    public float maxCircles = 5;
    public float firstCircleCost = 100;
    public string goldPerTapFormula = "5*upgradeLevel^2.1";
    public string upgradeCostFormula = "5*1.08^upgradeLevel";

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
    internal void LoadData(string data)
    {
        configuration = new Configuration(data);
    }

    internal string GetStringData() => configuration.ToString();
}
