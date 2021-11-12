using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour {

	public GameObject player;
	public Canvas canvas;

	private Animator _animator;
	
	private bool _isDead;
	
	private static readonly int IsDead = Animator.StringToHash("isDead");

	private void Awake() {
		canvas.enabled = false;
	}

	private void Start() {
		_animator = player.GetComponent<Animator>();
	}

	private void Update() {
		if(!_animator.GetBool(IsDead)) return;
		if (_isDead) return;
		_isDead = true;
		StartCoroutine(GameOverCoroutine());
	}

	IEnumerator GameOverCoroutine() {
		canvas.enabled = true;
		while (!Input.GetKeyDown(KeyCode.R)) {
			yield return null;
		}
		SceneManager.LoadScene("Game");
	}
}