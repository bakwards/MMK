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

	public float maxHeightMove;
	public float minHeightMove;
	public float maxSideMove=1;

	private Vector3 controlOriginalPosition;
	
	void Start () {
		controlOriginalPosition = controlTarget.transform.position;
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
	private void HandlePanStateChangedSearch(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if(controlTarget != null){
			Vector2 deltaPosition = new Vector2(0,0);
			SimplePanGesture gesture = sender as SimplePanGesture;
			if(e.State == Gesture.GestureState.Began){
				lastPanPosition = gesture.ScreenPosition;
			} else {
				deltaPosition = gesture.ScreenPosition - lastPanPosition;
				lastPanPosition = gesture.ScreenPosition;
			}
			if(!float.IsNaN(deltaPosition.x)){
				deltaPosition *= -0.005f;
				deltaPosition.x = Mathf.Clamp(deltaPosition.x, -maxSideMove-controlTarget.transform.localPosition.x, maxSideMove-controlTarget.transform.localPosition.x);
				deltaPosition.y = Mathf.Clamp(deltaPosition.y, minHeightMove-controlTarget.transform.localPosition.y, maxHeightMove-controlTarget.transform.localPosition.y); 
				controlTarget.MovePosition(controlTarget.transform.position+controlTarget.transform.up*deltaPosition.y+controlTarget.transform.right*deltaPosition.x);
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
				controlTarget.MovePosition(controlOriginalPosition);
				GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChangedSearch;
				GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
			}
		} else {
			smoothFollow.height = 0.1f;
			if(fullscreenTarget.Type == FullscreenTarget.TargetType.Foreground){
				fullscreenTarget.Type = FullscreenTarget.TargetType.Background;
				controlTarget.MoveRotation(Quaternion.Euler(0,85,0));
				GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChanged;
				GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChangedSearch;
			}
		}
	}

}
