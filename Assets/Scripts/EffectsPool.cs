using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectsPool : MonoBehaviour
{
    [SerializeField]
    public List<CoinsAnimationScript> pool;
    public GameObject coinsAnimationPrefab;
    public Transform poolHolder;
    public void CoinsAnimations(int amount, Transform position)
    {
        var effect = pool.Where(item => !item.animating).FirstOrDefault();
        if (effect == null)
        {
            effect = Instantiate(coinsAnimationPrefab, poolHolder).GetComponent<CoinsAnimationScript>();
            pool.Add(effect);
        }
        effect.gameObject.SetActive(true);
        effect.Animate(amount, position, transform);
    }
}
