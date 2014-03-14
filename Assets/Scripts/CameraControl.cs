using UnityEngine;
using System.Collections;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class CameraControl : MonoBehaviour {

	public Rigidbody controlTarget;
	private Vector2 lastPanPosition;
	public SmoothFollow smoothFollow;
	public float maxCamHeight = 3;
	public float minDistance = 2;
	public float maxDistance = 6;
	public float camDistanceTreshold = 3;
	public FullscreenTarget fullscreenTarget;
	
	void Start () {
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
		GetComponent<SimpleScaleGesture>().StateChanged += HandleScaleStateChanged;
		if(smoothFollow == null){
			smoothFollow = GetComponent<SmoothFollow>();
		}
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
			if(fullscreenTarget.Type == FullscreenTarget.TargetType.Background){
				fullscreenTarget.Type = FullscreenTarget.TargetType.Foreground;
			}
		} else {
			smoothFollow.height = 0.1f;
			if(fullscreenTarget.Type == FullscreenTarget.TargetType.Foreground){
				fullscreenTarget.Type = FullscreenTarget.TargetType.Background;
			}
		}
	}

}
