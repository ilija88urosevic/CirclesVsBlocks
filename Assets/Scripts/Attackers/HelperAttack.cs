using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAttack : AttackerScript
{
    public Animation attackAnimation;
    public WaitForSeconds oneSecond = new WaitForSeconds(1);
    public new void Init(int level, Action<EventHandler> coinsEvent, Action<int,Transform> attackAction, ConfigurationSO config)
    {
        StartCoroutine(AttackBlock());
        base.Init(level,coinsEvent, attackAction, config);
    }

    private IEnumerator AttackBlock()
    {
        //Random wait to make it more interesting on screen
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2));
        while(true)
        {
            Attack();
            yield return oneSecond;
        }
        
    }

    public override void Attack()
    {
        attackAnimation.Play();
        base.Attack();
    }
}
