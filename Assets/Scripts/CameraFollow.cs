using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float smoothSpeed = 10f;
	public float offset = 0.4f;

	public Vector2 maxCameraPositions;
	public Vector2 minCameraPositions;
	
	private Vector3 _lastPos = Vector3.zero;
	private void Awake() {
		_lastPos = transform.position;
	}

	private void FixedUpdate() {
		var position = transform.position;
		Vector3 clampedTargetPosition = position;
		var targetPosition = target.position;

		var advancedPosition = targetPosition - _lastPos;
		if (advancedPosition.x - advancedPosition.x * offset > 0f)
			clampedTargetPosition.x = Mathf.Clamp(targetPosition.x, minCameraPositions.x, maxCameraPositions.x);
		clampedTargetPosition.y = Mathf.Clamp(targetPosition.y, minCameraPositions.y, maxCameraPositions.y);
		
		position = Vector3.Lerp(position, clampedTargetPosition, smoothSpeed * Time.deltaTime);
		transform.position = position;

		_lastPos = position;
	}
}
