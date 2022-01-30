using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[RequireComponent(typeof(NPCAnimation))]
public class NPCScript : MonoBehaviour
{
	//public GameController gameController;

	bool receivingMovementInput;
	[SerializeField] bool canMove = true;




	[Header("NPC Settings")]
	[SerializeField] NPCAnimation npcAnimation;
	[SerializeField] CharacterController characterController;
	[SerializeField] StatsController stats;
	[SerializeField] float physicsPushPower;
	[SerializeField] Vector2 seekPlayerDistanceThreshold = new Vector2(1,3);
	[Range(0,10)] [SerializeField] float seekPlayerMoveSpeed = 3;
	[SerializeField] float knockbackTime;
	Vector3 playerPosition;
	Vector3 secondaryMoveDirection;

	[SerializeField] MovementInfo movement;
	[SerializeField] internal CharacterEvents characterEvents;
	[SerializeField] List<DirectionBasedObjectFlip> directionBasedObjectFlips;
	Vector3 moveDirection = Vector3.zero;
	UnityEvent onUpdateCalled = new UnityEvent();


	[HideInInspector] public bool characterGrabbed = false;



	[SerializeField] bool isFacingRight;

	[Header("Raycast")]
	[SerializeField] RaycastInfo _raycast;

	[Header("Debug")]
	[SerializeField] Transform playerTransform;
	Vector3 originalPos;
	[SerializeField] bool isGrounded;
	[SerializeField] bool isHittingWall;

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
		originalPos = playerTransform.position;
		npcAnimation.Init();
		InitAllDirectionBasedObjects();
		InitStatEvents();
	}

	void InitStatEvents()
    {
		stats.onDeath.AddListener(delegate { npcAnimation.Death(); });
		stats.onDeath.AddListener(delegate { canMove = false;  });
		stats.onHealthLost.AddListener(delegate { npcAnimation.TookDamage(); });
		stats.onHealthLost.AddListener(delegate { GetKnockBack(PlayerController.instance.StatsController.StatProfile); });
		//characterEvents.onAttack.AddListener(delegate { npcAnimation.at });


	}

	public CharacterController PlayerCharacterController
	{
		get
		{
			return characterController;
		}
	}

	public StatsController StatsController
    {
		get
        {
			return stats;
        }
    }

	public void GetKnockBack(StatsDetails statProfile)
    {

			//StatsController.DealDamageToOther(StatsController);

			float newDirection = 0;
			if (PlayerController.IsObjectOnRight(transform, PlayerController.instance.transform))
			{
				newDirection = -statProfile.KnockBackStrength;
			}

			else
			{
				newDirection = statProfile.KnockBackStrength;
			}
			secondaryMoveDirection.x = newDirection;
			StartCoroutine(RemoveFromSecondaryMoveDirection());

	}

	public IEnumerator RemoveFromSecondaryMoveDirection()
	{
		float t = 0; ;

		while (secondaryMoveDirection != Vector3.zero)
		{
			t += Time.deltaTime;
			secondaryMoveDirection = Vector3.Lerp(secondaryMoveDirection, Vector3.zero, t / knockbackTime);

			yield return null;
		}
		yield return null;
	}

	void OnBecameInvisible()
	{
		canMove = false;
	}

	void OnBecameVisible()
	{
		canMove = true;
	}


	// Main tick
	void FixedUpdate()
	{
		//Movement();
		
			MovementVector();
		

		IsGrounded = characterController.isGrounded;

	}

	public void Update()
	{
		ForcePlayerHeightToDrop();
		HittingWallLogic();
		IsFallingCheck();
		onUpdateCalled.Invoke();
		//playerPosition = 
	}

	public bool IsGrounded
	{
		set
		{
			if (value != isGrounded)
			{
				value = characterController.isGrounded;
				npcAnimation.IsGrounded = value;
				characterEvents.onGrounded.Invoke();
				isGrounded = value;
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


	public void Attack()
	{
		characterEvents.onAttack.Invoke();
	}


	public void MovementVector()
	{
		float input = 0;
		float distance = Vector3.Distance(PlayerController.instance.transform.position, transform.position);
		float xDistance = Mathf.Abs(PlayerController.instance.transform.position.x - transform.position.x);
		if (canMove)
		{

			if (distance < seekPlayerDistanceThreshold.y && distance > seekPlayerDistanceThreshold.x || xDistance < seekPlayerDistanceThreshold.y && xDistance > seekPlayerDistanceThreshold.x)
			{
				if (PlayerController.instance.transform.position.x > transform.position.x)
				{
					input = seekPlayerMoveSpeed;
				}

				else
				{
					input = -seekPlayerMoveSpeed;
				}

			}

			else
			{
				input = movement.horizontalMoveDirection;
			}



			//Facing Direction
			if (input < 0)
			{
				IsFacingRight = false;
				receivingMovementInput = true;
			}

			else if (input > 0)
			{
				IsFacingRight = true;
				receivingMovementInput = true;

			}

			else
			{
				receivingMovementInput = false;


			}



			//Position
			if (Mathf.Abs(input) > 0.3f && !characterGrabbed)
			{

				if (IsGrounded && !IsHittingWall)
				{
					npcAnimation.onMoveInputStateChange.Invoke(MovementState.Sprint);
				}

				else if (IsHittingWall)
				{
					npcAnimation.onMoveInputStateChange.Invoke(MovementState.idle);
				}

			}
			else
			{
				npcAnimation.AttemptIdleAnimationState();
			}
		}

		moveDirection += movement.gravity * (Time.deltaTime *GlobalHelper.instance.UniversalTimeScale);
	

	moveDirection = new Vector3(input, moveDirection.y, 0);

	

		if (!characterGrabbed)
			characterController.Move((moveDirection + secondaryMoveDirection) * (Time.deltaTime * GlobalHelper.instance.UniversalTimeScale));
		else
		{
			moveDirection = new Vector3(0, -2, 0);
		}


	}

	private void onPlayerFreezeAbility()
	{
		npcAnimation.NPCAnimator.speed = GlobalHelper.instance.UniversalTimeScale;
	}

	public void Raycast(InputAction.CallbackContext context)
	{

		if (context.performed && _raycast.useRaycast)
		{
			bool isHitting = false;
			RaycastHit hit;
			Vector3 origin = new Vector3(_raycast.raycastPoint.position.x, _raycast.raycastPoint.position.y + 1, _raycast.raycastPoint.position.z);
			Debug.DrawRay(origin, Vector2.right * (isFacingRight ? 1 : -1) * _raycast.raycastDistance, _raycast.aboveCheckRaycastColour);
			if (Physics.Raycast(origin, Vector2.right * (isFacingRight ? 1 : -1), out hit, _raycast.raycastDistance, _raycast.raycastMask))
			{
				//Below is the if statement to find objects. Can be used from Unity 2017 onwards, otherwise use GetComponent instead of TryGetComponent()
				if (hit.collider.TryGetComponent(out InteractableDimensionObject interactable))
				{
					interactable.DebugRaycastHit();
				}
			}
		}
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
		if (HitObjectAbove() == true)
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
		if (Physics.Raycast(origin, transform.TransformDirection(Vector2.up), out hit, _raycast.aboveCheckDistance, _raycast.raycastMask))
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
			if(item != null) 
			{ 
				item.Init();
			}
		}
	}

	void FlipDirectionBasedObjects()
	{
		foreach (DirectionBasedObjectFlip item in directionBasedObjectFlips)
		{
			if (item != null)
			{
				item.FlipObject(IsFacingRight);
			}
		}
	}

	public void IsFallingCheck()
	{

		bool isFalling = characterController.velocity.y < movement.fallVelocityBuffer && !IsGrounded;
		if (movement.isFalling != isFalling)
		{
			movement.isFalling = isFalling;
			if (isFalling)
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

	public float MovementValue
    {
		get
        {
			return movement.horizontalMoveDirection;
        }
    }
}

[System.Serializable]
public class RaycastInfo
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
public class MovementInfo
{
	public Vector3 gravity = new Vector3(0, 20.0f, 0);

	[Space(10)]

	public Vector3 velocity;
	[HideInInspector] public Vector3 lastPositon;
	public float horizontalMoveDirection;

	[Space(10)]

	public float fallVelocityBuffer = .03f;
	[HideInInspector] public bool isFalling;
	[Range(1.25f, 15f)] public float controlDamping = 7f;





}




