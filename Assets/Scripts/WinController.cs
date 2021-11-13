using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour {

	public Transform flag;
	public Transform downFlagPos;
	public Canvas canvas;
	
	public float downSpeed = 1f;

	private bool _isWin;

	private void Awake() {
		canvas.enabled = false;
	}

	private void Update() {
		if (!_isWin) return;
		// TODO Add total score + time
		flag.position = Vector3.MoveTowards(flag.position, downFlagPos.position, downSpeed * Time.deltaTime);
		if (flag.position.Equals(downFlagPos.position)) {
			_isWin = false;
			StartCoroutine(WinCoroutine());
		}
	}

	private void OnTriggerEnter2D(Collider2D contact) {
		if (!contact.CompareTag("Player")) return;
		_isWin = true;
	}

	private IEnumerator WinCoroutine() {
		canvas.enabled = true;
		while (!Input.anyKey){
			yield return null;
		}
		SceneManager.LoadScene("Game");
	}
}
