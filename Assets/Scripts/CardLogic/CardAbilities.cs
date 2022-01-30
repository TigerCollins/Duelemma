using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilities : MonoBehaviour
{
    public static CardAbilities instance;
    public enum Ability
    {
        DimensionSwap,
        Teleport,
        Dash,
        TimeStop,
        HangmanVine,
        RockThrow,
        ProjectileAttack
    }

    public List<CardDetails> abilityCards;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
     
    }
}
