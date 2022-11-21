using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : AttackerScript
{
    public Button attackButton;
    public new void Init(int level, Action<EventHandler> coinsEvent, Action<int, Transform> attackAction, ConfigurationSO config)
    {
        attackButton.onClick.AddListener(Attack);
        base.Init(level, coinsEvent, attackAction, config);
    }

}
