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
    public Transform parentTransform;
     [Header("Logic")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI subtext;
    [SerializeField] Image cardImage;
    [SerializeField] CardDetails sideACardDetails;
    [SerializeField] CardDetails sideBCardDetails;



    [SerializeField] bool aSideCardIsReady = true;
    [SerializeField] bool bSideCardIsReady = true;
    Coroutine fillCoroutine;

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
          ///  cardImage.sprite = sideACardDetails.Art;
        }

        else
        {
           // cardImage.sprite = sideBCardDetails.Art;
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

 

    public bool BothSidesLocked()
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
           // print("okefeqw");
           
            if (DimensionSwitcher.instance != null)
            {
                if (DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
                {
                    if (sideACardDetails.IsUnlocked && aSideCardIsReady)
                    {
                        PlayerController.instance.characterEvents.onAbilityUsed.Invoke();
                        PlayerController.instance.HaltMovement();
                        switch (sideACardDetails.CardAbility)
                        {
                            case CardAbilities.Ability.DimensionSwap:
                                DimensionSwitcher.instance.onDimensionChange.Invoke();
                                break;
                            case CardAbilities.Ability.Teleport:
                                PlayerAbilities.instance.ThrowTeleportCard();
                                break;
                            case CardAbilities.Ability.Dash:
                                PlayerAbilities.instance.DashMovement();
                                break;
                            case CardAbilities.Ability.TimeStop:
                                PlayerAbilities.instance.FreezeTimeAbility();
                                break;
                            case CardAbilities.Ability.HangmanVine:
                                PlayerAbilities.instance.SuspendTarget();
                                break;
                            case CardAbilities.Ability.RockThrow:
                                PlayerAbilities.instance.ThrowRock();
                                break;
                            case CardAbilities.Ability.ProjectileAttack:
                                PlayerAbilities.instance.ProjectileAttack();
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
                        PlayerController.instance.HaltMovement();

                        PlayerController.instance.characterEvents.onAbilityUsed.Invoke();
                        switch (sideBCardDetails.CardAbility)
                        {
                            case CardAbilities.Ability.DimensionSwap:
                                DimensionSwitcher.instance.onDimensionChange.Invoke();
                                break;
                            case CardAbilities.Ability.Teleport:
                                PlayerAbilities.instance.ThrowTeleportCard();
                                break;
                            case CardAbilities.Ability.Dash:
                                PlayerAbilities.instance.DashMovement();
                                break;
                            case CardAbilities.Ability.TimeStop:
                                PlayerAbilities.instance.FreezeTimeAbility();
                                break;
                            case CardAbilities.Ability.HangmanVine:
                                PlayerAbilities.instance.SuspendTarget();
                                break;
                            case CardAbilities.Ability.RockThrow:
                                PlayerAbilities.instance.ThrowRock();
                                break;
                            case CardAbilities.Ability.ProjectileAttack:
                                PlayerAbilities.instance.ProjectileAttack();
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
        if(fillCoroutine != null)
        {
          //  StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(UpdateFillAmount(cooldown, cardSide));
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

    public IEnumerator UpdateFillAmount(float cooldown, GlobalHelper.CardSide cardSide)
    {
        float t = 0;
        float timer = 0;
        switch (cardSide)
        {
            case GlobalHelper.CardSide.aSide:
                while(t<cooldown)
                {
                    timer = t / cooldown;
                    GUIHandler.instance.UpdateSideAFill(timer);
                    t += Time.deltaTime;
                    yield return null;
                }
                break;
            case GlobalHelper.CardSide.bSide:
                while (t < cooldown)
                {
                    timer = t / cooldown;
 
                    GUIHandler.instance.UpdateSideBFill(timer);
                    t += Time.deltaTime;
                    yield return null;
                }
                break;
            default:
                break;
        }
        
    }

}

