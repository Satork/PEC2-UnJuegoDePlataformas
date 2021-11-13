using UnityEngine;

public class MushroomPowerUp : PowerUpSpawn {

	private float _direction = 1f;

	// TODO Add detection system to switch Mushroom to Flower if Player is in SuperMode and backwards
	// Moves the object when the spawn animation is finished
	public override void Update() {
		base.Update();
		if (!hasSpawned || !hasComponent) return;
		rigidbody2D.velocity = new Vector2(_direction * tileData.moveSpeed, rigidbody2D.velocity.y);
	}

	// Gives Player Super and destroys the object, when the player touches the object
	private void OnTriggerEnter2D(Collider2D contact) {
		//Debug.Log("Player Tag: " + contact.tag);
		if (!contact.CompareTag("Player")) return;
		var playerController = contact.gameObject.GetComponent<PlayerController>();
		if (!PlayerController.IsSuperMode)
			playerController.SuperMarioToggle(true);
		Destroy(gameObject);
	}

	// Changes Mushroom direction if it hits platform form left or right
	private void OnCollisionEnter2D(Collision2D collision) {
		//Debug.Log("OnCollisionStay gameobject:" + collision);
		if (!collision.gameObject.CompareTag("Brick") && !collision.gameObject.CompareTag("Platform")) return;
		var contact = collision.GetContact(0);
		var contactNormal = contact.normal;
		//Debug.Log("OnCollisionEnter normal: " + contactNormal);
		if (contactNormal.Equals(Vector2.left)) _direction = 1f;
		if (contactNormal.Equals(Vector2.right)) _direction = -1f;
	}

	// private void OnCollisionStay2D(Collision2D collision) {
	// 	Debug.Log("OnCollisionStay gameobject:" + collision);
	// 	if (!collision.gameObject.CompareTag("Brick") && !collision.gameObject.CompareTag("Platform")) return;
	// 	var contact = collision.GetContact(0);
	// 	var contactNormal = contact.normal;
	// 	Debug.Log("OnCollisionStay normal: " + contactNormal);
	// 	if (contactNormal.Equals(Vector2.left)) _direction = -1f;
	// 	if (contactNormal.Equals(Vector2.right)) _direction = 1f;
	// }
}
