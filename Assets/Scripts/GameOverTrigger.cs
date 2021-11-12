using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour {

	public GameObject player;

	private Animator _animator;
	
	private static readonly int IsDead = Animator.StringToHash("isDead");

	private void Start() {
		_animator = player.GetComponent<Animator>();
	}

	private void Update() {
		if(!_animator.GetBool(IsDead)) return;

		SceneManager.LoadScene("Game");
	}
}