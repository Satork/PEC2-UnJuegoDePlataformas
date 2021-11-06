using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform target;
	public float smoothing;

	private void LateUpdate() {
		if (transform.position != target.position) {
			var transformPositionVar = transform.position;
			var targetPositionVar = target.position;
			Vector3 targetPosition = new Vector3(targetPositionVar.x,
			                                     targetPositionVar.y,
			                                     transformPositionVar.z);
			transformPositionVar = Vector3.Lerp(transformPositionVar, targetPosition, smoothing);
			transform.position = transformPositionVar;
		}
	}
}