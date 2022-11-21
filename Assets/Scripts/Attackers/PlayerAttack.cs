using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : AttackerScript
{
    public Button attackButton;
    public new void Init(PlayerDataSO data, Action<EventHandler> coinsEvent, Action<int, Transform> attackAction, ConfigurationSO config, int id)
    {
        attackButton.onClick.AddListener(Attack);
        base.Init(data, coinsEvent, attackAction, config, id);
    }

}
