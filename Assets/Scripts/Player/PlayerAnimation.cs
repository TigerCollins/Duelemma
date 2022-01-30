using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation instance;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerController playerController;

    [SerializeField] Animator animatorCard;
    [SerializeField] SpriteRenderer spriteRendererCard;

    public UnityEvent onJump;
    public UnityEvent onGrounded;
    public UnityEvent onNoLongerGrounded;
    public UnityEvent<PlayerController.MovementState> onMoveInputStateChange;

    [Header("Debug")]
    [SerializeField] bool groundedState;
    [SerializeField] bool playerFacingRight;
    [SerializeField] PlayerController.MovementState movementState;

    private void Awake()
    {
        instance = this;
    }

    public bool IsGrounded
    {
        set
        {
            if(value != groundedState)
            {
                groundedState = value;
                animator.SetBool("Grounded", value);
                animatorCard.SetBool("Grounded", value);
                if (value)
                {
                    onGrounded.Invoke();
                    animator.SetBool("Falling", false);
                    animatorCard.SetBool("Falling", false);
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
        playerController.characterEvents.onAttack.AddListener(delegate { Attack(); });
        playerController.characterEvents.onAbilityUsed.AddListener(delegate { AbilityUsed(); });
        playerController.characterEvents.onFalling.AddListener(delegate { IsFalling(); });
       // onGrounded.AddListener(delegate { AttemptIdleAnimationState(); });
        onMoveInputStateChange.AddListener(MovementStateChange);
    }

    private void Update()
    {
        //Grounded
       // animator.SetBool("Grounded", groundedState);
        DirectionChange();
        if(!playerController.IsGrounded && !playerController.IsFalling)
        {
            Jump();
        }

        else if(!playerController.IsGrounded && playerController.IsFalling)
        {
            IsFalling();
        }
    }


    void Jump()
    {
        if(!playerController.IsFalling)
        {
            animator.SetTrigger("Jump");
            animatorCard.SetTrigger("Jump");
        }
        
    }

    public void Death()
    {
        if (playerController.IsGrounded)
        {
            animator.SetTrigger("Death");
        }
    }

    public void TookDamage()
    {

        animator.SetTrigger("Hurt");

    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        animatorCard.SetTrigger("Attack");
    }

    void AbilityUsed()
    {
        animator.SetTrigger("Ability");
        animatorCard.SetTrigger("Ability");
    }

    void IsFalling()
    {

            animator.SetBool("Falling", true);
            animatorCard.SetBool("Falling", true);
    }

    public void MovementStateChange(PlayerController.MovementState state)
    {
        if(state != movementState && playerController.IsGrounded)
        {
            movementState = state;
            if (playerController.CanMove)
            {
                animator.SetInteger("AnimState", (int)state);
                animatorCard.SetInteger("AnimState", (int)state);
            }

            else
            {

            }
         
          
        }       
    }

    public void AttemptIdleAnimationState()
    {
        if(playerController.IsGrounded)
        {
            onMoveInputStateChange.Invoke(PlayerController.MovementState.idle);
        }
    }

    void DirectionChange()
    {
        if(playerController.IsFacingRight != playerFacingRight)
        {
            playerFacingRight = playerController.IsFacingRight;
            spriteRenderer.flipX = playerFacingRight;
            spriteRendererCard.flipX = playerFacingRight;
        }
    }

    public Animator GetPlayerAnimator
    {
        get
        {
            return animator;
        }
    }

    public Animator GetCardAnimator
    {
        get
        {
            return animatorCard;
        }
    }
    

}
