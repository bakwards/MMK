﻿using UnityEngine;
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
	public Collider commodeCollider;
	public GameObject bounceObject;
	public GameObject backButton;

	public float maxHeightMove;
	public float minHeightMove;
	public float maxSideMove=1;

	private Vector3 controlOriginalPosition;
	
	void Start () {
		controlOriginalPosition = controlTarget.transform.position;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
		GetComponent<SimpleScaleGesture>().StateChanged += HandleScaleStateChanged;
		GetComponent<TapGesture>().StateChanged += HandleTapStateChanged;
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
			if(e.State == Gesture.GestureState.Recognized){
				AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Swish"));
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
				deltaPosition.x = Mathf.Clamp(-deltaPosition.x, -maxSideMove-controlTarget.transform.localPosition.x, maxSideMove-controlTarget.transform.localPosition.x);
				deltaPosition.y = Mathf.Clamp(deltaPosition.y, minHeightMove-controlTarget.transform.localPosition.y, maxHeightMove-controlTarget.transform.localPosition.y); 
				controlTarget.MovePosition(controlTarget.transform.position+controlTarget.transform.up*deltaPosition.y + commodeCollider.transform.right*deltaPosition.x);
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
				ZoomOut();
			}
		} else {
			smoothFollow.height = 0.1f;
			if(fullscreenTarget.Type == FullscreenTarget.TargetType.Foreground){
				ZoomIn();
			}
		}
	}
	private void HandleTapStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if(e.State == Gesture.GestureState.Recognized){
			TapGesture gesture = sender as TapGesture;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(gesture.ScreenPosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider != null && hit.collider.name == "BounceObject"){
					bounceObject.SetActive(false);
					RaycastHit bounceHit;
					Ray bounceRay = new Ray(hit.point, -hit.collider.transform.forward);
					if(Physics.Raycast(bounceRay, out bounceHit)){
						controlTarget.MovePosition(bounceHit.point);
					}
					smoothFollow.height = 0.1f;
					smoothFollow.distance = minDistance;
					//controlTarget.MovePosition(hit.point);
					ZoomIn();
				}
			}
		}
	}
	private void HandleTapStateChangedOut(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if(e.State == Gesture.GestureState.Recognized){
			smoothFollow.distance = maxDistance;
			ZoomOut();
		}
	}

	public void SetNewTarget(GameObject target){
		controlTarget.MovePosition(target.transform.position-Vector3.up*0.16f);
		controlTarget.MoveRotation(target.transform.rotation);
		smoothFollow.height = 0.2f;
		smoothFollow.distance = 1.2f;
		GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChanged;
		GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChangedSearch;
		GetComponent<SimpleScaleGesture>().StateChanged -= HandleScaleStateChanged;

	}

	void ZoomIn(){
		GetComponent<TapGesture>().StateChanged -= HandleTapStateChanged;
		GetComponent<TapGesture>().StateChanged += HandleTapStateChangedOut;
		fullscreenTarget.Type = FullscreenTarget.TargetType.Background;
		controlTarget.MoveRotation(Quaternion.Euler(0,85,0));
		GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChanged;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChangedSearch;
		commodeCollider.enabled = false;
		bounceObject.SetActive(false);
		AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Swish"));
	}
	void ZoomOut(){
		GetComponent<TapGesture>().StateChanged -= HandleTapStateChangedOut;
		GetComponent<TapGesture>().StateChanged += HandleTapStateChanged;
		fullscreenTarget.Type = FullscreenTarget.TargetType.Foreground;
		controlTarget.MovePosition(controlOriginalPosition);
		GetComponent<SimplePanGesture>().StateChanged -= HandlePanStateChangedSearch;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
		commodeCollider.enabled = true;
		bounceObject.SetActive(true);
		AudioController.Instance.PlayClip((AudioClip)Resources.Load("Audio/SoundFX/Swish"));
	}

	public void SetFollowState(bool b){
		GetComponent<SmoothFollow>().enabled = b;
		GetComponent<BoxCollider>().enabled = b;
		if(b){
			backButton.SetActive(!b);
		}

	}
	public void SetBackButtonActive(bool b){
		backButton.SetActive(b);
	}
}
