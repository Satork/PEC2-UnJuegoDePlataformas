using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(Animator))]
public class EnemyAI : MonoBehaviour {
	
	public GameObject player;
	public LayerMask platformLayerMask;
	public float speed = 2.5f;	//Enemies Move speed in scene
	
	private BoxCollider2D _enemyBoxCollider;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Animator _playerAnimator;
	private PlayerController _playerController;
	
	private float _direction;
	private static readonly int IsDead = Animator.StringToHash("isDead");
	private const float Distance = 0.1f; // BoxCast distance cast
	
	// Initialization of variables
	private void Start() {
		_enemyBoxCollider = GetComponent<BoxCollider2D>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_playerAnimator = player.GetComponent<Animator>();
		_playerController = player.GetComponent<PlayerController>();
		_direction = -1f;	// Enemies will start walking from right to left
		enabled = false;
	}

	private void Update() {
		var bounds = _enemyBoxCollider.bounds;
		
		// TODO Change direction through OnCollisionEnter
		
		// Boxcast to detect collisions from left and right sides
		bool isTouchingLeft = Physics2D.BoxCast(bounds.center, bounds.size, 0f,
		                                        Vector2.left, Distance, platformLayerMask);
		bool isTouchingRight = Physics2D.BoxCast(bounds.center, bounds.size, 0f,
		                                         Vector2.right, Distance, platformLayerMask);
		
		//Direction change
		if (isTouchingLeft) _direction = 1f;
		if (isTouchingRight) _direction = -1f;
		
		// Moves the enemy around the scene
		_rigidbody.velocity = new Vector2(_direction * speed, _rigidbody.velocity.y);
		
		//OnDrawDebugLines(isTouchingLeft, isTouchingRight);
	}
	
	// Deprecated code. Now using OnCollisionEnter2D
	/*private void OnTriggerEnter2D(Collider2D other) {
		if (!other.Equals(_playerBoxCollider)) return;
		var bounds = _enemyBoxCollider.bounds;
		Debug.Log(other.Equals(_playerBoxCollider));
		bool isEnteringThroughTop = Physics2D.BoxCast(new Vector2(bounds.center.x, bounds.max.y), new Vector2(bounds.size.x - 0.1f, bounds.size.y), 0f,
		                                              Vector2.up, Distance, playerLayerMask);
		Debug.Log(isEnteringThroughTop);
		if (isEnteringThroughTop) {
			_playerController.IsHit = true;
			_animator.SetBool(IsDead, true);
			float timerCounDown = 0.3f;
			while (timerCounDown > 0) {
				timerCounDown -= Time.deltaTime;
			}
			gameObject.SetActive(false);
		}
		else {
			_playerController.enabled = false;
			_playerAnimator.SetBool(IsDead, true);
		}
		enabled = false;
	}*/
	
	// Kills enemy if player jump on top. Otherwise, it kills player. If player is in Super Mode, it downgrades to
	// normal mode.
	private void OnCollisionEnter2D(Collision2D collision) {
		if(!collision.gameObject.CompareTag("Player")) return;

		var contact = collision.GetContact(0);

		if (contact.normal.Equals(Vector2.down)) { // Collision is from above => Player kill
			_playerController.IsHit = true;
			_animator.SetBool(IsDead, true);
			float timerCounDown = 1f;
			while (timerCounDown > 0) {
				timerCounDown -= Time.deltaTime;
			}
			// TODO Add points to the count
			Destroy(gameObject);
		}
		else { // Collision is not Player kill => Player death
			if (PlayerController.IsSuperMode) { // Player is in Super mode => Downgrades player to normal
				_playerController.SuperMarioToggle(false);
			}
			else {
				_playerController.enabled = false;
				_playerAnimator.SetBool(IsDead, true);
				enabled = false;
				// TODO Add Game Over screen
			}
		}
	}

	private void OnBecameVisible() {
		enabled = true;
	}

	/*private void OnDrawDebugLines(bool isTouchingLeft, bool isTouchingRight) {
		Color leftColor = Color.green;
		Color rightColor = Color.green;
		if (isTouchingLeft) {
			leftColor  = Color.red;
		}

		if (isTouchingRight) {
			rightColor = Color.red;
		}

		var bounds = _enemyBoxCollider.bounds;
		Vector3 leftTopPos = new Vector3(bounds.min.x - Distance, bounds.max.y, 0f);
		Vector3 leftBotPos = new Vector3(bounds.min.x - Distance, bounds.min.y, 0f);
		
		Vector3 rightTopPos = new Vector3(bounds.max.x + Distance, bounds.max.y, 0f);
		Vector3 rightBotPos = new Vector3(bounds.max.x + Distance, bounds.min.y, 0f);
		
		Debug.DrawLine(new Vector3(bounds.min.x, bounds.max.y, 0f), leftTopPos, leftColor);
		Debug.DrawLine(_enemyBoxCollider.bounds.min, leftBotPos, leftColor);
		Debug.DrawLine(leftTopPos, leftBotPos, leftColor);
		
		Debug.DrawLine(bounds.max, rightTopPos, rightColor);
		Debug.DrawLine(new Vector3(bounds.max.x, bounds.min.y, 0f), rightBotPos, rightColor);
		Debug.DrawLine(rightTopPos, rightBotPos, rightColor);
		
		Debug.DrawRay(new Vector3(bounds.center.x, bounds.max.y) + Vector3.right * bounds.extents.x, Vector3.up * Distance, Color.magenta);
		Debug.DrawRay(new Vector3(bounds.center.x, bounds.max.y) + Vector3.left * bounds.extents.x, Vector3.up *  Distance, Color.magenta);
		Debug.DrawRay(new Vector3(bounds.center.x, bounds.max.y) + (bounds.extents + Vector3.up * Distance), Vector3.left * Distance, Color.magenta);

	}*/
}
