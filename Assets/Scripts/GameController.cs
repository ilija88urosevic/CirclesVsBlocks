using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject circleHolder;
    public PlayerAttack player;
    public Button hireHelpers;
    public TextMeshProUGUI hireHelpersText;
    public GameObject tutorialHolder;
    private void Awake()
    {
        hireHelpers.onClick.AddListener(BuyHelper);
        //player.Init(1,)
    }

    private void BuyHelper()
    {
        throw new NotImplementedException();
    }
}
