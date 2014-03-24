using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Gestures.Simple;

public class DrawerOpen : MonoBehaviour {

	private Vector2 lastDrawPosition;
	private float maxOpenDistance = 0.5f;
	private float minOpenPosition;

	// Use this for initialization
	void Start () {
		minOpenPosition = transform.localPosition.z;
		GetComponent<PressGesture>().StateChanged += HandleStateChanged;
		GetComponent<SimplePanGesture>().StateChanged += HandlePanStateChanged;
	
	}
	
	private void HandleStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		if (e.State == Gesture.GestureState.Recognized)	{
		}
	}
	private void HandlePanStateChanged(object sender, TouchScript.Events.GestureStateChangeEventArgs e){
		SimplePanGesture gesture = sender as SimplePanGesture;
		float deltaPosition = 0;
		if(e.State == Gesture.GestureState.Began){
			lastDrawPosition = gesture.ScreenPosition;
		} else {
			deltaPosition = lastDrawPosition.x - gesture.ScreenPosition.x;
			lastDrawPosition = gesture.ScreenPosition;
		}
		if(!float.IsNaN(deltaPosition)){
			deltaPosition *= 0.01f*Vector3.Dot(transform.right, transform.position-Camera.main.transform.position);
			deltaPosition = Mathf.Clamp(deltaPosition,
		                            minOpenPosition-transform.localPosition.z,
		                            minOpenPosition+maxOpenDistance-transform.localPosition.z);
			Vector3 newPosition = transform.position + transform.forward*deltaPosition;
			rigidbody.MovePosition(newPosition);
			if(minOpenPosition+maxOpenDistance-transform.localPosition.z < 0.01 && transform.FindChild("StoryController")){
				transform.FindChild("StoryController").gameObject.SetActive(true);
				Camera.main.GetComponent<CameraControl>().SetNewTarget(transform.FindChild("StoryController").gameObject);
				transform.FindChild("StoryController").parent = null;
			}
		}
		if (e.State == Gesture.GestureState.Recognized)	{
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
