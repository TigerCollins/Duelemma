using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] NPCScript npcController;

//    [SerializeField] Animator //animatorCard;
    [SerializeField] SpriteRenderer spriteRendererCard;

    public UnityEvent onJump;
    public UnityEvent onGrounded;
    public UnityEvent onNoLongerGrounded;
    public UnityEvent<NPCScript.MovementState> onMoveInputStateChange;

    [Header("Debug")]
    [SerializeField] bool groundedState;
    [SerializeField] bool playerFacingRight;
    [SerializeField] NPCScript.MovementState movementState;

    public bool IsGrounded
    {
        set
        {
            if (value != groundedState)
            {
                groundedState = value;
                animator.SetBool("Grounded", value);
               // //animatorCard.SetBool("Grounded", value);
                if (value)
                {
                    onGrounded.Invoke();
                    animator.SetBool("Falling", false);
                  //  //animatorCard.SetBool("Falling", false);
                }


                else
                {
                    onNoLongerGrounded.Invoke();
                }

            }
        }
    }

    public void Init()
    {
        onJump.AddListener(delegate { Jump(); });
        npcController.characterEvents.onAttack.AddListener(delegate { Attack(); });
        npcController.characterEvents.onAbilityUsed.AddListener(delegate { AbilityUsed(); });
        npcController.characterEvents.onFalling.AddListener(delegate { IsFalling(); });
        // onGrounded.AddListener(delegate { AttemptIdleAnimationState(); });
        onMoveInputStateChange.AddListener(MovementStateChange);
    }

    private void Update()
    {
        //Grounded
        // animator.SetBool("Grounded", groundedState);
        DirectionChange();
        if (!npcController.IsGrounded && !npcController.IsFalling)
        {
            Jump();
        }

        else if (!npcController.IsGrounded && npcController.IsFalling)
        {
            IsFalling();
        }
    }


    void Jump()
    {
        if (!npcController.IsFalling)
        {
            animator.SetTrigger("Jump");
           // //animatorCard.SetTrigger("Jump");
        }

    }
    void Attack()
    {
        animator.SetTrigger("Attack");
       // //animatorCard.SetTrigger("Attack");
    }

    void AbilityUsed()
    {
        animator.SetTrigger("Ability");
       // //animatorCard.SetTrigger("Ability");
    }

    void IsFalling()
    {

        animator.SetBool("Falling", true);
       // //animatorCard.SetBool("Falling", true);
    }

    public void MovementStateChange(NPCScript.MovementState state)
    {
        if (state != movementState && npcController.IsGrounded)
        {
            movementState = state;
            if (npcController.CanMove)
            {
                animator.SetInteger("AnimState", (int)state);
              //  //animatorCard.SetInteger("AnimState", (int)state);
            }

            else
            {

            }


        }
    }

    public void AttemptIdleAnimationState()
    {
        if (npcController.IsGrounded)
        {
            onMoveInputStateChange.Invoke(NPCScript.MovementState.idle);
        }
    }

    public void Death()
    {
        if (npcController.IsGrounded)
        {
            animator.SetTrigger("Death");
        }
    }

    public void TookDamage()
    {

            animator.SetTrigger("Hurt");
        
    }

    void DirectionChange()
    {
        if (npcController.IsFacingRight != playerFacingRight)
        {
            playerFacingRight = npcController.IsFacingRight;
            spriteRenderer.flipX = playerFacingRight;
            spriteRendererCard.flipX = playerFacingRight;
        }
    }

    public Animator NPCAnimator
    {
        get
        {
            return animator;
        }
    }
}
