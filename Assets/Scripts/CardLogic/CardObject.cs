using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using MoreMountains.Feedbacks;

public class CardObject : MonoBehaviour
{
    PlayerAbilities playerAbilities;

    [Header("Logic")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI subtext;
    [SerializeField] Image cardImage;
    [SerializeField] CardDetails sideACardDetails;
    [SerializeField] CardDetails sideBCardDetails;



    bool aSideCardIsReady = true;
    bool bSideCardIsReady = true;

    private void Start()
    {
        playerAbilities = PlayerAbilities.instance;
    }

    public void Init()
    {
       // PlayCardFlip();
        SetCardTextures();
        SetCardText();
       
    }

    public void SetCardTextures()
    {            
        if (DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
        {
            cardImage.sprite = sideACardDetails.Art;
        }

        else
        {
            cardImage.sprite = sideBCardDetails.Art;
        }
    }

    public void SetCardText()
    {
        if(DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
        {
            title.text = sideACardDetails.CardName;
            if(subtext != null)
            {
                subtext.text = sideACardDetails.UnlockedAbilityFlavour;
            }
        }

        else
        {
            title.text = sideBCardDetails.CardName;
            if (subtext != null)
            {
                subtext.text = sideBCardDetails.UnlockedAbilityFlavour;
            }
        }
    }

 

    bool BothSidesLocked()
    {
        bool bothLocked = false;
        if (!sideACardDetails.IsUnlocked && !sideBCardDetails.IsUnlocked)
        {
            bothLocked = true;
        }

        return bothLocked;
    }

    public void ActivateAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            PlayerController.instance.characterEvents.onAbilityUsed.Invoke();
            if (DimensionSwitcher.instance != null)
            {
                if (DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
                {
                    if (sideACardDetails.IsUnlocked && aSideCardIsReady)
                    {
                        switch (sideACardDetails.CardAbility)
                        {
                            case CardAbilities.Ability.DimensionSwap:
                                DimensionSwitcher.instance.onDimensionChange.Invoke();
                                break;
                            case CardAbilities.Ability.Teleport:
                                playerAbilities.ThrowTeleportCard();
                                break;
                            case CardAbilities.Ability.Dash:
                                playerAbilities.DashMovement();
                                break;
                            case CardAbilities.Ability.TimeStop:
                                playerAbilities.FreezeTimeAbility();
                                break;
                            case CardAbilities.Ability.HangmanVine:
                                playerAbilities.SuspendTarget();
                                break;
                            case CardAbilities.Ability.RockThrow:
                                playerAbilities.ThrowRock();
                                break;
                            case CardAbilities.Ability.ProjectileAttack:
                                playerAbilities.ProjectileAttack();
                                break;
                            default:
                                break;
                        }
                        StartCoroutine(Cooldown(sideACardDetails.CardCooldown, GlobalHelper.CardSide.aSide));
                    }
                }

                else
                {
                    if (sideBCardDetails.IsUnlocked && bSideCardIsReady)
                    {
                        switch (sideBCardDetails.CardAbility)
                        {
                            case CardAbilities.Ability.DimensionSwap:
                                DimensionSwitcher.instance.onDimensionChange.Invoke();
                                break;
                            case CardAbilities.Ability.Teleport:
                                playerAbilities.ThrowTeleportCard();
                                break;
                            case CardAbilities.Ability.Dash:
                                playerAbilities.DashMovement();
                                break;
                            case CardAbilities.Ability.TimeStop:
                                playerAbilities.FreezeTimeAbility();
                                break;
                            case CardAbilities.Ability.HangmanVine:
                                playerAbilities.SuspendTarget();
                                break;
                            case CardAbilities.Ability.RockThrow:
                                playerAbilities.ThrowRock();
                                break;
                            case CardAbilities.Ability.ProjectileAttack:
                                playerAbilities.ProjectileAttack();
                                break;
                            default:
                                break;
                        }
                        StartCoroutine(Cooldown(sideBCardDetails.CardCooldown, GlobalHelper.CardSide.bSide));
                    }
                }
            }
        }
    }

    public IEnumerator Cooldown(float cooldown, GlobalHelper.CardSide cardSide)
    {
        switch (cardSide)
        {
            case GlobalHelper.CardSide.aSide:
                aSideCardIsReady = false;
                yield return new WaitForSeconds(cooldown);
                aSideCardIsReady = true;
                break;
            case GlobalHelper.CardSide.bSide:
                bSideCardIsReady = false;
                yield return new WaitForSeconds(cooldown);
                bSideCardIsReady = true;
                break;
            default:
                break;
        }
    }
}

