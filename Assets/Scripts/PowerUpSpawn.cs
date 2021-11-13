using UnityEngine;

// Base spawn that makes all different powerups enter the screen. 
[RequireComponent(typeof(BoxCollider2D))]
public class PowerUpSpawn : MonoBehaviour {

	public TileData tileData;
	
	[HideInInspector]
	public new Transform transform;
	[HideInInspector]
	public BoxCollider2D boxCollider2D; // Collider that will allow the powerup to move through the scene
	[HideInInspector]
	public new Rigidbody2D rigidbody2D;
	
	private Vector3 _targetPosition;
	
	[HideInInspector]
	public bool hasSpawned;
	
	[HideInInspector]
	public bool hasComponent;
	
	// Initialization of the variables
	public virtual void Start() {
		transform = GetComponent<Transform>();
		_targetPosition = tileData.targetPosition;
		boxCollider2D = GetComponent<BoxCollider2D>();

		boxCollider2D.enabled = false;
	}

	// Moves object from spawn position to target position
	public virtual void Update() {
		if (!transform.position.Equals(_targetPosition) && !hasSpawned)
			((Component) this).transform.position =
				Vector3.MoveTowards(((Component) this).transform.position, _targetPosition, tileData.spawnSpeed * Time.deltaTime);
		else {
			if (!hasComponent) {
				hasSpawned = true;
				boxCollider2D.enabled = true;
				rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
				rigidbody2D.gravityScale = 5f;
				rigidbody2D.freezeRotation = true;
				hasComponent = true;
			}
		}
	}
	
}
