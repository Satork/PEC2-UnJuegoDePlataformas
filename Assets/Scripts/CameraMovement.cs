using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform target;
	public float smoothing;

	public Vector2 maxPosition;
	public Vector2 minPosition;

	private void LateUpdate() {
		if (transform.position != target.position) {
			var transformPositionVar = transform.position;
			var targetPositionVar = target.position;
			Vector3 targetPosition = new Vector3(targetPositionVar.x, targetPositionVar.y, transformPositionVar.z);
			targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
			targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

			
			transformPositionVar = Vector3.Lerp(transformPositionVar, targetPosition, smoothing);
			transform.position = transformPositionVar;
			
		}
	}
}