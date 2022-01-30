using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCard : MonoBehaviour
{

    [SerializeField] CardAbilities.Ability abilityToUnlock;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerAbilities playerAbilities))
        {
            playerAbilities.UnlockAbility(abilityToUnlock);
            Destroy(this.gameObject);
        }
    }
}
