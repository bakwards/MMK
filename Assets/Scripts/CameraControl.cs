using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class CameraControl : MonoBehaviour {

	public Rigidbody controlTarget;
	private Vector2 lastPanPosition;
	private SmoothFollow smoothFollow;
	public float maxCamHeight = 3;
	public float minCamHeight = 1;
	public float maxDistance = 5;
	
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
		Debug.Log("smoothfollowheight1: " + smoothFollow.height);
		smoothFollow.height /= gesture.LocalDeltaScale;
		Debug.Log("smoothfollowheight2: " + smoothFollow.height);
		smoothFollow.height = Mathf.Clamp (smoothFollow.height, minCamHeight, maxCamHeight);
		Debug.Log("smoothfollowheight3: " + smoothFollow.height);
		smoothFollow.distance = maxDistance * smoothFollow.height / maxCamHeight;
	}

}
