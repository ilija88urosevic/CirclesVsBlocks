using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsAnimationScript : MonoBehaviour
{
    [HideInInspector]
    public bool animating;
    public TextMeshProUGUI text;
    public Animation coinAnimation;
    private Transform poolHolder;
    internal void Animate(int amount, Transform position, Transform pool)
    {
        animating = true;
        poolHolder = pool;
        bool addCoins = false;
        if (amount > 0)
            addCoins = true;
        text.text = (addCoins ? "+" : "" )+ amount + "<sprite=0>";
        coinAnimation.Play(AnimationName(addCoins));
        transform.parent = position;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    public void AnimationFinished()
    {
        //Called from animation event
        gameObject.SetActive(false);
        animating = false;
        transform.parent = poolHolder;
    }

    private string AnimationName(bool addCoins)
    {
        return addCoins ? "GainedCoinAnim" : "SpendCoinAnim";
    }
}
