using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCard : MonoBehaviour
{

    [SerializeField] CardAbilities.Ability abilityToUnlock;
    [SerializeField] CardAbilities.Ability secondAbilityToUnlock;


   public void UnlockAbility()
    { 
            PlayerAbilities.instance.UnlockAbility(abilityToUnlock);
            PlayerAbilities.instance.UnlockAbility(secondAbilityToUnlock);
            Destroy(this.gameObject);
        
    }
}
