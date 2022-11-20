using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : AttackerScript
{
    public Button attackButton;
    public new void Init(int level, EventHandler coinsEvent, Action<int, Transform> usedCoins)
    {
        attackButton.onClick.AddListener(Attack);
        base.Init(level, coinsEvent, usedCoins);
    }
}
