using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour {

	public LayerMask groundLayerMask;
	public float speed = 9f;
	public float jumpForce = 12f;
	public float jumpTime = 0.35f;
	public float castHeight = 1f;
	public float brickPush = 5f;

	private Rigidbody2D _rigidbody;
	private BoxCollider2D _boxCollider;
	private Animator _animator;

	private float _jumpCountdown;
	// private const float Distance = .1f;	// Deprecated since collisions direction are now updated through OnCollisionEnter2D method

	public static bool IsCrouching { get; set; }
	public static bool IsSuperMode { get; set; }
	private bool IsJumping { get; set; }
	public bool IsHit { get; set; }

	private static readonly int Move = Animator.StringToHash("isMoving");
	private static readonly int Jump = Animator.StringToHash("isJumping");
	private static readonly int IsSuper = Animator.StringToHash("isSuper");

	// Inicialization of variables
	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_boxCollider = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
		_jumpCountdown = jumpTime;
		IsJumping = false;
		IsHit = true;
		IsSuperMode = false;
	}
	
	// Detect Jump and Crouch intentions and perform jump and crouch
	private void Update() {
		var bounds = _boxCollider.bounds;
		bool isGrounded = Physics2D.BoxCast(bounds.center, bounds.size,
		                                    0f, Vector2.down, castHeight,
		                                    groundLayerMask);
		//Debug.DrawRay(bounds.center, Vector3.down * castHeight, Color.green);
		// TODO Change jump key from Space to configurable JumpKey
		// Player jumps when JumpKey is active
		if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			_rigidbody.velocity = Vector2.up * jumpForce;
			_jumpCountdown = jumpTime;
			IsJumping = true;
		}
		
		//Continous Jumping: during a small period of time, allows player to jump heigher if Jump Key is active
		if (IsJumping && Input.GetKey(KeyCode.Space)) {
			if (_jumpCountdown > 0) {
				_rigidbody.velocity = Vector2.up * jumpForce;
				_jumpCountdown -= Time.deltaTime;
			}
			else IsJumping = false;
		}
		
		// If Player jumps on enemy, bounces up with a small jump force
		if (IsHit) {
			_rigidbody.velocity = Vector2.up * jumpForce;
			IsHit = false;
			//Debug.Log("Player bounced and killed enemy");
		}
		
		// TODO Add crouching mechanincs for Super Mario

		if (Input.GetKeyUp(KeyCode.Space)) IsJumping = false;
		//Debug.Log("IsGrounded: " + isGrounded);
		// Starts Jump animation
		_animator.SetBool(Jump, !isGrounded);

		// TODO Add Resizing of BoxCollider when changing Sprites. For this, method must be called only when sprite is changed. WIP
		
	}
	
	// Moves Player with a given Input and a Speed.
	private void FixedUpdate() {
		float moveInput = Input.GetAxis("Horizontal");
		
		// Depending on direction, rotates GameObject
		transform.eulerAngles = moveInput switch {
			                        < 0 => Vector3.up * 180f,
			                        > 0 => Vector3.zero,
			                        _ => transform.eulerAngles
		                        };
		
		// If player is Moving activate animation
		if (moveInput != 0) _animator.SetBool(Move, true);
		else if (moveInput == 0) _animator.SetBool(Move, false);
		_rigidbody.velocity = new Vector2(moveInput * speed, _rigidbody.velocity.y);
	}
	
	// When Player hits a brick it bounces back with a small force and the player is unable to perform continous Jump
	private void OnCollisionEnter2D(Collision2D collision) {
		if(!collision.gameObject.CompareTag("Brick")) return;
		
		var contact = collision.GetContact(0);
		var isHeadCollision = contact.normal.Equals(Vector2.down);
		
		if (!isHeadCollision) return;
		
		IsJumping = false;
		_rigidbody.velocity = Vector2.down * brickPush;
		
		// TODO Add same concept but when Player jumps and crouches, make player bounce up.
	}
	
	// Activates Player Super Mode and adjusts collider to match Sprite
	public void SuperMarioToggle(bool toggle) {
		IsSuperMode = toggle;
		_animator.SetBool(IsSuper, toggle);
	}

	// Deprecated OnTriggerEnter2D that used BoxCast to detect collision direction. Updated to use OnCollisionEnter2D
	// private void OnTriggerEnter2D(Collider2D other) {
	// 	if (other.CompareTag("Brick")) {
	// 		var bounds = _boxCollider.bounds;
	// 		bool isHeadCollision = Physics2D.Raycast(bounds.center, Vector2.up, bounds.extents.y + Distance, groundLayerMask);
	// 		Debug.Log("Head Collision Raycast: " + isHeadCollision);
	// 		if (!isHeadCollision) return;
	// 		Debug.Log("Head Collisioned!");
	// 		IsJumping = false;
	// 		_rigidbody.velocity = Vector2.down * brickPush;
	// 	}
	// }
}
