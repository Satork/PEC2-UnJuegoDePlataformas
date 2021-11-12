using System;
using Unity.VisualScripting;
using UnityEngine;

public class WinController : MonoBehaviour {

	public Transform flag;
	public Transform downFlagPos;

	public float downSpeed = 1f;

	private bool _isWin;

	private void Update() {
		if (!_isWin) return;
		// TODO Add total score + time
		// TODO Add Win Screen
		flag.position = Vector3.MoveTowards(flag.position, downFlagPos.position, downSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D contact) {
		if (!contact.CompareTag("Player")) return;
		_isWin = true;
	}

}
