using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAttack : AttackerScript
{
    public Animation attackAnimation;
    public WaitForSeconds oneSecond = new WaitForSeconds(1);
    public new void Init(int level, EventHandler coinsEvent, Action<int,Transform> usedCoins)
    {
        StartCoroutine(AttackBlock());
        base.Init(level,coinsEvent,usedCoins);
    }

    private IEnumerator AttackBlock()
    {
        //Random wait to make it more interesting on screen
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2));
        while(true)
        {
            yield return oneSecond;
        }
    }

    public override void Attack()
    {
        attackAnimation.Play();
        base.Attack();
    }
}
