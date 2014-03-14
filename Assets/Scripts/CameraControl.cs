using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class CameraControl : MonoBehaviour {

	public Rigidbody controlTarget;
	private Vector2 lastPanPosition;
	private SmoothFollow smoothFollow;
	public float maxCamHeight = 3;
	public float minDistance = 2;
	public float maxDistance = 6;
	public float camDistanceTreshold = 3;
	
	void Start () {
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
		GetComponent<SimpleScaleGesture>().StateChanged += HandleScaleStateChanged;
		smoothFollow = GetComponent<SmoothFollow>();
	}
	
	private void HandlePanStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if(controlTarget != null){
			float deltaRotation = 0;
			SimplePanGesture gesture = sender as SimplePanGesture;
			if(e.State == Gesture.GestureState.Began){
				lastPanPosition = gesture.ScreenPosition;
			} else {
				deltaRotation = gesture.ScreenPosition.x - lastPanPosition.x;
				lastPanPosition = gesture.ScreenPosition;
			}
			if(!float.IsNaN(deltaRotation)){
				deltaRotation = Mathf.Clamp(deltaRotation, -20, 20);
				Debug.Log("Pan? " + deltaRotation);
				controlTarget.MoveRotation(controlTarget.rotation * Quaternion.Euler(0, deltaRotation, 0));
			}
		}
	}
	private void HandleScaleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		SimpleScaleGesture gesture = sender as SimpleScaleGesture;
		smoothFollow.distance /= gesture.LocalDeltaScale;
		smoothFollow.distance = Mathf.Clamp (smoothFollow.distance, minDistance, maxDistance);
		if(smoothFollow.distance > camDistanceTreshold){
			smoothFollow.height = maxCamHeight * smoothFollow.distance / maxDistance;
		} else {
			smoothFollow.height = 0.1f;
		}
	}

}
