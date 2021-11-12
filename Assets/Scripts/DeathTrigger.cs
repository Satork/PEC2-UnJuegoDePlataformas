using System;
using UnityEditor.UIElements;
using UnityEngine;

// Activates when Player falls to its death
[RequireComponent(typeof(Collider2D))]
public class DeathTrigger : MonoBehaviour {

	public GameObject player;

	private PlayerController _playerController;
	private Rigidbody2D _playerRigidbody2D;
	private Animator _playerAnimator;
	
	private static readonly int IsDead = Animator.StringToHash("isDead");

	private void Start() {
		_playerController = player.GetComponent<PlayerController>();
		_playerRigidbody2D = player.GetComponent<Rigidbody2D>();
		_playerAnimator = player.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.CompareTag("Enemy") || other.CompareTag("PowerUp")) {		// If enemy or powerup falls, they get destroyed: intended to clear possible lag issues and improve performance
			Destroy(other.gameObject);
		}
		
		if (!other.CompareTag("Player")) return;
	
		_playerController.enabled = false;
		_playerRigidbody2D.bodyType = RigidbodyType2D.Static;
		_playerAnimator.SetBool(IsDead, true);
		// TODO Add Game Over screen 
	}
}
