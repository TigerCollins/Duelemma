using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
	//public GameController gameController;

	bool receivingMovementInput;
	[SerializeField] bool canMove = true;
	[HideInInspector] public bool pauseMove = false;


	[Header("Player Settings")]
	public static PlayerController instance;
	[SerializeField] StatsController statsController;
	[SerializeField] PlayerAnimation playerAnimator;
	public CharacterController characterController;
	[SerializeField] float physicsPushPower;
	NPCScript npcScript;


	[SerializeField] MovementDetails movement;
	[SerializeField] float knockbackTime = 5;
	[SerializeField] internal CharacterEvents characterEvents;
	[SerializeField] List<DirectionBasedObjectFlip> directionBasedObjectFlips;
	 Vector3 moveDirection = Vector3.zero;
	 Vector3 secondaryMoveDirection = Vector3.zero;
	UnityEvent onUpdateCalled = new UnityEvent();

	

	[SerializeField] bool isFacingRight;

	[Header("Raycast")]
	[SerializeField] RaycastDetails _raycast;

	[Header("Position Tracking")]
	[SerializeField] bool isSafe;
	[SerializeField] Vector3 lastSafePosition;
	[SerializeField] float tickRate;

	[Header("Debug")]
	[SerializeField] Transform playerTransform;
	[SerializeField] Vector2 moveAxis;
	Vector3 originalPos;
	[SerializeField] bool isGrounded;
	[SerializeField] bool isHittingWall;
	bool paused;

	public enum MovementState
    {
		idle,
		combatIdle,
		Sprint,
		Jumping,
		Freefall
    }

	// Initialization
	private void Awake()
	{
		instance = this;
		originalPos = playerTransform.position;
		playerAnimator.Init();
		InitAllDirectionBasedObjects();
		InitStatEvents();
	}

	void InitStatEvents()
	{
		statsController.onDeath.AddListener(delegate { playerAnimator.Death(); });
		statsController.onHealthLost.AddListener(delegate { playerAnimator.TookDamage(); });
		statsController.onHealthLost.AddListener(delegate { GetKnockBack(); });
	}


	void Start()
	{
		StartCoroutine(PositionTracking());
	}

	public CharacterController PlayerCharacterController
    {
		get
        {
			return characterController;
        }
    }

	public IEnumerator PositionTracking()
	{
		if (isSafe && IsGrounded)
		{
			lastSafePosition = transform.position;
		}

		yield return new WaitForSeconds(tickRate);
		StartCoroutine(PositionTracking());
	}

	// Main tick
	void FixedUpdate()
	{
		//Movement();

		ApplyMovement();

		

	}

    public void Update()
    {
		IsGrounded = characterController.isGrounded;
		//transform.position
		ForcePlayerHeightToDrop();
		HittingWallLogic();
		IsFallingCheck();
		onUpdateCalled.Invoke();
		CheckForInputAfterResuming();


	}

	void CheckForInputAfterResuming()
    {
		if(paused != GlobalHelper.instance.IsPaused && !GlobalHelper.InMainMenu())
        {
			paused = GlobalHelper.instance.IsPaused;
			if (!receivingMovementInput && !paused)
			{
				moveAxis = Vector2.zero;
			}

			else if(paused)
            {
				receivingMovementInput = false;
			}
		}

		else if(GlobalHelper.InMainMenu())
        {
			receivingMovementInput = true;
			moveAxis = Vector2.zero;
		}
    }

    public bool IsGrounded
    {
        set
        {
			if(value != isGrounded)
            {
				value = characterController.isGrounded;
				playerAnimator.IsGrounded = value;
				characterEvents.onGrounded.Invoke();
				isGrounded = value;
				

				if (value == true)
                {
					movement.isMidair = false;
					//playerAnimator.AttemptIdleAnimationState();
				}
			}
        }

		get
        {
			 return characterController.isGrounded;
        }
    }

#if UNITY_EDITOR
	public void EditorResetPosition(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerTransform.position = originalPos;
		}
	}

	public void EditorSlowMo(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Time.timeScale = 0.2f;
		}
	}

	public void EditorNormalTime(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Time.timeScale = 1f;
		}
	}
#endif

	public StatsController StatsController
    {
        get
        {
			return statsController;
        }
    }

	public void BasicAttack(InputAction.CallbackContext context)
	{
		if (context.performed && !GlobalHelper.instance.IsPaused)
		{
			
			StatsController stats = RaycastStats();
			if(stats != null)
            {
		
				statsController.DealDamageToOther(stats);
				if(stats.TryGetComponent(out NPCScript npc))
                {
					npc.GetKnockBack(statsController.StatProfile);
                }

			}
			HaltMovement();
			characterEvents.onAttack.Invoke();
		}
	}

	void ApplyMovement()
	{

		float input = moveAxis.x;
		if (Mathf.Abs(input) < 0.3f)
		{
			input = 0f;
			playerAnimator.AttemptIdleAnimationState();
			//Animate Run

		}

		else if( canMove)
        {
			if(IsGrounded && !IsHittingWall)
            {
				playerAnimator.onMoveInputStateChange.Invoke(MovementState.Sprint);
			}

			else if(IsHittingWall)
            {
				playerAnimator.onMoveInputStateChange.Invoke(MovementState.idle);
			}
			
		}

		Vector2 desiredVelocity = new Vector2(input, characterController.velocity.y);
		desiredVelocity *= movement.maxSpeed;

		if (!pauseMove)
		{

			//Midair
			if (!IsGrounded)
			{
				if (movement.canControlMidAir)
				{
					desiredVelocity = Vector2.Lerp(characterController.velocity, desiredVelocity, Time.deltaTime * movement.airControlDamping);
					movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
				}

				movement.MoveVector = transform.TransformDirection(movement.MoveVector);
			}

			//On Ground
			else if (IsGrounded && canMove)
			{
				desiredVelocity = new Vector2(Mathf.LerpUnclamped(characterController.velocity.x, desiredVelocity.x, movement.controlDamping * Time.deltaTime), characterController.velocity.y);
				movement.MoveVector = new Vector3(desiredVelocity.x, desiredVelocity.y, 0);
				movement.MoveVector = transform.TransformDirection(movement.MoveVector);
			}

			//JUMP (when midair)
			if (!IsGrounded && !pauseMove)
			{
				moveDirection += movement.gravity * Time.deltaTime * (movement.jumpGravityMultiplier * GlobalHelper.instance.PlayerTimeScale);
			}

			if (canMove)
			{
				moveDirection = new Vector3(movement.MoveVector.x, moveDirection.y, movement.MoveVector.z);
			}

			else
			{
				moveDirection = new Vector3(0, moveDirection.y, movement.MoveVector.z);
			}
		}

		characterController.Move((moveDirection + secondaryMoveDirection) * Time.deltaTime);
	}

	public Vector3 SetMoveDirection
	{
		get => moveDirection;
		set
		{
			moveDirection = value;
		}
	}

	public void ReduceYMoveSpeedForFreeze()
	{
		if (!isGrounded)
		{
			float predictedApexOfJump = movement.JumpTarget + movement.HeightWhenJumped;
			float progressOfJump = transform.position.y / predictedApexOfJump;
			moveDirection.y = (Mathf.Sqrt((movement.jumpHeight * GlobalHelper.instance.PlayerTimeScale) * -1.0f * movement.gravity.y)) * (1 - progressOfJump);
		}
	}

	public void Jump(InputAction.CallbackContext callbackContext)
	{
		if(callbackContext.performed && !GlobalHelper.instance.IsPaused)
        {

            if (IsGrounded)
            {
				movement.isMidair = true;
				movement.HeightWhenJumped = transform.position.y;
				movement.JumpTarget = Mathf.Sqrt((movement.jumpHeight * GlobalHelper.instance.PlayerTimeScale) * -3.0f * movement.gravity.y);
				moveDirection.y = movement.JumpTarget;
				characterEvents.onJumped.Invoke();

				playerAnimator.onJump.Invoke();
			}
			
		}
	}

	public void MovementVector(InputAction.CallbackContext callbackContext)
	{
		if (!GlobalHelper.instance.IsPaused)
        {
			moveAxis = callbackContext.ReadValue<Vector2>();


			//Facing Direction
			if (moveAxis.x < 0)
			{
				IsFacingRight = false;
				receivingMovementInput = true;
			}

			else if (moveAxis.x > 0)
			{
				IsFacingRight = true;
				receivingMovementInput = true;

			}

			else
			{
				receivingMovementInput = false;

			}
		}
		
	}

	public void Raycast(InputAction.CallbackContext context)
	{

		if (context.performed && _raycast.useRaycast)
		{
			
		}
	}

	StatsController RaycastStats()
    {
		StatsController newController = null;
		if(_raycast.useRaycast)
        {
			RaycastHit hit;
			Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y +1, _raycast.raycastPoint.position.z);
			Debug.DrawRay(origin, Vector2.right * (isFacingRight ? 1 : -1) * _raycast.raycastDistance, _raycast.aboveCheckRaycastColour);
			if (Physics.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), out hit, _raycast.raycastDistance, _raycast.raycastMask))
			{
				//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
				 if (hit.transform.TryGetComponent(out StatsController stats))
				{
					newController = stats;
				}
			}
		}

		return newController;
    }



	public bool CheckInputState()
    {
		return receivingMovementInput;
    }

	public bool IsFacingRight
    {
		set
        {
			isFacingRight = value;
			FlipDirectionBasedObjects();

		}
		
        get
        {
			return isFacingRight;
        }
    }


	void ForcePlayerHeightToDrop()
    {
		if(HitObjectAbove() == true)
        {
			moveDirection.y = 0;
        }
	}

	public void HittingWallLogic()
    {
		isHittingWall = HittingObjectInfrontWithoutRigidBody();
    }

	public bool HitObjectAbove()
    {
		bool isHitting = false;
		RaycastHit hit;
		Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z);
		Debug.DrawRay(origin, transform.TransformDirection(Vector2.up) * _raycast.aboveCheckDistance, _raycast.aboveCheckRaycastColour);
		if (Physics.Raycast(origin,transform.TransformDirection(Vector2.up),out hit, _raycast.aboveCheckDistance,_raycast.raycastMask))  
		{
			//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
			if (hit.transform != null)
			{
				isHitting = true;
			}

		}
		return isHitting;
	}

	public bool HittingObjectInfrontWithoutRigidBody()
    {
		bool isHitting = false;
		RaycastHit hit;
		Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z);
		Debug.DrawRay(origin, Vector2.right * (isFacingRight ? 1 : -1) * _raycast.wallCheckDistance, _raycast.wallCheckRaycastColour);
		if (Physics.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), out hit, _raycast.wallCheckDistance, _raycast.raycastMask))
		{
			//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
			if (hit.rigidbody == null)
			{
				isHitting = true;
			}

			else if (hit.transform.TryGetComponent(out InteractableDimensionObject interactable))
			{
				isHitting = true;
			}

			else if(hit.transform.TryGetComponent(out NPCScript NPCScript))
            {
				isHitting = true;
            }
		}
		return isHitting;
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
			RigidBodyPhysics(hit);
	}

	public void RigidBodyPhysics(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;


		// no rigidbody
		if (body == null || body.isKinematic)
			return;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f)
			return;

		// Calculate push direction from move direction,
		// we only push objects to the sides never up and down
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

		// If you know how fast your character is trying to move,
		// then you can also multiply the push velocity by that.

		// Apply the push
		body.velocity = pushDir * physicsPushPower;

	}

	public UnityEvent UpdateEvent
    {
		get
        {
			return onUpdateCalled;
        }
    }

	public Vector2 MoveAxis
    {
		get
        {
			return moveAxis;
        }
    }

	public bool IsHittingWall
    {
        get
        {
			return isHittingWall;
        }
    }

	public bool CanMove
    {
		get
        {
			return canMove;
        }
    }

	void InitAllDirectionBasedObjects()
    {
        foreach (DirectionBasedObjectFlip item in directionBasedObjectFlips)
        {
			item.Init();
        }
    }

	void FlipDirectionBasedObjects()
    {
		foreach (DirectionBasedObjectFlip item in directionBasedObjectFlips)
		{
			item.FlipObject(IsFacingRight);
		}
	}

	public void IsFallingCheck()
    {

			bool isFalling = characterController.velocity.y < movement.fallVelocityBuffer && !IsGrounded;
			if(movement.isFalling != isFalling)
            {
				movement.isFalling = isFalling;
			if(isFalling)
            {
				characterEvents.onFalling.Invoke();

			}
				
			}
    }

	public bool IsFalling
    {
		get
        {
			return movement.isFalling;
		}
    }

	public void HaltMovement(InputAction.CallbackContext context)
    {
		if(context.performed)
        {
			HaltMovement();
        }
    }

	public void HaltMovement()
    {
		if (movement.stopMovementWhenAttacking == true)
        {
			if (movement.StopMovementCoroutine != null)
			{
				//StopCoroutine(movement.StopMovementCoroutine);
				//movement.StopMovementCoroutine = null;
			}
			movement.StopMovementCoroutine = StartCoroutine(StopMovement());
		}
		
	}

	IEnumerator StopMovement()
    {
		canMove = false;
		yield return new WaitForSeconds(movement.movementStopTime);
		canMove = true;

	}


	/// <summary>
	/// COMBAT LOGIC
	/// </summary>
	/// <param name="other"></param>


    private void OnTriggerEnter(Collider other)
    {
		//Enemy collision
		if(other.transform.TryGetComponent(out NPCScript npc))
        {
			npcScript = npc;
			npc.Attack();
			
		}
    }

	public void GetKnockBack()
	{
		float newDirection = 0;
		if (npcScript != null)
		{
			//npcScript.StatsController.DealDamageToOther(statsController);

			if (IsObjectOnRight(transform, npcScript.transform))
			{
				newDirection = -npcScript.StatsController.StatProfile.KnockBackStrength;
			}

			else
			{
				newDirection = npcScript.StatsController.StatProfile.KnockBackStrength;
			}
		}
		npcScript = null;

		secondaryMoveDirection.x = newDirection;
		StartCoroutine(RemoveFromSecondaryMoveDirection());
	}

	public IEnumerator RemoveFromSecondaryMoveDirection()
    {
		float t = 0; ;
		
		while (secondaryMoveDirection != Vector3.zero)
        {
			t += Time.deltaTime;
			secondaryMoveDirection = Vector3.Lerp(secondaryMoveDirection, Vector3.zero, t/knockbackTime);

			yield return null;
		}

		yield return null;
	}

	public static bool IsObjectOnRight(Transform player,Transform other)
    {
		bool value = false;
		if(player.position.x < other.position.x)
        {
			value = true;
			

		}
		return value;
    }
}

[System.Serializable]
public class RaycastDetails
{
	public bool useRaycast;
	public LayerMask raycastMask;
	[Tooltip("This decides where the raycast comes from Leave this variable blank for it to default to this gameobject.")]
	public Transform raycastPoint;
	public float raycastDistance;
	public Color raycastColour;

	[Space(10)]

	public float aboveCheckDistance = .5f;
	public Color aboveCheckRaycastColour;

	[Space(10)]

	public float wallCheckDistance = .5f;
	public Color wallCheckRaycastColour;
}

[System.Serializable]
public class MovementDetails
{
	public Vector3 gravity = new Vector3(0, 20.0f, 0);
	public float jumpGravityMultiplier = 10;

	[Space(10)]

	public float fallVelocityBuffer = .03f;
	[HideInInspector] public bool isFalling;
	[Range(1.25f,15f)] public float controlDamping = 7f;
	public float maxSpeed = 6;
	public float jumpHeight = 8;
	float heightWhenJumped;
	float jumpTarget;

	Vector3 moveVector;

	[Space(10)]

	public bool isMidair;
	public bool canControlMidAir = true;
	[Range(1.25f, 15f)] public float airControlDamping = 2.5f;

	[Space(10)]
	public bool stopMovementWhenAttacking;
	public float movementStopTime;
	Coroutine stopCoroutine;
		

	public Coroutine StopMovementCoroutine
    {
		set
        {
			stopCoroutine = value;
        }

		get
        {
			return stopCoroutine;
        }
    }

	public Vector3 MoveVector
	{
		set
		{
			moveVector = value;
		}

		get
		{
			return moveVector;
		}
	}

	public float HeightWhenJumped
    {
		get
        {
			return heightWhenJumped;

		}

        set
        {
			heightWhenJumped = value;
        }
    }

	public float JumpTarget
    {
		get
        {
			return jumpTarget;
        }

		set
        {
			jumpTarget = value;
        }
    }
}

[System.Serializable]
public class CharacterEvents
{
	[Header("Jump")]
	public UnityEvent onGrounded;
	public UnityEvent onJumped;
	public UnityEvent onFalling;
	public UnityEvent onAttack;
	public UnityEvent onAbilityUsed;

	[Header("Life")]
	public UnityEvent onDeath;
	public UnityEvent onKill;
	public UnityEvent onRespawnOrSpawn;
}

