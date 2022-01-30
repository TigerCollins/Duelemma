using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using MoreMountains.Feedbacks;

public class CardObject : MonoBehaviour
{
    [SerializeField] MMFeedbacks cardFlipFeedback;
    float zRotation;

    PlayerAbilities playerAbilities;

    [Header("Logic")]
    [SerializeField] TextMeshGroups sideAText;
    [SerializeField] TextMeshGroups sideBText;
    [SerializeField] Renderer thisCardRenderer;
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
        PlayCardFlip();
        SetCardTextures();
        SetCardText();
    }

    void SetCardTextures()
    {
        if (thisCardRenderer != null)
        {
            thisCardRenderer.material.SetTexture("Back_Face_Texture", sideACardDetails.Art);
            thisCardRenderer.material.SetTexture("Card_Colour_Texture", sideBCardDetails.Art);
        }

    }

    void SetCardText()
    {
        sideAText.title.text = sideACardDetails.CardName;
        sideAText.body.text = sideACardDetails.UnlockedAbilityFlavour;
        sideBText.title.text = sideBCardDetails.CardName;
        sideBText.body.text = sideBCardDetails.UnlockedAbilityFlavour;
    }

    public void PlayCardFlip()
    {
        if (DimensionSwitcher.instance.CurrentDimension() == GlobalHelper.Dimensions.dimensionA)
        {

            if (cardFlipFeedback.TryGetComponent(out MMFeedbackRotation rotationFeedback))
            {
                rotationFeedback.DestinationAngles = new Vector3(90, 0, 180);
            }
        }

        else
        {
            if (cardFlipFeedback.TryGetComponent(out MMFeedbackRotation rotationFeedback))
            {
                rotationFeedback.DestinationAngles = new Vector3(90, 0, 0);
            }
        }
        cardFlipFeedback.Initialization();
        cardFlipFeedback.PlayFeedbacks();
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

[System.Serializable]
public class TextMeshGroups
{
    public TextMeshPro title;
    public TextMeshPro body;
}
