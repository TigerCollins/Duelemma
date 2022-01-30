using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Scriptable Objects/Card Details", order = 1)]
public class CardDetails : ScriptableObject
{
    [SerializeField] CardAbilities.Ability cardAbility;
    [SerializeField] Texture cardArt;
    [SerializeField] string cardName;
    [SerializeField] string abilityLockedFlavour;
    [SerializeField] string abilityUnlockedFlavour;

    [Space(10)]

    [SerializeField] bool unlocked;
    [SerializeField] float cooldown;

    public float CardCooldown
    {
        get
        {
            return cooldown;
        }
    }

    public CardAbilities.Ability CardAbility
    {
        get
        {
            return cardAbility;
        }
    }

    public Texture Art
    {
        get
        {
            return cardArt;
        }
    }

    public string CardName
    {
        get
        {
            return cardName;
        }
    }
    public string LockedAbilityHint
    {
        get
        {
            return abilityLockedFlavour;
        }
    }
    public string UnlockedAbilityFlavour
    {
        get
        {
            return abilityUnlockedFlavour;
        }
    }

    public bool IsUnlocked
    {
        get
        {
            return unlocked;
        }

        set
        {
            unlocked = value;
        }

    }


}
