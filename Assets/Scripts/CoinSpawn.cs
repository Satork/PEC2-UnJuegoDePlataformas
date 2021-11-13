using UnityEngine;

public class CoinSpawn : MonoBehaviour {

	public float despawnTime = 1f;
	public TileData tileData;
	
	private float _timerCountDown;
	private Transform _transform;
	private Vector3 _targetPos;
	
	// Initialization of variables
	private void Start() {
		_transform = GetComponent<Transform>();
		_timerCountDown = despawnTime;
		_targetPos = tileData.targetPosition;
	}
	
	// Moves coin object to target Position and destroys it when timer has finished
	private void Update() {
		_timerCountDown -= Time.deltaTime;
		_transform.position = Vector3.MoveTowards(_transform.position, _targetPos, tileData.spawnSpeed * Time.deltaTime);
		if (!transform.position.Equals(_targetPos) && _timerCountDown > 0) return;
		// TODO Add coin score to total score
		Destroy(gameObject);
	}
}
